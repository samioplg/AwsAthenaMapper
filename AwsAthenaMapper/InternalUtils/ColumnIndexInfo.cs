using Amazon.Athena.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace AwsAthenaMapper.InternalUtils
{
    internal class ColumnIndexInfo
    {
        public int Index { get; set; }
        public ColumnInfo ColumnInfo { get; set; }
    }
}
