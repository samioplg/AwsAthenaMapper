using Amazon.Athena;
using Amazon.Athena.Model;
using AwsAthenaMapper.InternalUtils;
using AwsAthenaMapper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwsAthenaMapper
{
    public class AthenaClient : IAthenaClient
    {
        private readonly AmazonAthenaClient _client;

        private readonly string _queryLocation;

        private readonly int _timeoutSecounds;

        private const int SLEEP_MS = 500;
        public AthenaClient(AthenaConfig config)
        {
            if (config.Credentials == null)
            {
                throw new ArgumentException($"{nameof(config.Credentials)} can't be null ");
            }

            if (string.IsNullOrWhiteSpace(config.QueryResultS3Location))
            {
                throw new ArgumentException($"{nameof(config.QueryResultS3Location)} can't be null or empty ");

            }

            if (config.RegionEndpoint == null)
            {
                throw new ArgumentException($"{nameof(config.RegionEndpoint)} can't be null ");
            }


            if (config.TimeOutSecounds <= 0)
            {
                throw new ArgumentException("TimeOutSecounds Can't less than zero");
            }

            _client = new AmazonAthenaClient(config.Credentials, config.RegionEndpoint);
            _queryLocation = config.QueryResultS3Location;
            _timeoutSecounds = config.TimeOutSecounds;
        }


        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, AthenaParameter parameter = null) where T : class, new()
        {
            if (parameter != null && parameter.Parameters.Count != 0)
            {
                sql = parameter.Parameters.Aggregate(sql, (input, map) => input.Replace(map.Key, map.Value));
            }

            var exec = await _client.StartQueryExecutionAsync(new StartQueryExecutionRequest
            {
                QueryString = sql,
                ResultConfiguration = new ResultConfiguration
                {
                    OutputLocation = _queryLocation
                }
            });

            bool isWaiting = true;
            DateTime endTime = DateTime.Now.AddSeconds(_timeoutSecounds);
            while (isWaiting)
            {
                if (endTime <= DateTime.Now)
                {
                    throw new TimeoutException($"Query was timeout ExecutionId:{exec.QueryExecutionId}");
                }

                isWaiting = await WaitingQuery(exec.QueryExecutionId);

                if (isWaiting)
                {
                    await Task.Delay(SLEEP_MS);
                }
            }
            var result = await _client.GetQueryResultsAsync(new GetQueryResultsRequest
            {
                QueryExecutionId = exec.QueryExecutionId
            });

            var rows = result.ResultSet.Rows;
            var colMap = AthenaConvert.MapColumnsIndexInfo(rows[0].Data, result.ResultSet.ResultSetMetadata.ColumnInfo);
            rows.RemoveAt(0);

            return AthenaConvert.MappingToObject<T>(rows[0].Data, colMap);
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, AthenaParameter parameter = null) where T : class, new()
        {
            if (parameter != null && parameter.Parameters.Count != 0)
            {
                sql = parameter.Parameters.Aggregate(sql, (input, map) => input.Replace(map.Key, map.Value));
            }


            var exec = await _client.StartQueryExecutionAsync(new StartQueryExecutionRequest
            {
                QueryString = sql,
                ResultConfiguration = new ResultConfiguration
                {
                    OutputLocation = _queryLocation
                }
            });

            bool isWaiting = true;

            DateTime endTime = DateTime.Now.AddSeconds(_timeoutSecounds);
            while (isWaiting)
            {
                if (endTime <= DateTime.Now)
                {
                    throw new TimeoutException($"Query was timeout ExecutionId:{exec.QueryExecutionId}");
                }

                isWaiting = await WaitingQuery(exec.QueryExecutionId);

                if (isWaiting)
                {
                    await Task.Delay(SLEEP_MS);
                }
            }

            var result = await _client.GetQueryResultsAsync(new GetQueryResultsRequest
            {
                QueryExecutionId = exec.QueryExecutionId
            });

            var rows = result.ResultSet.Rows;
            var colMap = AthenaConvert.MapColumnsIndexInfo(rows[0].Data, result.ResultSet.ResultSetMetadata.ColumnInfo);
            rows.RemoveAt(0);

            return AthenaConvert.MappingToList<T>(rows, colMap);
        }

        private async Task<bool> WaitingQuery(string requestId)
        {
            bool isWaiting = true;
            var resp = await _client.GetQueryExecutionAsync(new GetQueryExecutionRequest
            {
                QueryExecutionId = requestId
            });
            var state = resp.QueryExecution.Status.State;


            if (state == QueryExecutionState.FAILED)
            {
                throw new AmazonAthenaException($"Query was Failed Error Message: {resp.QueryExecution.Status.StateChangeReason}");
            }

            if (state == QueryExecutionState.CANCELLED)
            {
                throw new AmazonAthenaException("Query was cancelled");
            }

            if (state == QueryExecutionState.SUCCEEDED)
            {
                isWaiting = false;
            }

            return isWaiting;

        }
    }
}
