using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GameServerDAL.Entities
{
    class Value
    {
        [Key()] // Primary key 
        public int ValueID { get; set; }
        public int KeyID { get; set; }
        public int Scope { get; set; }
        public string Val { get; set; }
        public int SetByUserID { get; set; }
        public DateTime SetAtTime { get; set; } // Timestamp
    }
}
