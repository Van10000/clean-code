using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.MarkdownEnumerable;
using Markdown.MarkdownEnumerable.Tags;
using Markdown.TagsRepresentation;

namespace Markdown
{
    //this class is thread unsafe
    public class MarkdownRenderer
    {
        private readonly IMarkdownEnumerable markdown;

        private readonly Dictionary<TagInfo, Tag[]> tagsInside = new Dictionary<TagInfo, Tag[]>
        {
            {TagInfo.None, new [] {Tag.Paragraph}},
            {new HyperlinkTagInfo(TagPosition.Opening, HyperlinkTagInfo.VALUE_PART), new[] {Tag.Strong, Tag.Italic}},
            {new HyperlinkTagInfo(TagPosition.Opening, HyperlinkTagInfo.LINK_PART), new Tag[0] },
            {new SimpleTagInfo(Tag.Italic, TagPosition.Opening), new [] {Tag.NewLine}},
            {new SimpleTagInfo(Tag.Strong, TagPosition.Opening), new[] {Tag.Italic, Tag.NewLine}},
            {new ParagraphTagInfo(TagPosition.Opening), new[] {Tag.Strong, Tag.Italic, Tag.Hyperlink, Tag.NewLine, Tag.Header} },
            {new HeaderTagInfo(TagPosition.Opening), new[] {Tag.Strong, Tag.Italic, Tag.Hyperlink} }
        };

        private readonly Stack<TagType> tagsStack = new Stack<TagType>();
        private readonly Stack<StringBuilder> renderedParts = new Stack<StringBuilder>();
        private readonly Stack<ParsedTag> parsedTags = new Stack<ParsedTag>();
        
        private readonly string baseUrl;
        private readonly string cssClass;

        private IEnumerable<TagInfo> TagInfosStack
            => tagsStack.Zip(parsedTags.Select(parsedTag => parsedTag.Tag), (tagType, tag) => TagInfo.Create(tag, tagType));

        private TagInfo CurrentTagInfo => TagInfo.Create(parsedTags.Peek().Tag, tagsStack.Peek());

        public static string RenderToHtml(string markdown)
        {
            var renderer = new MarkdownRenderer(new StringMarkdownEnumerable(markdown));
            return renderer.RenderToHtml();
        }

        public MarkdownRenderer(IMarkdownEnumerable markdown, string baseUrl = null, string cssClass = null)
        {
            this.markdown = markdown;
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

                if (stoppedAt.ShouldClose())
                {
                    CloseLowerLevelsOfTags(stoppedAt);
                    if (tagsStack.Count == 1)
                        return renderedParts.Peek().ToString();
                    CloseTopLevelTag();
                }
                else
                {
                    PushTag(stoppedAt);
                    if (stoppedAt.IsSingle())
                        CloseTopLevelTag();
                }
            }
        }
        
        private void AddCurrentValueToParsedTag()
        {
            parsedTags.Peek().AddValueOrProperty(renderedParts.Peek().ToString(), tagsStack.Peek(), baseUrl);
        }

        private void CloseLowerLevelsOfTags(TagInfo stoppedAt)
        {
            while (CurrentTagInfo.GetOfNextType() != stoppedAt)
                CloseTopLevelTag();
        }

        private void CloseTopLevelTag()
        {
            AddCurrentValueToParsedTag();
            var closingTagInfo = CurrentTagInfo.GetOfNextType();

            if (closingTagInfo.IsNotLastPartOfTag())
            {
                TagInfo nextStoppedAt;
                markdown.ParseUntil(new[] { closingTagInfo.GetOfNextType() }, out nextStoppedAt);
                tagsStack.Pop();
                tagsStack.Push(nextStoppedAt.TagType);
                renderedParts.Pop();
                renderedParts.Push(new StringBuilder());
            }
            else
            {
                var rendered = PopTag();
                renderedParts.Peek().Append(rendered);
            }
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
                yield return TagInfo.Create(tag, TagType.FirstOpeningTag);
            yield return CurrentTagInfo.GetOfNextType();
        }

        private void ClearState()
        {
            tagsStack.Clear();
            renderedParts.Clear();
            parsedTags.Clear();
        }

        private void PushTag(TagInfo tagInfo)
        {
            tagsStack.Push(tagInfo.TagType);
            parsedTags.Push(ParsedTag.Create(tagInfo));
            renderedParts.Push(new StringBuilder());
        }

        private string PopTag()
        {
            tagsStack.Pop();
            renderedParts.Pop();
            if (cssClass != null)
                parsedTags.Peek().AddProperty("class", cssClass);
            return parsedTags.Pop().GetCurrentRepresentation();
        }
    }
}