using System;
using System.Collections.Generic;

namespace WorkerRole.Models
{
    public class Configurations : ICloneable
    {
        public int _id { get; set; }

        public Dictionary<string,bool> NeedUpdate { get; set; }

        public object Clone()
        {
            return (Configurations) MemberwiseClone();
        }
    }
}