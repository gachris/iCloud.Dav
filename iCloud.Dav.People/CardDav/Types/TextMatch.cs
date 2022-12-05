namespace iCloud.Dav.People.CardDav.Types
{
    internal sealed class TextMatch
    {
        public string Collation { get; set; }

        public string MatchType { get; set; }

        public string SearchText { get; set; }

        public string NegateCondition { get; set; }
    }
}