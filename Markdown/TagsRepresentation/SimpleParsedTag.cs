using System.Collections.Generic;
using System.Text;
using Markdown.MarkdownEnumerable.Tags;

namespace Markdown.TagsRepresentation
{
    internal class SimpleParsedTag : ParsedTag
    {
        readonly Dictionary<Tag, string> htmlNames = new Dictionary<Tag, string>
        {
            { Tag.None, "" },
            { Tag.Italic, "em"},
            { Tag.Strong, "strong"},
            { Tag.Paragraph, "p" }
        };

        protected virtual string GetHtmlTagName() => htmlNames[Tag];

        public SimpleParsedTag(Tag tag) : base(tag)
        {
        }

        public override string GetCurrentRepresentation()
        {
            if (Tag == Tag.None)
                return Value;
            var builder = new StringBuilder();
            builder.AppendFormat("<{0}", GetHtmlTagName());
            foreach (var property in Properties)
                builder.AppendFormat(" {0}=\"{1}\"", property.Key, property.Value);
            builder.AppendFormat(">{0}</{1}>", Value, GetHtmlTagName());
            return builder.ToString();
        }
    }
}
