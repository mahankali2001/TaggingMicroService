using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Service.Contracts.Context;
using System.Data.Common;
using System.Data.SqlClient;
using Core.Logger;
using Business.Interface;
using Persistence.Interface;

namespace Services.Implementation.Core
{
    public class GenericServiceApplicationContext : IApplicationContext
    {
        private IBusinessManager businessManager;
        private IDataManager dataManager;
        private DbConnection dbConnection;
        protected static readonly ILogger Logger = LogManager.GetLogger(typeof(GenericServiceApplicationContext));

        public GenericServiceApplicationContext()
        {
            this.dbConnection = new SqlConnection(CustomRequestContext.ConnectionString);
            this.businessManager = Unity.Container.Resolve<IBusinessManager>(new ParameterOverride("context", this));
            this.dataManager = Unity.Container.Resolve<IDataManager>(new ParameterOverride("connection", this.dbConnection));
        }

        public DbConnection GetDbConnection()
        {
            return this.dbConnection;
        }

        public IBusinessManager GetBusinessManager()
        {
            return this.businessManager;
        }

        public IDataManager GetDataManager()
        {
            return this.dataManager;
        }

        public void Dispose()
        {
            List<string> errors = new List<string>();
            Action<IDisposable> cleanUp = disposable =>
                {
                    try 
	                {	        
	                    disposable.Dispose();	
	                }
	                catch (Exception ex)
	                {
                        string error = string.Format("Error Disposing Application Context - Object {0}, Exception Message: {1}, Exception Stack : {2}, Inner Exception: {3}", disposable.ToString(), ex.Message, ex.StackTrace, ex.InnerException == null ? "No Inner Exception" : string.Format("Inner Message : {0}, Inner StackTrace : {1}", ex.InnerException.Message, ex.InnerException.StackTrace));
                        errors.Add(error);
	                }
                };

            cleanUp(this.businessManager);
            cleanUp(this.dataManager);
            cleanUp(this.dbConnection);

            if (errors.Count > 0)
            {
                throw new Exception(string.Join(Environment.NewLine, errors));
            }
        }
    }
}
