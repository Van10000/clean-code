using System.Collections.Generic;
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

        public static ParsedTag Create(TagInfo tag)
        {
            switch (tag.Tag)
            {
                case Tag.NewLine:
                    return new ParsedNewLineTag();
                case Tag.Hyperlink:
                    return new HyperlinkParsedTag();
                case Tag.Header:
                    return new HeaderParsedTag(tag as HeaderTagInfo);
                default:
                    return new SimpleParsedTag(tag.Tag);
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
