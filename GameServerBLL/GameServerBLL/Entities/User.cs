using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameServerBLL.Entities
{
    public class User
    {
        [Key()] // Primary key 
        public int UserID { get; set; }

        [Index(IsUnique = true)] 
        public string Email { get; set; }
        public string EncodedPassword { get; set; }
        public bool IsVerified { get; set; }
    }
}
