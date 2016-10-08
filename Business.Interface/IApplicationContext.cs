using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using Persistence.Interface;

namespace Business.Interface
{
    public interface IApplicationContext : IDisposable
    {
        DbConnection GetDbConnection();
        IBusinessManager GetBusinessManager();
        IDataManager GetDataManager();
    }
}
