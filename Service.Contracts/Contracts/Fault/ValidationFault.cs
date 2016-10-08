using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Service.Contracts.Fault
{
    [DataContract]
    public class ValidationFault
    {
        private List<string> _message = new List<string>();
        private const string StrDelimiter = "</br>";

        public ValidationFault(string message)
        {
            _message.Add(message);
        }

        public ValidationFault(List<string> message)
        {
            _message = message;
        }

        [DataMember]
        public string Message
        {
            get { return string.Join(StrDelimiter,_message.ToArray()) ; }
            set { _message.Add(value); }
        }

        [DataMember(Name = "MessageList")]
        public List<string> MessageList
        {
            get { return _message; }
            set { _message = value; }
        }
    }

}
