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
        public string Val { get; set; } // member names cannot be the same as their enclosing type
        public int SetByUserID { get; set; }
        public DateTime SetAtTime { get; set; } //Timestamp
        public KeyValueScope Scope { get; set; }

        public int? Key1ID { get; set; }
        public int? Key2ID { get; set; }
        public int? Key3ID { get; set; }
        public int? Key4ID { get; set; }
        public int? Key5ID { get; set; }
        public int? Key6ID { get; set; }
        public int? Key7ID { get; set; }
        public int? Key8ID { get; set; }
        public int? Key9ID { get; set; }
        public int? Key10ID { get; set; }
        public int? Key11ID { get; set; }
        public int? Key12ID { get; set; }
        public int? Key13ID { get; set; }
        public int? Key14ID { get; set; }
        public int? Key15ID { get; set; }
        public int? Key16ID { get; set; }
        public int? Key17ID { get; set; }
        public int? Key18ID { get; set; }
        public int? Key19ID { get; set; }
        public int? Key20ID { get; set; }
        public int? Key21ID { get; set; }
        public int? Key22ID { get; set; }
        public int? Key23ID { get; set; }
        public int? Key24ID { get; set; }
        public int? Key25ID { get; set; }
        public int? Key26ID { get; set; } 
        public int? Key27ID { get; set; }
        public int? Key28ID { get; set; }
        public int? Key29ID { get; set; }
        public int? Key30ID { get; set; }

        // Navigation property 
        //public virtual Key Key { get; set; }
 
        int[] KeyIDs
        {
		    get
            {
			    var ids = new List<int>();
			
			    if( Key1ID.HasValue) { ids.Add(Key1ID.Value); } else { return ids.ToArray(); }
                if (Key2ID.HasValue) { ids.Add(Key2ID.Value); } else { return ids.ToArray(); }
                if (Key3ID.HasValue) { ids.Add(Key3ID.Value); } else { return ids.ToArray(); }
                if (Key4ID.HasValue) { ids.Add(Key4ID.Value); } else { return ids.ToArray(); }
                if (Key5ID.HasValue) { ids.Add(Key5ID.Value); } else { return ids.ToArray(); }
                if (Key6ID.HasValue) { ids.Add(Key6ID.Value); } else { return ids.ToArray(); }
                if (Key7ID.HasValue) { ids.Add(Key7ID.Value); } else { return ids.ToArray(); }
                if (Key8ID.HasValue) { ids.Add(Key8ID.Value); } else { return ids.ToArray(); }
                if (Key9ID.HasValue) { ids.Add(Key9ID.Value); } else { return ids.ToArray(); }
                if (Key10ID.HasValue) { ids.Add(Key10ID.Value); } else { return ids.ToArray(); }

                if (Key11ID.HasValue) { ids.Add(Key11ID.Value); } else { return ids.ToArray(); }
                if (Key12ID.HasValue) { ids.Add(Key12ID.Value); } else { return ids.ToArray(); }
                if (Key13ID.HasValue) { ids.Add(Key13ID.Value); } else { return ids.ToArray(); }
                if (Key14ID.HasValue) { ids.Add(Key14ID.Value); } else { return ids.ToArray(); }
                if (Key15ID.HasValue) { ids.Add(Key15ID.Value); } else { return ids.ToArray(); }
                if (Key16ID.HasValue) { ids.Add(Key16ID.Value); } else { return ids.ToArray(); }
                if (Key17ID.HasValue) { ids.Add(Key17ID.Value); } else { return ids.ToArray(); }
                if (Key18ID.HasValue) { ids.Add(Key18ID.Value); } else { return ids.ToArray(); }
                if (Key19ID.HasValue) { ids.Add(Key19ID.Value); } else { return ids.ToArray(); }
                if (Key20ID.HasValue) { ids.Add(Key20ID.Value); } else { return ids.ToArray(); }

                if (Key21ID.HasValue) { ids.Add(Key21ID.Value); } else { return ids.ToArray(); }
                if (Key22ID.HasValue) { ids.Add(Key22ID.Value); } else { return ids.ToArray(); }
                if (Key23ID.HasValue) { ids.Add(Key23ID.Value); } else { return ids.ToArray(); }
                if (Key24ID.HasValue) { ids.Add(Key24ID.Value); } else { return ids.ToArray(); }
                if (Key25ID.HasValue) { ids.Add(Key25ID.Value); } else { return ids.ToArray(); }
                if (Key26ID.HasValue) { ids.Add(Key26ID.Value); } else { return ids.ToArray(); }
                if (Key27ID.HasValue) { ids.Add(Key27ID.Value); } else { return ids.ToArray(); }
                if (Key28ID.HasValue) { ids.Add(Key28ID.Value); } else { return ids.ToArray(); }
                if (Key29ID.HasValue) { ids.Add(Key29ID.Value); } else { return ids.ToArray(); }
                if (Key30ID.HasValue) { ids.Add(Key30ID.Value); } else { return ids.ToArray(); }
			
			    return ids.ToArray();
		    }
	    }

        string KeyString
        {
            get
            {
                StringBuilder text = new StringBuilder();

                foreach (var id in KeyIDs)
                {
                    //text.Append(LookupKeyID(id) + ".");
                }

                return text.ToString().TrimEnd('.');
            }
        }
    }
}
