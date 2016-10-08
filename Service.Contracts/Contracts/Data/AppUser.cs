using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Service.Contracts;

namespace Service.Contracts.Data
{
    /// <summary>
    /// App User contains complete information on Application User.
    /// </summary>
    [DataContract(Namespace = ServiceConstants.DataContractNameSpace)]
    public class AppUser
    {
        /*!< App User ID is the unique numeric ID assigned to user */
        [DataMember]
        public int AppUserId { get; set; }

        /*!< App User Login is the login/username of user used to login in application */
        [DataMember]
        public string AppUserLogin { get; set; }

        /*!< Indicates whether this User is Active or InActive */
        [DataMember]
        public bool IsActive { get; set; }

    }

}
