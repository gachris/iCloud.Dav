namespace iCloud.Dav.People
{
    /// <summary>A note or comment in a Person.</summary>
    public class Note
    {
        #region Properties

        /// <summary>The language of the note.</summary>
        public virtual string Language { get; set; }

        /// <summary>The text of the note.</summary>
        public virtual string Text { get; set; }

        #endregion
    }
}
