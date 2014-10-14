using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameServerDAL.Entities
{

    public enum KeyValueScope
    {
        User,
        Shared
    }

    public class Value
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Primary key 
        public int ValueID { get; set; }
        public int KeyID { get; set; }
        public KeyValueScope Scope { get; set; }
        public string Val { get; set; }
        public int SetByUserID { get; set; }
        public DateTime SetAtTime { get; set; } // Timestamp

        // Navigation property 
        public virtual Key Key { get; set; } 
    }
}
