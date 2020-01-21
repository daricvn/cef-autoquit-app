using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Autoquit2.Services {
    internal class Compressor {
        private const string _desKey = "_c=v3;MYN*4BE+eiM,fFDcl>6kNgvpz5L@S](vhRd[;2SMPrz:jz^f-Nav(";
        private const string _appendCode = "#sil:";
        private static byte[] _keyHash = null;
        private static byte[] KeyHash {
            get {
                if (_keyHash == null ) {
                    var hash = new HMACSHA256();
                    _keyHash = hash.ComputeHash(UTF8Encoding.UTF8.GetBytes(_desKey));
                    //Always release the resources and flush data
                    // of the Cryptographic service provide. Best Practice

                    hash.Clear();
                }
                return _keyHash;
            }
        }
        public static bool WriteObject( string path, object content , bool noOpt = false) {
            try {
                using ( var fs = new FileStream(path, FileMode.Create, FileAccess.Write) )
                using ( var df = new DeflateStream(fs, CompressionLevel.Fastest) ) {
                    var obj = JsonConvert.SerializeObject(content);
                    var encContent = obj;
                    if (!noOpt)
                        encContent =_appendCode + Encrypt(obj);
                    byte[] bytes = Encoding.UTF8.GetBytes(encContent);
                    df.Write(bytes, 0, bytes.Length);
                    df.FlushAsync().Wait();
                }
            }
            catch ( Exception ) {
                return false;
            }
            return true;
        }
        public static T ReadObject<T>( string path , bool noOpt = false) {
            try {
                int count = 0;
                byte[] bytes;
                using ( var fs = new FileStream(path, FileMode.Open, FileAccess.Read) )
                using ( var df = new DeflateStream(fs, CompressionMode.Decompress) )
                using ( var ms = new MemoryStream() ) {
                    df.CopyTo(ms);
                    bytes = new byte[ms.Length];
                    ms.Seek(0, SeekOrigin.Begin);
                    count = ms.Read(bytes, 0, (int)ms.Length);
                }
                if ( count > 0 ) {
                    var str = Encoding.UTF8.GetString(bytes);
                    if ( str != null ) {
                        // Compatibility with old one
                        if ( !str.StartsWith(_appendCode) || noOpt)
                            return JsonConvert.DeserializeObject<T>(str);
                        else {
                            var dec = Decrypt(str.Substring(_appendCode.Length));
                            return JsonConvert.DeserializeObject<T>(dec);
                        }
                    }
                }
            }
            catch ( Exception e ) {
            }
            return default(T);
        }

        public static string Encrypt( string toEncrypt ) {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
            // Get the key from config file

            //System.Windows.Forms.MessageBox.Show(key);
            //If hashing use get hashcode regards to your key

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = KeyHash;
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)

            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            byte[] resultArray =
              cTransform.TransformFinalBlock(toEncryptArray, 0,
              toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        public static string Decrypt( string cipherString ) {
            byte[] keyArray;
            //get the byte code of the string

            byte[] toEncryptArray = Convert.FromBase64String(cipherString);


            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = KeyHash;
            //mode of operation. there are other 4 modes. 
            //We choose ECB(Electronic code Book)

            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(
                                 toEncryptArray, 0, toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor                
            tdes.Clear();
            //return the Clear decrypted TEXT
            return UTF8Encoding.UTF8.GetString(resultArray);
        }

    }
}
