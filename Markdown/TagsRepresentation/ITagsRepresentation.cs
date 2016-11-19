using Markdown.MarkdownEnumerable;

namespace Markdown.TagsRepresentation
{
    public interface ITagsRepresentation
    {
        string GetRepresentation(TagInfo tagInfo);
    }
}
