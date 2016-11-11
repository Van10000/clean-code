﻿using System;
using Markdown.MarkdownEnumerable;

namespace Markdown.TagsRepresentation
{
    internal class HtmlTagsRepresentation : ITagsRepresentation
    {
        public string GetOpeningTag(Tag tag)
        {
            switch (tag)
            {
                case Tag.Italic:
                    return "<em>";
                case Tag.Strong:
                    return "<strong>";
                case Tag.None:
                    return "";
                default:
                    throw new ArgumentException($"Unknown tag:{tag}");
            }
        }

        public string GetClosingTag(Tag tag)
        {
            if (tag == Tag.None)
                return "";
            var openingTag = GetOpeningTag(tag);
            return openingTag.Insert(1, "/");
        }
    }
}
