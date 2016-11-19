using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Markdown.MarkdownEnumerable;
using Markdown.TagsRepresentation;

namespace Markdown
{
    //this class is thread unsafe
    public class MarkdownRenderer
    {
        private readonly IMarkdownEnumerable markdown;

        private readonly Dictionary<Tag, TagInfo[]> stopTagsDict = new Dictionary<Tag, TagInfo[]>
        {
            {Tag.Italic,    new[] { new TagInfo(Tag.Italic, TagType.Closing) } },

            {Tag.Strong,    new[] { new TagInfo(Tag.Strong, TagType.Closing),
                                    new TagInfo(Tag.Italic, TagType.Opening) } },

            {Tag.None,      new[] { new TagInfo(Tag.Italic, TagType.Opening),
                                    new TagInfo(Tag.Strong, TagType.Opening),
                                    new TagInfo(Tag.Hyperlink, TagType.Opening)} },

            {Tag.Hyperlink, new[] { new TagInfo(Tag.Italic, TagType.Opening),
                                    new TagInfo(Tag.Strong, TagType.Opening),
                                    new TagInfo(Tag.Hyperlink, TagType.Middle)} }
        };

        private readonly Stack<StringBuilder> renderedParts = new Stack<StringBuilder>();
        private readonly Stack<TagInfo> tagsStack = new Stack<TagInfo>();

        private readonly ITagsRepresentation representation;

        public MarkdownRenderer(IMarkdownEnumerable markdown, ITagsRepresentation representation)
        {
            this.markdown = markdown;
            this.representation = representation;
        }

        public string RenderToHtml()
        {
            ClearState();
            AddInitialStateToStacks();

            while (true)
            {
                var curTag = tagsStack.Peek();
                var stopTags = stopTagsDict[curTag.Tag]; // here will be a bit more complicated
                var currentBuilder = renderedParts.Peek();

                TagInfo stoppedAt;
                var parsedPart = markdown.ParseUntil(stopTags, out stoppedAt);
                currentBuilder.Append(parsedPart);
                if (ShouldCloseTag(stoppedAt.TagType))
                {
                    renderedParts.Pop();
                    if (renderedParts.Count == 0)
                        return currentBuilder.ToString();
                    var nextLevelBuilder = renderedParts.Peek();
                    WrapIntoTag(nextLevelBuilder, currentBuilder.ToString(), tagsStack.Pop().Tag/*here lose information*/);
                }
                else // here will also be case with middle
                {
                    renderedParts.Push(new StringBuilder());
                    tagsStack.Push(stoppedAt);
                }
            }
        }

        private void WrapIntoTag(StringBuilder builderToAddResult, string str, Tag tag)
        {
            builderToAddResult.Append(representation.GetRepresentation(new TagInfo(tag, TagType.Opening)));
            builderToAddResult.Append(str);
            builderToAddResult.Append(representation.GetRepresentation(new TagInfo(tag, TagType.Closing)));
        }

        private bool ShouldCloseTag(TagType tagType)
        {
            return tagType == TagType.Closing || tagType == TagType.None; // none if markdown finished
        }

        private void ClearState()
        {
            tagsStack.Clear();
            renderedParts.Clear();
        }

        private void AddInitialStateToStacks()
        {
            tagsStack.Push(TagInfo.None);
            renderedParts.Push(new StringBuilder());
        }
    }


}