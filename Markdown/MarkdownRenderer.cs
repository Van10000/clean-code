using System;
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

        private readonly Dictionary<TagInfo, Tag[]> tagsInside = new Dictionary<TagInfo, Tag[]>
        {
            {TagInfo.None, new[] {Tag.Strong, Tag.Italic, Tag.Hyperlink}},
            {new TagInfo(Tag.Hyperlink, TagType.Opening), new[] {Tag.Strong, Tag.Italic}},
            {new TagInfo(Tag.Hyperlink, TagType.Middle), new Tag[0] },
            {new TagInfo(Tag.Italic, TagType.Opening), new Tag[0]},
            {new TagInfo(Tag.Strong, TagType.Opening), new[] {Tag.Italic}}
        };

        private readonly Stack<List<StringBuilder>> renderedParts = new Stack<List<StringBuilder>>();
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
                var currentBuilder = GetCurrentRenderedPartBuilder();
                var stopTags = GetCurrentStopTags();

                TagInfo stoppedAt;
                var parsedPart = markdown.ParseUntil(stopTags, out stoppedAt);
                currentBuilder.Append(parsedPart);
                if (ShouldCloseTag(stoppedAt.TagType))
                {
                    if (renderedParts.Count == 1)
                        return GetTopLevelResult();
                    if (GetLastTagStopTags().Contains(stoppedAt))
                        CloseTopLevelTag();
                    else while (tagsStack.Peek().GetOfNextType() != stoppedAt) // in case we closed tag at higher level
                        CloseTopLevelTag();
                }
                else if (stoppedAt.TagType == TagType.Middle)
                {
                    while (tagsStack.Peek().GetOfNextType() != stoppedAt) // in case we closed tag at higher level
                        CloseTopLevelTag();
                    renderedParts.Peek().Add(new StringBuilder());
                    tagsStack.Pop();
                    tagsStack.Push(stoppedAt);
                }
                else
                {
                    AddTopLevelStringBuilder();
                    tagsStack.Push(stoppedAt);
                }
            }
        }

        private string GetTopLevelResult()
        { 
            var currentList = renderedParts.Pop();
            AddTopLevelStringBuilder();
            renderedParts.Push(currentList);
            CloseTopLevelTag();
            return renderedParts.Pop().Last().ToString();
        }

        private void CloseTopLevelTag()
        {
            var currentList = renderedParts.Peek();
            renderedParts.Pop();
            var nextLevelBuilder = GetCurrentRenderedPartBuilder();
            if (tagsStack.Peek().Tag == Tag.Hyperlink)
            {
                tagsStack.Pop();
                var hyperlinkParts = currentList
                    .Select(builder => builder.ToString())
                    .ToArray();
                WrapHyperlinkIntoTag(nextLevelBuilder, hyperlinkParts);
            }
            else
                WrapIntoTag(nextLevelBuilder, currentList.Last().ToString(), tagsStack.Pop().Tag);
        }

        private void WrapHyperlinkIntoTag(StringBuilder builderToAddResult, string[] hyperlinkParts)
        {
            if (hyperlinkParts.Length != 2)
                throw new InvalidOperationException($"Hyperlink should contain exactly 2 parts. " +
                                                    $"Given hypelink contains {hyperlinkParts.Length} parts.");
            var link = ToCorrectLink(hyperlinkParts[1]);
            if (link == null)
                throw new NotImplementedException(); //TODO: should not happen, need to rewrite it somehow
            builderToAddResult.Append(representation.GetRepresentation(new TagInfo(Tag.Hyperlink, TagType.Opening)));
            builderToAddResult.Append(hyperlinkParts[1]);
            builderToAddResult.Append(representation.GetRepresentation(new TagInfo(Tag.Hyperlink, TagType.Middle)));
            builderToAddResult.Append(hyperlinkParts[0]);
            builderToAddResult.Append(representation.GetRepresentation(new TagInfo(Tag.Hyperlink, TagType.Closing)));
        }
        
        private string ToCorrectLink(string link)
        {
            var skippedSpaces = link
                .SkipWhile(char.IsWhiteSpace)
                .TakeWhile(ch => !char.IsWhiteSpace(ch))
                .Aggregate(new StringBuilder(), (builder, ch) => builder.Append(ch))
                .ToString();
            if (skippedSpaces.Count(char.IsWhiteSpace) != link.Count(char.IsWhiteSpace))
                return null;
            return skippedSpaces;
        }

        private IEnumerable<TagInfo> GetCurrentStopTags()
        {
            foreach (var tag in GetLastTagStopTags())
                yield return tag;
            foreach (var tag in tagsStack)
                yield return tag.GetOfNextType(); // higher level tags
        }

        private IEnumerable<TagInfo> GetLastTagStopTags()
        {
            foreach (var tag in tagsInside[tagsStack.Peek()])
                yield return new TagInfo(tag, TagType.Opening);
            yield return tagsStack.Peek().GetOfNextType();
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

        private StringBuilder GetCurrentRenderedPartBuilder()
        {
            return renderedParts.Peek().Last();
        }

        private void ClearState()
        {
            tagsStack.Clear();
            renderedParts.Clear();
        }

        private void AddInitialStateToStacks()
        {
            tagsStack.Push(TagInfo.None);
            AddTopLevelStringBuilder();
        }

        private void AddTopLevelStringBuilder()
        {
            renderedParts.Push(new List<StringBuilder> { new StringBuilder() });
        }
    }
}