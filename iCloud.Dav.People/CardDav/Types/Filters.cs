using System.Collections.Generic;

namespace iCloud.Dav.People.CardDav.Types
{
    internal sealed class Filters
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public bool IsNotDefined { get; set; }

        public IList<TextMatch> TextMatches { get; }

        public Filters() => TextMatches = new List<TextMatch>();
    }
}