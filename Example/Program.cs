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
        const string _accessKey = "***"; //aws accesskey
        const string _secretKey = "***"; //aws secretkey
        const string _resultLocation = "s3://***/"; //athena query result s3 location
        const string _database = "***"; //aws glue database name
        const string _table = "***"; // aws glue table name

        static readonly RegionEndpoint _region = RegionEndpoint.APSoutheast1; //aws region
        public static async Task Main(string[] args)
        {
            var cred = new MyAwsCredential(_accessKey, _secretKey);

            //IAthenaClient client = new AthenaClient(new AthenaConfig
            //{
            //    Credentials = cred,
            //    QueryResultS3Location = _resultLocation,
            //    RegionEndpoint = _region,
            //    TimeOutSecounds = 60 * 2 // 2 mins
            //});
            var client = new AthenaClient(new Amazon.Athena.AmazonAthenaClient(cred, _region), _resultLocation, 60 * 2);

            var query = @$"select * from ""{_database}"".""{_table}"";";
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