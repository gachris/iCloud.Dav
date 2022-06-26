using System.Collections.Generic;

namespace iCloud.Dav.People.Types
{
    internal sealed class Filters
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public IList<TextMatch> TextMatches { get; }

        public Filters()
        {
            TextMatches = new List<TextMatch>();
        }
    }
}
