using Markdown.MarkdownEnumerable.Tags;

namespace Markdown.TagsRepresentation
{
    internal class ParsedNewLineTag : ParsedTag
    {
        public ParsedNewLineTag() : base(Tag.NewLine)
        {
        }

        public override string GetCurrentRepresentation()
        {
            return "<br>";
        }

        public override void AddValueOrProperty(string valueOrProperty, TagType tagType, string baseUrl)
        {
            return;
        }
    }
}
