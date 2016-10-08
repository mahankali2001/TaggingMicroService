using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Core.Logger;

namespace Core.Security
{
    public class UrlEncryptor :IDisposable
    {
        private const String DigitalEnvelopeDelimiter = ",";
        //private const String DigitalEnvelopeDelimiter = "@DED@";
        private readonly ILogger _log = LogManager.GetLogger("Core.Security.UrlEncryptor");
        private readonly string _privateCertificate = string.Empty;
        private readonly string _privateCertificatepassword = string.Empty;
        private readonly string _publicCertificate = string.Empty;
        private string _toBeEncrypted = string.Empty;
        private RSACryptoServiceProvider _privateKey;
        private PublicKey _publicKey;
        private bool _disposed = false;

        public UrlEncryptor(string appname, bool oldCode)
        {
            if (!String.IsNullOrEmpty(appname))
            {
                string publickey = ConfigurationManager.AppSettings.Get(appname + "PUBLICKEY");
                if (publickey == null)
                    throw new ArgumentException(string.Format("Application {0} Publickey not found", appname));
                else
                    _publicCertificate = publickey;

                string privatekey = ConfigurationManager.AppSettings.Get(appname + "PRIVATEKEY");
                if (privatekey == null)
                    throw new ArgumentException(string.Format("Application {0} PRIVATE not found", appname));
                else
                    _privateCertificate = privatekey;

                string privateCertificatepassword = ConfigurationManager.AppSettings.Get(appname + "PRIVATEKEYPASSWORD");
                if (privateCertificatepassword == null)
                    throw new ArgumentException(string.Format("Application {0} PRIVATE KEY PASSWORD not found", appname));
                else
                    _privateCertificatepassword = privateCertificatepassword;
            }
            
            InitalizeKeys();
        }

        public UrlEncryptor(string appname)
        {
            try
            {
                var collection = new X509Certificate2Collection();
                string certificateCollection = ConfigurationManager.AppSettings.Get(appname + "CERTKEYCOLLECTION");
                string certificateCollectionpassword = ConfigurationManager.AppSettings.Get(appname + "CERTKEYCOLLECTIONPASSWORD");

                collection.Import(certificateCollection, certificateCollectionpassword, X509KeyStorageFlags.DefaultKeySet);

                foreach (X509Certificate2 x509Certificate2 in collection)
                {
                    string cn = x509Certificate2.GetNameInfo(X509NameType.SimpleName, true);
                    Console.WriteLine(x509Certificate2.GetNameInfo(X509NameType.SimpleName, true));

                    if (cn != null && cn.ToUpper() == "DM")
                    {
                        var dmprivateKey = x509Certificate2;
                        _privateKey = (RSACryptoServiceProvider)dmprivateKey.PrivateKey;
                    }

                    if (cn != null && cn.ToUpper() == "LIFERAY")
                    {
                        var lrPublickey = x509Certificate2;
                        _publicKey = lrPublickey.PublicKey;
                    }
                }

                if (_privateKey == null) throw new Exception("Private key is empty");
                if (_publicKey == null) throw new Exception("Public key is empty");
            }
            catch (Exception ex)
            {
                _log.Error("Error while initializing Encryptor",ex);
                throw;
            }
        }


        public string Encrypt(string toBeEncrypted, string returnformat)
        {
            try
            {
                _toBeEncrypted = toBeEncrypted;
                _log.Info(string.Format("Message :{0}", _toBeEncrypted));
                _log.Info(string.Format("MessageSize:{0}", _toBeEncrypted.Length));

                byte[] key = GetKey();

                _log.Info(string.Format("Key Generated {0}", key));
                _log.Info(string.Format("Key Length {0}", key.Length));

                string token0 = Encrypt_Key_Using_PublicKey_RSA_ECB_PKCS1(_publicKey, key);
                

                _log.Info("token0:" + token0);
                _log.Info("token0 length:" + token0.Length);

                string signature = SignUserinformationUsingPrivatekey(toBeEncrypted);
                string envelope = signature + DigitalEnvelopeDelimiter + toBeEncrypted;

                _log.Info("Envelope:" + envelope);

                var byteConverter = new ASCIIEncoding();
                byte[] envelopebytes = byteConverter.GetBytes(envelope);
                string token1 = EncryptEnvelopeUsingAesSymkey(key, envelope);


                _log.Info("token1:" + token1);
                _log.Info("token1 length:" + token1.Length);
                _log.Info(string.Format("Total token length={0}", token0.Length + token1.Length));

                return (string.Format(returnformat, System.Web.HttpUtility.UrlEncode(token0), System.Web.HttpUtility.UrlEncode(token1)));
            }
            catch (Exception ex)
            {
                _log.Error(string.Format("Error while encrypting string <{0}> {1}", toBeEncrypted, ex));
                return null;
            }
        }

        private string Encode(string tobeEncoded)
        {
            if (!String.IsNullOrEmpty(tobeEncoded))
                return System.Web.HttpUtility.UrlEncode(tobeEncoded);
            return string.Empty;
        }

        private string EncryptEnvelopeUsingAesSymkey(byte[] symmetricKey, string envelope)
        {
            byte[] encrypted;
            using (var aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = symmetricKey;
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.IV = new[]
                                {
                                    (byte) 0xB2, (byte) 0x12, (byte) 0xD5, (byte) 0xB4,
                                    (byte) 0x44, (byte) 0x21, (byte) 0xC3, (byte) 0xC3,
                                    (byte) 0xB2, (byte) 0x12, (byte) 0xD5, (byte) 0xB4,
                                    (byte) 0x44, (byte) 0x21, (byte) 0xC3, (byte) 0xC3
                                };
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(envelope);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            _log.Info("encryptedbyte.length=" + encrypted.Length);
            string retunencrypted = Convert.ToBase64String(encrypted);
            _log.Info("retunencrypted.length=" + retunencrypted.Length);
            return retunencrypted;
        }

        public string SignUserinformationUsingPrivatekey(string u)
        {
            var byteConverter = new ASCIIEncoding();
            byte[] input = byteConverter.GetBytes(u);
            byte[] signature = _privateKey.SignData(input, new SHA1CryptoServiceProvider());
            Debug.WriteLine("signature.length=" + signature.Length);
            string returnsignedinformation = Convert.ToBase64String(signature);
            Debug.WriteLine("returnsignedinformation.length=" + returnsignedinformation.Length);
            return returnsignedinformation;
        }

        private string Encrypt_Key_Using_PublicKey_RSA_ECB_PKCS1(PublicKey lrPublickey, byte[] aesKey)
        {
            var RSA = (RSACryptoServiceProvider) lrPublickey.Key;
            byte[] bb = RSA.Encrypt(aesKey, false);
            return Convert.ToBase64String(bb);
        }

        private void InitalizeKeys()
        {
            var servicePrivateKey = new X509Certificate2(_privateCertificate, _privateCertificatepassword,X509KeyStorageFlags.DefaultKeySet);
            var lrPublickey = new X509Certificate2(_publicCertificate);

            _privateKey = (RSACryptoServiceProvider) servicePrivateKey.PrivateKey;
            _publicKey = lrPublickey.PublicKey;
        }

        public byte[] GetKey()
        {
            byte[] retkey;
            using (Aes aes = new AesManaged())
            {
                aes.KeySize = 128;
                aes.GenerateKey();
                retkey = aes.Key;
            }
            return retkey;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {}

            _disposed = true;
        }
    }
}