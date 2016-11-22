using System;
using System.Collections.Generic;
using System.Text;
using Markdown.MarkdownEnumerable;
using Markdown.MarkdownEnumerable.Tags;

namespace Markdown.TagsRepresentation
{
    internal class ParsedTag
    {
        readonly Dictionary<Tag, string> htmlNames = new Dictionary<Tag, string>
        {
            { Tag.Italic, "em"},
            { Tag.Strong, "strong"},
            { Tag.Hyperlink, "a"}
        };

        private readonly Dictionary<string, string> properties = new Dictionary<string, string>();

        public readonly Tag Tag;
        public string Value;

        public ParsedTag(Tag tag)
        {
            Tag = tag;
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
                if (tagType.TagPart == HyperlinkTagInfo.VALUE_PART)
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
            builder.AppendFormat("<{0}", htmlNames[Tag]);
            foreach (var property in properties)
                builder.AppendFormat(" {0}=\"{1}\"", property.Key, property.Value);
            builder.AppendFormat(">{0}</{1}>", Value, htmlNames[Tag]);
            return builder.ToString();
        }
    }
}
