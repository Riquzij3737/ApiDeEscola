using ApiDeEscola.Models.KeyAnomymus;

namespace ApiDeEscola.Services
{
    public interface ISecurityCrypService
    {
        public SecurityKeys Keys { get; set; }
        public string EncryptData(string text);
        public string DecryptData(string text);
    }
}
