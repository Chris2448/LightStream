using System;
using System.Collections.Generic;

namespace WorkerRole.Models
{
    public class People : ICloneable
    {
        public int _id { get; set; }

        public string Name { get; set; }

        public IList<Enrollment> Enrollments { get; set; }

        public object Clone()
        {
            return (People) MemberwiseClone();
        }
    }

    public class Enrollment
    {
        public int ProgramId { get; set; }

        public string Role { get; set; }
    }
}
