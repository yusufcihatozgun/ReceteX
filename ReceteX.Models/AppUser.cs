using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceteX.Models
{
    public class AppUser : BaseModel
    {
        public Guid CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Gsm { get; set; }
        public string? TCKN { get; set; }
        public string? PinCode { get; set;}
        public int? CityCode { get; set;}
        public int? MedulaPassword{ get; set;}
        public bool isAdmin { get; set; } = false;
        public bool isActive { get; set; }
        public bool isRememberMe { get; set; } = false;
        
    }
}
