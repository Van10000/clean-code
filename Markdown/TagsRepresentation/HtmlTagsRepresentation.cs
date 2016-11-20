using System;
using System.Collections.Generic;
using Markdown.MarkdownEnumerable;

namespace Markdown.TagsRepresentation
{
    public class HtmlTagsRepresentation : ITagsRepresentation
    {
        readonly Dictionary<Tag, string> htmlNames = new Dictionary<Tag, string>
        {
            { Tag.Italic, "em"},
            { Tag.Strong, "strong"},
            { Tag.Hyperlink, "a"}
        };

        public string GetRepresentation(TagInfo tagInfo)
        {
            if (tagInfo.Tag == Tag.None)
                return "";

            switch (tagInfo.TagType)
            {
                case TagType.Opening:
                    return "<" + htmlNames[tagInfo.Tag];
                case TagType.Middle:
                    return ">";
                case TagType.Closing:
                    return "</" + htmlNames[tagInfo.Tag] + ">";
                default:
                    throw new ArgumentException($"Unknown tag type:{tagInfo.TagType}");
            }
        }
    }
}
