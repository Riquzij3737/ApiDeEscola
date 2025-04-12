using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiDeEscola.Models
{
    public class UsersModel
    {
        public Guid Id { get; set; }                
        public string Name { get; set; }
        public string Emaíl { get; set; }
        public string password { get; set; }         

        public UsersModel()
        {
            this.Id = Guid.NewGuid();            
        }
    }
}
