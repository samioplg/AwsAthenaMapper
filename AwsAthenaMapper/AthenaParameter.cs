using System;
using System.Collections.Generic;
using System.Linq;

namespace AwsAthenaMapper
{
    public class AthenaParameter
    {
        private readonly Dictionary<string, string> _parameters = new Dictionary<string, string>();

        internal IReadOnlyDictionary<string, string> Parameters
        {
            get
            {
                return _parameters;
            }
        }

        public void Add(string name, AthenaParameterType type, object value)
        {
            switch (type)
            {
                case AthenaParameterType.Int16:
                    if (value.GetType().ToString() != typeof(Int16).ToString())
                    {
                        throw new ArgumentException($"Property: {name} is not Int16 type");
                    }
                    _parameters.Add($"@{name}", value.ToString());
                    break;

                case AthenaParameterType.Int32:
                    if (value.GetType().ToString() != typeof(Int32).ToString())
                    {
                        throw new ArgumentException($"Property: {name} is not Int32 type");
                    }
                    _parameters.Add($"@{name}", value.ToString());
                    break;

                case AthenaParameterType.Int64:
                    if (value.GetType().ToString() != typeof(Int64).ToString())
                    {
                        throw new ArgumentException($"Property: {name} is not Int64 type");
                    }
                    _parameters.Add($"@{name}", value.ToString());
                    break;

                case AthenaParameterType.Double:
                    if (value.GetType().ToString() != typeof(Double).ToString())
                    {
                        throw new ArgumentException($"Property: {name} is not Double type");
                    }
                    _parameters.Add($"@{name}", value.ToString());
                    break;

                case AthenaParameterType.Decimal:
                    if (value.GetType().ToString() != typeof(Decimal).ToString())
                    {
                        throw new ArgumentException($"Property: {name} is not Decimal type");
                    }
                    _parameters.Add($"@{name}", value.ToString());
                    break;

                case AthenaParameterType.Float:
                    if (value.GetType().ToString() != typeof(Single).ToString())
                    {
                        throw new ArgumentException($"Property: {name} is not Float type");
                    }
                    _parameters.Add($"@{name}", value.ToString());
                    break;

                case AthenaParameterType.Boolean:
                    if (value.GetType().ToString() != typeof(Boolean).ToString())
                    {
                        throw new ArgumentException($"Property: {name} is not Boolean type");
                    }
                    _parameters.Add($"@{name}", value.ToString());
                    break;

                case AthenaParameterType.String:
                    if (value.GetType().ToString() != typeof(String).ToString())
                    {
                        throw new ArgumentException($"Property: {name} is not String type");
                    }
                    _parameters.Add($"@{name}", $"'{value}'");
                    break;

                case AthenaParameterType.DateTime:
                    if (value.GetType().ToString() != typeof(DateTime).ToString())
                    {
                        throw new ArgumentException($"Property: {name} is not DateTime type");
                    }

                    _parameters.Add($"@{name}", $"TIMESTAMP '{value}'");
                    break;

                #region array_type_transfer
                case AthenaParameterType.Int16 | AthenaParameterType.Array:
                    if (value is IEnumerable<Int16> shortArray)
                    {
                        _parameters.Add($"@{name}", $"({string.Join(",", shortArray)})");
                        break;
                    }
                    else
                    {
                        throw new ArgumentException($"Property: {name} type error");
                    }
                case AthenaParameterType.Int32 | AthenaParameterType.Array:
                    if (value is IEnumerable<int> intArray)
                    {
                        _parameters.Add($"@{name}", $"({string.Join(",", intArray)})");
                        break;
                    }
                    else
                    {
                        throw new ArgumentException($"Property: {name} type error");
                    }
                case AthenaParameterType.Int64 | AthenaParameterType.Array:
                    if (value is IEnumerable<Int64> longArray)
                    {
                        _parameters.Add($"@{name}", $"({string.Join(",", longArray)}");
                        break;
                    }
                    else
                    {
                        throw new ArgumentException($"Property: {name} type error");
                    }
                case AthenaParameterType.Double | AthenaParameterType.Array:
                    if (value is IEnumerable<Double> doubleArray)
                    {
                        _parameters.Add($"@{name}", $"({string.Join(",", doubleArray)})");
                        break;
                    }
                    else
                    {
                        throw new ArgumentException($"Property: {name} type error");
                    }
                case AthenaParameterType.Decimal | AthenaParameterType.Array:
                    if (value is IEnumerable<Decimal> decimalArray)
                    {
                        _parameters.Add($"@{name}", $"({string.Join(",", decimalArray)})");
                        break;
                    }
                    else
                    {
                        throw new ArgumentException($"Property: {name} type error");
                    }
                case AthenaParameterType.Float | AthenaParameterType.Array:
                    if (value is IEnumerable<float> floatArray)
                    {
                        _parameters.Add($"@{name}", $"({string.Join(",", floatArray)})");
                        break;
                    }
                    else
                    {
                        throw new ArgumentException($"Property: {name} type error");
                    }
                case AthenaParameterType.Boolean | AthenaParameterType.Array:

                    if (value is IEnumerable<Boolean> boolArray)
                    {
                        _parameters.Add($"@{name}", $"({string.Join(",", boolArray)})");
                        break;
                    }
                    else
                    {
                        throw new ArgumentException($"Property: {name} type error");
                    }

                case AthenaParameterType.String | AthenaParameterType.Array:
                    if (value is IEnumerable<string> stringArray)
                    {
                        _parameters.Add($"@{name}", $"({string.Join(",", stringArray.Select(x => $"'{x}'"))})");
                        break;
                    }
                    else
                    {
                        throw new ArgumentException($"Property: {name} type error");
                    }
                case AthenaParameterType.DateTime | AthenaParameterType.Array:
                    if (value is IEnumerable<DateTime> dateArray)
                    {
                        _parameters.Add($"@{name}", $"({string.Join(",", dateArray.Select(x => $"TIMESTAMP '{x.ToString("yyyy-MM-dd HH:mm:ss.fff")}'"))})");
                        break;
                    }
                    else
                    {
                        throw new ArgumentException($"Property: {name} type error");
                    }
                #endregion

                default:
                    throw new NotSupportedException($"Type: {type} Not Support Tpye");
            }
        }
    }
}
