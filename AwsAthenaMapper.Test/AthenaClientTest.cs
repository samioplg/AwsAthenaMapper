using NUnit.Framework;
using Amazon.Athena;
using Moq;
using AwsAthenaMapper.Test.Mock;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Amazon.Athena.Model;
using System.Threading;

namespace AwsAthenaMapper.Test
{
    internal class AthenaClientTest
    {
        private Mock<IAmazonAthena> _amazonAthenaMock;
        private MockAthenaClient _mock;

        [SetUp]
        public void Setup()
        {
            _amazonAthenaMock = new Mock<IAmazonAthena>();
            _mock = new MockAthenaClient();
        }

        [Test]
        public async Task QueryFirstOrDefaultAsyncTest()
        {
            //Arrange
            _amazonAthenaMock.Setup(p => p.StartQueryExecutionAsync(It.IsAny<StartQueryExecutionRequest>(), It.IsAny<CancellationToken>()))
                             .ReturnsAsync(_mock.MockStartQueryExecutionResponse());
            _amazonAthenaMock.Setup(p => p.GetQueryExecutionAsync(It.IsAny<GetQueryExecutionRequest>(), It.IsAny<CancellationToken>()))
                             .ReturnsAsync(_mock.MockGetQueryExecutionResponse());
            _amazonAthenaMock.Setup(p => p.GetQueryResultsAsync(It.IsAny<GetQueryResultsRequest>(), It.IsAny<CancellationToken>()))
                             .ReturnsAsync(_mock.MockGetQueryResultsResponse());

            IAthenaClient client = new AthenaClient(_amazonAthenaMock.Object, "no-empty", 60 * 2);

            //Act
            var member = await client.QueryFirstOrDefaultAsync<Member>(@"select * from ""{_database}"".""{_table}"";");

            //Assert
            Assert.That(member, Is.Not.Null);
        }

        private class Member
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public decimal Amount { get; set; }

            public DateTime BirthDay { get; set; }

            public IEnumerable<string> Families { get; set; }
        }
    }
}
