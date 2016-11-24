using System;
using System.Collections.Generic;
using System.Text;
using Markdown.MarkdownEnumerable;
using Markdown.MarkdownEnumerable.Tags;

namespace Markdown.TagsRepresentation
{
    internal abstract class ParsedTag
    {
        protected readonly Dictionary<string, string> Properties = new Dictionary<string, string>();

        public readonly Tag Tag;
        public string Value;

        protected ParsedTag(Tag tag)
        {
            Tag = tag;
        }

        public static ParsedTag Create(Tag tag)
        {
            switch (tag)
            {
                case Tag.NewLine:
                    return new ParsedNewLineTag();
                case Tag.Hyperlink:
                    return new HyperlinkParsedTag();
                default:
                    return new SimpleParsedTag(tag);
            }
        }

        public void AddProperty(string propertyName, string value)
        {
            Properties[propertyName] = value;
        }

        public virtual void AddValueOrProperty(string valueOrProperty, TagType tagType, string baseUrl)
        {
            Value = valueOrProperty;
        }
        
        public abstract string GetCurrentRepresentation();
    }
}
