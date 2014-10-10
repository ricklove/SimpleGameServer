﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GameServerDAL.Entities
{
    class Key
    {
        [Key()] // Primary key 
        public int KeyID { get; set; }
        public int ParentID { get; set; } // //Null or KeyID must exist
        public int Depth { get; set; }
        public string Name { get; set; }

        // Navigation property 
        //.
        //.
        //.
    }
}