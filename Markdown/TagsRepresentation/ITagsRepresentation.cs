using Markdown.MarkdownEnumerable;

namespace Markdown.TagsRepresentation
{
    internal interface ITagsRepresentation
    {
        string GetOpeningTag(Tag tag);

        string GetClosingTag(Tag tag);
    }
}
