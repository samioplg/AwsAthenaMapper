using Amazon.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example
{
    internal class MyAwsCredential : AWSCredentials
    {
        private readonly string AccessKey;

        private readonly string SecertKey;

        public MyAwsCredential(string accessKey, string secertKey)
        {
            AccessKey = accessKey;
            SecertKey = secertKey;
        }
        public override ImmutableCredentials GetCredentials()
        {
            return new ImmutableCredentials(AccessKey, SecertKey, null);
        }
    }
}
