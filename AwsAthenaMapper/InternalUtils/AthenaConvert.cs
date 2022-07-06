using Amazon.Athena.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AwsAthenaMapper.InternalUtils
{
    internal static class AthenaConvert
    {
        internal static IEnumerable<T> MappingToList<T>(List<Row> rows, Dictionary<string, ColumnIndexInfo> colInfoMap) where T : class, new()
        {
            var result = new List<T>();
            var cacheMap = typeof(T).GetProperties().ToDictionary(x => x.Name.ToLower(), x => x);

            for (int i = 0; i < rows.Count; i++)
            {
                var obj = new T();
                foreach (var cache in cacheMap)
                {
                    if (colInfoMap.ContainsKey(cache.Key))
                    {
                        var colInfo = colInfoMap[cache.Key];
                        var idx = colInfo.Index;

                        if (IsCollectionType(cache.Value))
                        {
                            var val = ConvertToArrayType(rows[i].Data[idx].VarCharValue, cache.Value.PropertyType);
                            cache.Value.SetValue(obj, val);
                        }
                        else
                        {
                            var val = Convert.ChangeType(rows[i].Data[idx].VarCharValue, cache.Value.PropertyType);
                            cache.Value.SetValue(obj, val, null);
                        }
                    }
                }

                result.Add(obj);
            }

            return result;
        }

        internal static T MappingToObject<T>(List<Datum> data, Dictionary<string, ColumnIndexInfo> colInfoMap) where T : class, new()
        {
            var obj = new T();
            var cacheMap = typeof(T).GetProperties().ToDictionary(x => x.Name.ToLower(), x => x);
            bool hasValue = false;
            foreach (var cache in cacheMap)
            {
                if (colInfoMap.ContainsKey(cache.Key))
                {
                    hasValue = true;
                    var colInfo = colInfoMap[cache.Key];
                    var idx = colInfo.Index;


                    if (IsCollectionType(cache.Value))
                    {
                        var val = ConvertToArrayType(data[idx].VarCharValue, cache.Value.PropertyType);
                        cache.Value.SetValue(obj, val, null);
                    }
                    else
                    {
                        var val = Convert.ChangeType(data[idx].VarCharValue, cache.Value.PropertyType);
                        cache.Value.SetValue(obj, val, null);
                    }

                }
            }

            return hasValue ? obj : null;
        }

        internal static Dictionary<string, ColumnIndexInfo> MapColumnsIndexInfo(List<Datum> columnsIndex, List<ColumnInfo> columnInfoList)
        {
            var result = new Dictionary<string, ColumnIndexInfo>();

            if (columnsIndex == null || columnInfoList == null)
            {
                return result;
            }

            for (int i = 0; i < columnsIndex.Count; i++)
            {
                var column = columnsIndex[i].VarCharValue.ToLower();

                result.Add(column, new ColumnIndexInfo
                {
                    Index = i,
                    ColumnInfo = columnInfoList.FirstOrDefault(f => f.Name.ToLower() == column)
                });
            }

            return result;
        }


        private static IEnumerable ConvertToArrayType(string varcharValue, Type destType)
        {
            var arr = varcharValue.Replace("[", "").Replace("]", "").Split(',').Select(x => x).ToArray();

            if (destType.IsArray)
            {
                var t = destType.GetElementType();
                var instance = Array.CreateInstance(t, arr.Length);

                for (int i = 0; i < arr.Length; i++)
                {
                    instance.SetValue(Convert.ChangeType(arr[i].Trim(), t), i);
                }

                return instance;
            }

            if (destType.IsGenericType)
            {
                var t = destType.GetGenericArguments()[0];
                IList instance = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(t));

                for (int i = 0; i < arr.Length; i++)
                {
                    instance.Add(Convert.ChangeType(arr[i].Trim(), t));
                }

                return instance;
            }

            return null;
        }

        private static bool IsCollectionType(PropertyInfo prop)
        {
            return (!typeof(string).IsAssignableFrom(prop.PropertyType) && typeof(IEnumerable).IsAssignableFrom(prop.PropertyType));
        }
    }
}
