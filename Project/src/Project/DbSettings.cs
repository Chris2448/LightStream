using System.Collections.Generic;

namespace Project
{
    public class collections
    {
        public string Programs { get; set; }

        public string People { get; set; }

        public string Suggestions { get; set; }

        public string Configurations { get; set; }

        public string Counters { get; set; }
    }

    public class DbSettings
    {
        public string connectionstring { get; set; }

        public string database { get; set; }

        public collections collections { get; set; }
    }
}
