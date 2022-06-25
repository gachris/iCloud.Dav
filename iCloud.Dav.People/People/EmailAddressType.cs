﻿namespace iCloud.Dav.People
{
    /// <summary>Identifies the type of email address in a Person.</summary>
    /// <seealso cref="T:iCloud.Dav.People.People.EmailAddress" />
    public enum EmailAddressType
    {
        /// <summary>An Internet (SMTP) mail (default) address.</summary>
        Internet,
        /// <summary>An America On-Line email address.</summary>
        AOL,
        /// <summary>An AppleLink email address.</summary>
        AppleLink,
        /// <summary>An AT&amp;T Mail email address</summary>
        AttMail,
        /// <summary>
        ///     A CompuServe Information Service (CIS) email address.
        /// </summary>
        CompuServe,
        /// <summary>An eWorld email address.</summary>
        /// <remarks>
        ///     eWorld was an online service by Apple Computer in the mid 1990s.
        ///     It was officially shut down on March 31, 1996.
        /// </remarks>
        eWorld,
        /// <summary>An IBM Mail email address.</summary>
        IBMMail,
        /// <summary>An MCI Mail email address.</summary>
        MCIMail,
        /// <summary>A PowerShare email address.</summary>
        PowerShare,
        /// <summary>A Prodigy Information Service email address.</summary>
        Prodigy,
        /// <summary>A telex email address.</summary>
        Telex,
        /// <summary>An X.400 service email address.</summary>
        X400,
    }
}
