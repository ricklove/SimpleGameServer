using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;



namespace GameServerDAL.Entities
{
    public class UserClient
    {
        [Key()] // Primary key 
        public Guid UserClientToken { get; set; }
        // Foreign key 
        public int UserID { get; set; }

        // Navigation property 
        public virtual User User { get; set; } 
    }
}
