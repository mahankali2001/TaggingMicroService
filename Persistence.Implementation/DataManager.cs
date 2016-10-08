using System;
using System.Data.Entity;
//using System.Data.Objects;
using System.Data.Entity.Infrastructure;
using System.Data;
using System.Collections.Generic;
using System.Reflection;
using Persistence.Implementation.Repository;
using Persistence.Implementation.Context;
using System.Data.Common;
using Persistence.Interface.Repository;
using Persistence.Interface;

namespace Persistence.Implementation
{
    public class DataManager : IDataManager
    {
        private RepositoryContext _context;
        public DataManager(DbConnection connection)
        {
            this._context =  new RepositoryContext(connection);
        }

        public ITaggingRepository GetTEMPLATERepository()
        {
            return new TaggingRepository(this._context);
        }


        public void Dispose()
        {
            this._context.Dispose();
        }
    }
}
