using Amazon.Athena.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AwsAthenaMapper.Test.Mock
{
    internal class MockAthenaClient
    {
        internal StartQueryExecutionResponse MockStartQueryExecutionResponse()
        {
            var jsonData = File.ReadAllText("./Data/start-query.json");

            var data = JsonConvert.DeserializeObject<StartQueryExecutionResponse>(jsonData);

            return data;
        }


        internal GetQueryExecutionResponse MockGetQueryExecutionResponse()
        {
            var jsonData = File.ReadAllText("./Data/query-execution.json");

            var data = JsonConvert.DeserializeObject<GetQueryExecutionResponse>(jsonData);

            return data;
        }

        internal GetQueryResultsResponse MockGetQueryResultsResponse()
        {
            var jsonData = File.ReadAllText("./Data/query-result.json");

            var data = JsonConvert.DeserializeObject<GetQueryResultsResponse>(jsonData);

            return data;
        }
    }
}
