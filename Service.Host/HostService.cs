using System;
using Core.Logger;
using Services.Implementation;

namespace Service.Host
{
    public class HostService : IWinService
    {
        private static readonly ILogger Logger = LogManager.GetLogger("HostService");
        public static ServiceHost<TaggingServiceImplementation> TemplateService;

        #region IWinService Members

        public void Start()
        {
            try
            {
                if (TemplateService == null)
                {
                    TemplateService = new ServiceHost<TaggingServiceImplementation>();
                    TemplateService.EnableMetadataExchange(true);
                }
                else
                    TemplateService.Close();
                TemplateService.Open();
                Logger.Info("The service is ready.");
            }
            catch (Exception ex)
            {
                Logger.Error("Error while opening host:", ex);
            }
            
        }

        #endregion

        public void Stop()
        {
            if (TemplateService == null)
                return;
            TemplateService.Close();
        }
    }
}