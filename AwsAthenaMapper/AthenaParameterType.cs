using System;

namespace AwsAthenaMapper
{
    [Flags]
    public enum AthenaParameterType
    {
        Int16 = 0,
        Int32 = 1,
        Int64 = 2,
        Decimal = 4,
        Double = 8,
        Float = 16,
        Boolean = 32,
        String = 64,
        DateTime = 128,
        Array = 256
    }
}
