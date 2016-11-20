using System;
using System.Collections.Generic;
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

        private readonly Stack<TagType> tagsStack = new Stack<TagType>();
        private readonly Stack<StringBuilder> renderedParts = new Stack<StringBuilder>();
        private readonly Stack<ParsedTag> renderedTags = new Stack<ParsedTag>();

        private readonly ITagsRepresentation representation;
        private readonly string baseUrl;
        private readonly string cssClass;

        private IEnumerable<TagInfo> TagInfosStack
            => tagsStack.Zip(renderedTags.Select(parsedTag => parsedTag.Tag), (tagType, tag) => new TagInfo(tag, tagType));

        private TagInfo CurrentTagInfo => new TagInfo(renderedTags.Peek().Tag, tagsStack.Peek());

        public static string RenderToHtml(string markdown)
        {
            var renderer = new MarkdownRenderer(new StringMarkdownEnumerable(markdown), new HtmlTagsRepresentation());
            return renderer.RenderToHtml();
        }

        public MarkdownRenderer(IMarkdownEnumerable markdown, ITagsRepresentation representation,
            string baseUrl = null, string cssClass = null)
        {
            this.markdown = markdown;
            this.representation = representation;
            this.baseUrl = baseUrl;
            this.cssClass = cssClass;
        }

        public string RenderToHtml()
        {
            ClearState();
            PushTag(TagInfo.None);

            while (true)
            {
                var currentBuilder = renderedParts.Peek();
                var stopTags = GetCurrentStopTags();

                TagInfo stoppedAt;
                var parsedPart = markdown.ParseUntil(stopTags, out stoppedAt);
                currentBuilder.Append(parsedPart);

                if (ShouldCloseTag(stoppedAt.TagType))
                {
                    CloseLowerLevelsOfTags(stoppedAt);
                    if (tagsStack.Count == 1)
                        return renderedParts.Peek().ToString();
                    CloseTopLevelTag();
                }
                else if (stoppedAt.TagType == TagType.Middle)
                {
                    CloseLowerLevelsOfTags(stoppedAt);
                    AddCurrentValueToParsedTag();
                    tagsStack.Pop();
                    tagsStack.Push(stoppedAt.TagType);
                    renderedParts.Pop();
                    renderedParts.Push(new StringBuilder());
                }
                else
                {
                    PushTag(stoppedAt);
                }
            }
        }

        private void AddCurrentValueToParsedTag()
        {
            var currentTag = CurrentTagInfo;
            var collectedValue = renderedParts.Peek().ToString();
            if (currentTag.Tag == Tag.Hyperlink)
            {
                if (currentTag.TagType == TagType.Opening)
                    renderedTags.Peek().Value = collectedValue;
                else
                {
                    var correctLink = MarkdownParsingUtils.ToCorrectLink(collectedValue);
                    if (correctLink == null)
                        throw new InvalidOperationException("Link is incorrect"); // should not happen
                    var absoluteLink = IsRelativeLink(correctLink) ? GetAbsoluteLink(correctLink) : correctLink;
                    renderedTags.Peek().AddProperty("href", absoluteLink);
                }
            }
            else
            {
                renderedTags.Peek().Value = collectedValue;
            }
        }

        private void CloseLowerLevelsOfTags(TagInfo stoppedAt)
        {
            while (CurrentTagInfo.GetOfNextType() != stoppedAt)
                CloseTopLevelTag();
        }

        private void CloseTopLevelTag()
        {
            AddCurrentValueToParsedTag();
            var rendered = PopTag();
            renderedParts.Peek().Append(rendered);
        }

        private string GetAbsoluteLink(string relativeLink)
        {
            if (baseUrl == null)
                throw new InvalidOperationException("Base url was not specified.");
            return MarkdownParsingUtils.CombineLinks(baseUrl, relativeLink);
        }

        private bool IsRelativeLink(string correctLink)
        {
            return correctLink.Length == 0 || correctLink[0] == '/';
        }

        private IEnumerable<TagInfo> GetCurrentStopTags()
        {
            foreach (var tag in GetLastTagStopTags())
                yield return tag;
            foreach (var tag in TagInfosStack)
                yield return tag.GetOfNextType(); // higher level tags
        }

        private IEnumerable<TagInfo> GetLastTagStopTags()
        {
            foreach (var tag in tagsInside[CurrentTagInfo])
                yield return new TagInfo(tag, TagType.Opening);
            yield return CurrentTagInfo.GetOfNextType();
        }

        private bool ShouldCloseTag(TagType tagType)
        {
            return tagType == TagType.Closing || tagType == TagType.None; // none if markdown finished
        }

        private void ClearState()
        {
            tagsStack.Clear();
            renderedParts.Clear();
            renderedTags.Clear();
        }

        private void PushTag(TagInfo tagInfo)
        {
            tagsStack.Push(tagInfo.TagType);
            renderedTags.Push(new ParsedTag(tagInfo.Tag, representation));
            renderedParts.Push(new StringBuilder());
        }

        private string PopTag()
        {
            tagsStack.Pop();
            renderedParts.Pop();
            if (cssClass != null)
                renderedTags.Peek().AddProperty("class", cssClass);
            return renderedTags.Pop().GetCurrentRepresentation();
        }
    }
}