using System;
using System.IdentityModel.Claims;
using System.ServiceModel;

namespace Services.Implementation.Core
{


    /* Authorization manager currently not used but could be enabled by invoking it from the servicehost file by adding
     * Current this is not needed.
            host.Authorization.ServiceAuthorizationManager = new MyServiceAuthorizationManager();
            host.Open();
    */
    public class MyServiceAuthorizationManager : ServiceAuthorizationManager
    {
        public override bool CheckAccess(OperationContext operationContext)
        {
            Console.WriteLine("MyServiceAuthorizationManager CheckAccess. called.");
            string token = operationContext.RequestContext.RequestMessage.Headers.Action;
            return true;
        }

        protected override bool CheckAccessCore(OperationContext operationContext)
        {
            string action = operationContext.RequestContext.RequestMessage.Headers.Action;
            // Iterate through the various claim sets in the AuthorizationContext.
            foreach (ClaimSet cs in operationContext.ServiceSecurityContext.AuthorizationContext.ClaimSets)
            {
                // Examine only those claim sets issued by System.
                if (cs.Issuer == ClaimSet.System)
                {
                    foreach (
                        Claim c in
                            cs.FindClaims("http://xyz/allowedoperation", Rights.PossessProperty))
                    {
                        if (action == c.Resource.ToString())
                            return true;
                    }
                }
            }
            // If this point is reached, return false to deny access.
            return false;
        }
    }
}