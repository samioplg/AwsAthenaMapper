using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AwsAthenaMapper
{
    public interface IAthenaClient : IDisposable
    {
        Task<T> QueryFirstOrDefaultAsync<T>(string sql, AthenaParameter parameter = null) where T : class, new();

        Task<IEnumerable<T>> QueryAsync<T>(string sql, AthenaParameter parameter = null) where T : class, new();
    }
}
