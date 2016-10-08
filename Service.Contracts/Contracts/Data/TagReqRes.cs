using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Service.Contracts;

namespace Service.Contracts.Data
{
    /// <summary>
    /// TagRequest
    /// </summary>
    [DataContract(Namespace = ServiceConstants.DataContractNameSpace)]
    public class TagRequest
    {
        [DataMember]
        public List<Tag> Tags { get; set; }
        
        [DataMember]
        public List<TaggedObject> TaggedObjects { get; set; }

        [DataMember]
        public bool GetTaggedObjectsInResponse { get; set; }
    }

    /// <summary>
    /// TagRequest
    /// </summary>
    [DataContract(Namespace = ServiceConstants.DataContractNameSpace)]
    public class TagResponse
    {
        [DataMember]
        public List<Tag> Tags { get; set; }

        [DataMember]
        public List<TaggedObject> TaggedObjects { get; set; }

        [DataMember]
        public string ErrorCode { get; set; }
    }

    /// <summary>
    /// Tag
    /// </summary>
    [DataContract(Namespace = ServiceConstants.DataContractNameSpace)]
    public class Tag
    {
        [DataMember]
        public int? TagId { get; set; }

        [DataMember]
        public string TagName { get; set; }

        [DataMember]
        public string App { get; set; }

        [DataMember]
        public bool? IsActive { get; set; }

        [DataMember]
        public string ErrorCode { get; set; }
    }

    /// <summary>
    /// TaggedObject
    /// </summary>
    [DataContract(Namespace = ServiceConstants.DataContractNameSpace)]
    public class TaggedObject
    {
        [DataMember]
        public int TaggedObjectId { get; set; }

        [DataMember]
        public int TagId { get; set; }

        [DataMember]
        public string ObjectId { get; set; }

        [DataMember]
        public bool? IsActive { get; set; }

        [DataMember]
        public string App { get; set; }

        [DataMember]
        public string ErrorCode { get; set; }
    }

}
