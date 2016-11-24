using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.MarkdownEnumerable.Tags;

namespace Markdown.TagsRepresentation
{
    class SimpleParsedTag : ParsedTag
    {
        readonly Dictionary<Tag, string> htmlNames = new Dictionary<Tag, string>
        {
            { Tag.None, "" },
            { Tag.Italic, "em"},
            { Tag.Strong, "strong"},
            { Tag.Hyperlink, "a"},
            { Tag.Paragraph, "p" }
        };

        public SimpleParsedTag(Tag tag) : base(tag)
        {
            if (!htmlNames.ContainsKey(tag))
                throw new ArgumentException($"Unknown tag:{tag}");
        }

        public override string GetCurrentRepresentation()
        {
            if (Tag == Tag.None)
                return Value;
            var builder = new StringBuilder();
            builder.AppendFormat("<{0}", htmlNames[Tag]);
            foreach (var property in Properties)
                builder.AppendFormat(" {0}=\"{1}\"", property.Key, property.Value);
            builder.AppendFormat(">{0}</{1}>", Value, htmlNames[Tag]);
            return builder.ToString();
        }
    }
}
