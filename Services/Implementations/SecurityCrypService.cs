using ApiDeEscola.Models.KeyAnomymus;
using System.Security.Cryptography;
using System.Text;

namespace ApiDeEscola.Services.Implementations
{
    public class SecurityCrypService : ISecurityCrypService
    {
        public SecurityKeys Keys { get; set; }

        public SecurityCrypService()
        {
            Keys = new SecurityKeys();
        }

        public string EncryptData(string text)
        {
            byte[] textoembytes = Encoding.ASCII.GetBytes(text);

            using (var aes = Aes.Create())
            {
                aes.IV = this.Keys.Iv;
                aes.Key = this.Keys.key;
                aes.Mode = this.Keys.mode;
                aes.Padding = this.Keys.padding;

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms,aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(textoembytes);

                        cs.FlushFinalBlock();
                    }

                    return Encoding.ASCII.GetString(ms.ToArray());
                }
            }
        }

        public string DecryptData(string text)
        {
            byte[] textoembytes = Encoding.ASCII.GetBytes(text);

            using (var aes = Aes.Create())
            {
                aes.IV = this.Keys.Iv;
                aes.Key = this.Keys.key;
                aes.Mode = this.Keys.mode;
                aes.Padding = this.Keys.padding;

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(ms))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                    
                }
            }
        }
    }
}
