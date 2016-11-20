using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
