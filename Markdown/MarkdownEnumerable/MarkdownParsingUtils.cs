using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.MarkdownEnumerable.Tags;

namespace Markdown.MarkdownEnumerable
{
    internal static class MarkdownParsingUtils
    {
        public const string Digits = "0123456789";
        public const char Underscore = '_';
        public const string SpaceSymbols = " \n\r\u0009\u00a0\u2000\u2001";
        public const string NextLineSymbols = "\n";

        public static string ToCorrectLink(string link)
        {
            var skippedSpaces = link
                .SkipWhile(char.IsWhiteSpace)
                .TakeWhile(ch => !char.IsWhiteSpace(ch))
                .Aggregate(new StringBuilder(), (builder, ch) => builder.Append(ch))
                .ToString();
            if (skippedSpaces.Count(ch => !char.IsWhiteSpace(ch)) != link.Count(ch => !char.IsWhiteSpace(ch)))
                return null;
            return skippedSpaces;
        }

        public static bool IsCorrectLink(string link)
        {
            return ToCorrectLink(link) != null;
        }

        public static string CombineLinks(string baseLink, string relativePath)
        {
            var builder = new StringBuilder();
            builder.Append(baseLink.LastOrDefault() == '/' ? baseLink.Substring(0, baseLink.Length - 1) : baseLink);
            builder.Append('/');
            builder.Append(relativePath.FirstOrDefault() == '/' ? relativePath.Substring(1) : relativePath);
            return builder.ToString();
        }
    }
}
