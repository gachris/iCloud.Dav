using System;
using System.Collections.Generic;
using vCard.Net;

namespace iCloud.Dav.People.Utils
{
    internal static class TextUtil
    {
        /// <summary>
        /// Folds lines at 75 characters, and prepends the next line with a space per RFC https://tools.ietf.org/html/rfc5545#section-3.1 
        /// </summary>
        public static string FoldLines(string incoming)
        {
            //The spec says nothing about trimming, but it seems reasonable...
            var trimmed = incoming.Trim();
            if (trimmed.Length <= 75)
            {
                return trimmed + SerializationConstants.LineBreak;
            }

            const int takeLimit = 74;

            var firstLine = trimmed.Substring(0, takeLimit);
            var remainder = trimmed.Substring(takeLimit, trimmed.Length - takeLimit);

            var chunkedRemainder = string.Join(SerializationConstants.LineBreak + " ", Chunk(remainder));
            return firstLine + SerializationConstants.LineBreak + " " + chunkedRemainder + SerializationConstants.LineBreak;
        }

        public static IEnumerable<string> Chunk(string str, int chunkSize = 73)
        {
            for (var index = 0; index < str.Length; index += chunkSize)
            {
                yield return str.Substring(index, Math.Min(chunkSize, str.Length - index));
            }
        }
    }
}