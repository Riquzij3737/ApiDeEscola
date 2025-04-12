using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;

namespace ApiDeEscola.Models.KeyAnomymus
{
    public class SecurityKeys
    {
        public byte[] key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("CRIPKEY"));
        public byte[] Iv = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("CRIPIV"));
        public PaddingMode padding = PaddingMode.PKCS7;
        public CipherMode mode = CipherMode.CBC;        
    }
}
