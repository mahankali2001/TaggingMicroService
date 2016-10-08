using System;
using System.Threading;
using System.ComponentModel;
using System.IO.Ports;
using Persistence.Entities;

namespace Utility
{
    public class SMSCOMMS : IDisposable
    {
        private SerialPort SMSPort;
        private Thread SMSThread;
        private Thread ReadThread;
        public static bool _Continue = false;
        public static bool _ContSMS = false;
        private bool _Wait = false;
        public static bool _ReadPort = false;
        public string ListPhoneNumbers = "|@txt.att.net,number@tmomail.net,|number@vtext.com,|@messaging.sprintpcs.com,|@pm.sprint.com";

        public delegate void SendingEventHandler(bool done);

        public event SendingEventHandler Sending;

        public delegate void DataReceivedEventHandler(string message);

        public event DataReceivedEventHandler DataReceived;



        public SMSCOMMS(){}
        public SMSCOMMS(ref string commport)
        {
            SMSPort = new SerialPort
                          {
                              PortName = commport,
                              BaudRate = 9600,
                              Parity = Parity.None,
                              DataBits = 8,
                              StopBits = StopBits.One,
                              Handshake = Handshake.RequestToSend,
                              DtrEnable = true,
                              RtsEnable = true,
                              NewLine = System.Environment.NewLine
                          };
            ReadThread = new Thread(new System.Threading.ThreadStart(ReadPort));
        }

        public bool SendSMS(string cellNumber, string smsMessage)
        {
            string myMessage = null;
            //Check if Message Length <= 160
            if (smsMessage.Length <= 160)
                myMessage = smsMessage;
            else
                myMessage = smsMessage.Substring(0, 160);
            if (SMSPort.IsOpen == true)
            {
                SMSPort.WriteLine("AT+CMGS=" + cellNumber + "r");
                _ContSMS = false;
                SMSPort.WriteLine(
                    myMessage + System.Environment.NewLine + (char) (26));
                _Continue = false;
                if (Sending != null)
                    Sending(false);
            }
            return false;
        }

        private void ReadPort()
        {
            string SerialIn = null;
            byte[] RXBuffer = new byte[SMSPort.ReadBufferSize + 1];
            string SMSMessage = null;
            int Strpos = 0;
            string TmpStr = null;
            while (SMSPort.IsOpen == true)
            {
                if ((SMSPort.BytesToRead != 0) & (SMSPort.IsOpen == true))
                {
                    while (SMSPort.BytesToRead != 0)
                    {
                        SMSPort.Read(RXBuffer, 0, SMSPort.ReadBufferSize);
                        SerialIn =
                            SerialIn + System.Text.Encoding.ASCII.GetString(
                                RXBuffer);
                        if (SerialIn.Contains(">") == true)
                        {
                            _ContSMS = true;
                        }
                        if (SerialIn.Contains("+CMGS:") == true)
                        {
                            _Continue = true;
                            if (Sending != null)
                                Sending(true);
                            _Wait = false;
                            SerialIn = string.Empty;
                            RXBuffer = new byte[SMSPort.ReadBufferSize + 1];
                        }
                    }
                    if (DataReceived != null)
                        DataReceived(SerialIn);
                    SerialIn = string.Empty;
                    RXBuffer = new byte[SMSPort.ReadBufferSize + 1];
                }
            }
        }

        public bool SendSMSO(string CellNumber, string SMSMessage)
        {
            string MyMessage = null;
            if (SMSMessage.Length <= 160)
            {
                MyMessage = SMSMessage;
            }
            else
            {
                MyMessage = SMSMessage.Substring(0, 160);
            }
            if (SMSPort.IsOpen == true)
            {
                SMSPort.WriteLine("AT+CMGS=" + CellNumber + "r");
                _ContSMS = false;
                SMSPort.WriteLine(
                    MyMessage + System.Environment.NewLine + (char) (26));
                _Continue = false;
                if (Sending != null)
                    Sending(false);
            }
            return false;
        }

        public void Open()
        {
            if (SMSPort.IsOpen == false)
            {
                SMSPort.Open();
                ReadThread.Start();
            }
        }

        public void Close()
        {
            if (SMSPort.IsOpen == true)
            {
                SMSPort.Close();
            }
        }

        public void SendSMSBruteForce(string phoneNumber, IEmailAttributes emailatt)
        {
            string ph = ListPhoneNumbers.Replace("|", phoneNumber);
            using ( var e = new  Email(emailatt))
            {
                emailatt.ToAddress = ph;
                e.SendEmail();
            }
        }

        public void Dispose()
        {
            this.Close();
        }

 
    }
}