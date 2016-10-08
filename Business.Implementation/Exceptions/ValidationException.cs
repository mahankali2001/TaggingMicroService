using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Implementation.Exceptions
{
    public class ValidationException : ApplicationException
    {
        private List<string> _message = new List<string>();
        private string outputMessage = null;

        public ValidationException(string message)
        {
            _message.Add(message);
        }

        public ValidationException(List<string> message)
        {
            _message = message;
        }


        private IEnumerable<string> GetFormattedMessages()
        {
            foreach (var msg in _message)
            {
                yield return string.Format("[{0}]", msg);
            }
        }

        public override string Message
        {
            get 
            {
                if (outputMessage == null)
                {
                    if (_message.Count == 0)
                    {
                        outputMessage = string.Empty;
                    }
                    else
                    {

                        outputMessage = string.Join(",", GetFormattedMessages());
                    }
                }

                return outputMessage;
            }
        }
    }
}
