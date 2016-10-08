using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using HybridWindowsService;


namespace Service.Host.WindowService
{
    static class Program
    {
        /////<summary>
        /////The main entry point for the application.
        /////</summary>
        //static void Main(string[] args)
        //{
        //    ServiceBase[] ServicesToRun;
        //    ServicesToRun = new ServiceBase[] 
        //    { 
        //        new Service1(args.Length==0?"GenericService": args[0]) 
        //    };
        //    ServiceBase.Run(ServicesToRun);
        //}

        /// <summary>
        /// Hybrid Service Base
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            HybridServiceBase[] ServicesToRun;
            ServicesToRun = new HybridServiceBase[] 
            { 
                new Service1(args.Length==0?"TaggingService": args[0]) 
            };
            HybridServiceBase.Run(ServicesToRun);
        }
    }
}
