using System;
using System.Collections.Generic;
using System.Text;
using Markdown.MarkdownEnumerable;

namespace Markdown.TagsRepresentation
{
    internal class ParsedTag
    {
        private readonly Dictionary<string, string> properties = new Dictionary<string, string>();
        private readonly ITagsRepresentation representation;

        public readonly Tag Tag;
        public string Value;

        public ParsedTag(Tag tag, ITagsRepresentation representation)
        {
            Tag = tag;
            this.representation = representation;
        }

        public void AddProperty(string propertyName, string value)
        {
            properties[propertyName] = value;
        }

        public void AddValueOrProperty(string valueOrProperty, TagType tagType, string baseUrl)
        {
            var collectedValue = valueOrProperty;
            if (Tag == Tag.Hyperlink)
            {
                if (tagType == TagType.Opening)
                    Value = collectedValue;
                else
                {
                    var correctLink = MarkdownParsingUtils.ToCorrectLink(collectedValue);
                    if (correctLink == null)
                        throw new InvalidOperationException("Link is incorrect"); // should not happen
                    var absoluteLink = IsRelativeLink(correctLink) ? GetAbsoluteLink(correctLink, baseUrl) : correctLink;
                    AddProperty("href", absoluteLink);
                }
            }
            else
            {
                Value = collectedValue;
            }
        }

        private string GetAbsoluteLink(string relativeLink, string baseUrl)
        {
            if (baseUrl == null)
                throw new InvalidOperationException("Base url was not specified.");
            return MarkdownParsingUtils.CombineLinks(baseUrl, relativeLink);
        }

        private bool IsRelativeLink(string correctLink)
        {
            return correctLink.Length == 0 || correctLink[0] == '/';
        }

        public string GetCurrentRepresentation()
        {
            var builder = new StringBuilder();
            builder.Append(representation.GetRepresentation(new TagInfo(Tag, TagType.Opening)));
            foreach (var property in properties)
            {
                builder.Append(' ');
                builder.Append(property.Key);
                builder.Append("=\"");
                builder.Append(property.Value);
                builder.Append("\"");
            }
            builder.Append(representation.GetRepresentation(new TagInfo(Tag, TagType.Middle)));
            builder.Append(Value);
            builder.Append(representation.GetRepresentation(new TagInfo(Tag, TagType.Closing)));
            return builder.ToString();
        }
    }
}
