using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameServerDAL.Entities
{
    public class Key
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Primary key 
        public int KeyID { get; set; }
        public string Name { get; set; }
    }
}
