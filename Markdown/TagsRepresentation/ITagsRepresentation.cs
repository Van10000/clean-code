using Markdown.MarkdownEnumerable;

namespace Markdown.TagsRepresentation
{
    internal interface ITagsRepresentation
    {
        string GetRepresentation(TagInfo tagInfo);
    }
}
