using System;
using System.Collections.Generic;

namespace WorkerRole.Models
{
    public class Programs : ICloneable
    {
        public int _id { get; set; }

        public string Title { get; set; }

        public string Release { get; set; }

        public int Runtime { get; set; }

        public IEnumerable<string> Genres { get; set; }

        public IEnumerable<string> Directors { get; set; }

        public IEnumerable<string> Writers { get; set; }

        public IEnumerable<string> Actors { get; set; }

        public string Plot { get; set; }

        public string Language { get; set; }

        public string Poster { get; set; }

        public string Rating { get; set; }

        public string Type { get; set; }

        public string Url { get; set; }

        public bool PeopleAdded { get; set; }

        public bool SuggestionsAdded { get; set; }

        public object Clone()
        {
            return (Programs) MemberwiseClone();
        }
    }
}
