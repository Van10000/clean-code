using Markdown.MarkdownEnumerable;

namespace Markdown.TagsRepresentation
{
    interface ITagsRepresentation
    {
        string GetOpeningTag(Tag tag);

        string GetClosingTag(Tag tag);
    }
}
