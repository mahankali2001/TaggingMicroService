using System;
using System.Collections.Generic;
using System.Threading;
using Service.Contracts.Data;
using Service.Contracts.Data;

namespace Service.Contracts.Context
{
    public static class CustomRequestContext
    {
        [ThreadStatic]
        private static string _connectionString;
        [ThreadStatic]
        private static string _retailerCode;
        [ThreadStatic]
        private static string _liferayInstanceCode;
        [ThreadStatic]
        private static string _retailerEnvironmentKey;
        [ThreadStatic]
        private static string _logInUserName;
        [ThreadStatic]
        private static AppUser _appUser;
        [ThreadStatic]
        private static int  _appuserid;
        [ThreadStatic]
        private static IDictionary<string, object> _data;
        [ThreadStatic]
        private static string _cultureCode;

        public static int CurrentThreadId
        {
            get { return Thread.CurrentThread.ManagedThreadId; }
        }

        public static int AppUserId
        {
            get { return _appuserid; }
            set { _appuserid = value; }
        }

        

        public static string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }

        public static string RetailerCode
        {
            get { return _retailerCode; }
            set { _retailerCode = value; }
        }

        public static string LiferayInstanceCode
        {
            get { return _liferayInstanceCode; }
            set { _liferayInstanceCode = value; }
        }

        public static string RetailerEnvironmentKey
        {
            get { return _retailerEnvironmentKey; }
            set { _retailerEnvironmentKey = value; }
        }

        public static string LogInUserName
        {
            get { return _logInUserName; }
            set { _logInUserName = value; }
        }

        public static AppUser CurrentUser
        {
            get { return _appUser; }
            set { _appUser = value; }
        }

        public static object GetData(string keyName)
        {
            if (_data == null)
                return null;
            if (_data.ContainsKey(keyName))
                return _data[keyName];
            else return null;
        }
        public static void SetData(string key, object value)
        {
            if (_data == null)
                _data = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);
            if (_data.ContainsKey(key))
                _data[key] = value;
            else
                _data.Add(key, value);
        }

        public static string CultureCode
        {
            get { return _cultureCode; }
            set { _cultureCode = value; }
        }
    }
}