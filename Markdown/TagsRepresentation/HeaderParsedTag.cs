using Markdown.MarkdownEnumerable.Tags;

namespace Markdown.TagsRepresentation
{
    internal class HeaderParsedTag : SimpleParsedTag
    {
        private readonly int headerLevel;

        protected override string GetHtmlTagName() => "h" + headerLevel;

        public HeaderParsedTag(HeaderTagInfo tagInfo) : base(tagInfo.Tag)
        {
            headerLevel = tagInfo.HeaderLevel;
        }
    }
}
