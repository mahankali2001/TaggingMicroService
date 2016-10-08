﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;
using HybridWindowsService;

namespace Service.Host.WindowService
{
    public partial class Service1 : HybridServiceBase
    //public partial class Service1 : ServiceBase
    {
        public static IEnumerable<Type> WindowServices;

        public Service1(string serviceName)
        {
            InitializeComponent();
            Initialize();
            if (string.IsNullOrWhiteSpace(serviceName))
                serviceName = "TaggingService";

            ServiceName = serviceName;
            CanStop = true;
            CanPauseAndContinue = true;
            AutoLog = true;
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                EventLog.WriteEntry(string.Format("GenericService started at: {0}", DateTime.Now));
                try
                {
                    foreach (Type service in WindowServices)
                    {
                        object o = Activator.CreateInstance(service);
                        Type serviceType = service;
                        var thread = new Thread(() => serviceType.InvokeMember("Start", BindingFlags.Default | BindingFlags.InvokeMethod, null, o, null));
                        thread.Start();
                    }
                }
                catch (Exception ex)  // In case of exeption - Log and then throw exception
                {
                    EventLog.WriteEntry(ex.Message);
                    throw ex;
                }
            }
            catch (Exception ex)  // In case of exeption - Log and then throw exception
            {
                EventLog.WriteEntry(ex.Message);
                throw ex;
            }
        }

        protected override void OnStop()
        {
            EventLog.WriteEntry(string.Format("Tagging Service stopped at: {0}", DateTime.Now));
        }

        public void Initialize()
        {
            string currentAssemblyDirectoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string assemblyName = string.IsNullOrEmpty(ConfigurationManager.AppSettings["AssemblyName"])
                                      ? "Service.Host.dll"
                                      : ConfigurationManager.AppSettings["AssemblyName"];


            EventLog.WriteEntry("Loading Assembly:[" + currentAssemblyDirectoryName + "\\" + assemblyName + "]");
            Assembly serviceAssembly = Assembly.LoadFile(currentAssemblyDirectoryName + "\\" + assemblyName);

            WindowServices = from type in serviceAssembly.GetTypes()
                             where type.GetInterface("IWinService", true) != null
                             select type;
        }
    }
}