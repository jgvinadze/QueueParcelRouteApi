using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueParcelRouteApi.Application
{
    public interface IDapperDbConnectionFactory
    {
        IDbConnection CreateOracleConnection();
        IDbConnection CreateMariaDbConnection();
    }
}
