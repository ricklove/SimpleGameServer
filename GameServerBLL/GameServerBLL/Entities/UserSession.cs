using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GameServerBLL.Entities
{
    public class UserSession
    {
        [Key()] // Primary key 
        public Guid UserSessionToken { get; set; }
        // Foreign key 
        public Guid UserClientToken { get; set; }
        public int UserID { get; set; }

        // Navigation property 
        public virtual UserClient UserClient { get; set; }
    }
}
