using Amazon;
using Amazon.Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace AwsAthenaMapper.Models
{
    public class AthenaConfig
    {
        public AWSCredentials Credentials { get; set; }

        public string QueryResultS3Location { get; set; }

        public RegionEndpoint RegionEndpoint { get; set; }

        public int TimeOutSecounds { get; set; }
    }
}
