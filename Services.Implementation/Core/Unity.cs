using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Configuration;

namespace Services.Implementation
{
    public static class Unity
    {
        static IUnityContainer container;
        
        static Unity()
        {
            try
            {
                container = new UnityContainer();
                UnityConfigurationSection section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
                section.Configure(container);

            }
            catch (Exception ex)
            {
                
                throw ex;
            }
           
        }

        public static IUnityContainer Container
        {
            get
            {
                return container;
            }
        }
    }
}
