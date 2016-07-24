using System;
using System.Collections.Generic;

namespace WorkerRole.Models
{
    public class Suggestions : ICloneable
    {
        public int _id { get; set; }

        public int ProgramId { get; set; }

        public IList<Programs> SuggestedPrograms { get; set; } 

        public object Clone()
        {
            return (Suggestions) MemberwiseClone();
        }
    }
}
