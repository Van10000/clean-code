using System;
using Markdown.MarkdownEnumerable;

namespace Markdown.TagsRepresentation
{
    internal class HtmlTagsRepresentation : ITagsRepresentation
    {
        public string GetRepresentation(TagInfo tagInfo)
        {
            if (tagInfo.Tag == Tag.Hyperlink)
                return GetHyperlinkRepresentation(tagInfo.TagType);
            switch (tagInfo.TagType)
            {
                case TagType.Opening:
                    return GetOpeningTag(tagInfo.Tag);
                case TagType.Closing:
                    return GetClosingTag(tagInfo.Tag);
                default:
                    throw new ArgumentException($"Unsupported tag type{tagInfo.TagType}");
            }
        }

        private string GetHyperlinkRepresentation(TagType tagType)
        {
            switch (tagType)
            {
                case TagType.Opening:
                    return "<a href=\"";
                case TagType.Middle:
                    return "\">";
                case TagType.Closing:
                    return "</a>";
                default:
                    throw new ArgumentException($"Unknown tag type:{tagType}");
            }
        }

        private string GetOpeningTag(Tag tag)
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

        private string GetClosingTag(Tag tag)
        {
            if (tag == Tag.None)
                return "";
            var openingTag = GetOpeningTag(tag);
            return openingTag.Insert(1, "/");
        }
    }
}
