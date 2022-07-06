using AwsAthenaMapper.Models;
using AwsAthenaMapper;
using Amazon;
using System.Text.Json;
using System.Reflection;
using System.Collections;

namespace Example
{
    class Program
    {
        const string _accessKey = ""; //aws accesskey
        const string _secretKey = ""; //aws secretkey
        const string _resultLocation = ""; //athena query result location
        const string _database = ""; //aws glue database name
        const string _table = ""; // aws glue table name
         
        static readonly RegionEndpoint _region = null; //aws region
        public static async Task Main(string[] args)
        {
            var cred = new MyAwsCredential(_accessKey, _secretKey);

            IAthenaClient client = new AthenaClient(new AthenaConfig
            {
                Credentials = cred,
                QueryResultS3Location = _resultLocation,
                RegionEndpoint = _region,
                TimeOutSecounds = 60 * 2 // 2 mins
            });

            var query = @$"select * from ""{_database}"".""{_table}"" where Amount >= @amount and partition_0 in @years;";

            var p = new AthenaParameter();
            p.Add("amount", AthenaParameterType.Decimal, 400000m);
            p.Add("years", AthenaParameterType.String | AthenaParameterType.Array, new string[] { "2022" });

            var member = await client.QueryAsync<Member>(query, p);


            Console.WriteLine(JsonSerializer.Serialize(member, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            }));

        }
    }
}