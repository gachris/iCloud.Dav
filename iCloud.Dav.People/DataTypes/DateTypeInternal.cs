﻿using System;

namespace iCloud.Dav.People.DataTypes
{
    /// <summary>Identifies the type of date in a vCard.</summary>
    /// <seealso cref="X_ABDate" />
    [Flags]
    internal enum DateTypeInternal
    {
        /// <summary>Indicates the other type.</summary>
        Other = 1,
        /// <summary>Indicates the pref type.</summary>
        Pref = 2
    }
}