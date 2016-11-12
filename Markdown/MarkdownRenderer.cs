using System.Collections.Generic;
using System.Text;
using Markdown.MarkdownEnumerable;
using Markdown.TagsRepresentation;

namespace Markdown
{
	public class MarkdownRenderer
	{
	    private readonly IMarkdownEnumerable markdown;

	    private readonly Dictionary<Tag, TagInfo[]> stopTagsDict = new Dictionary<Tag, TagInfo[]>
	    {
            {Tag.Italic, new[] { new TagInfo(Tag.Italic, TagType.Closing) } },

            {Tag.Strong, new[] { new TagInfo(Tag.Strong, TagType.Closing),
                                 new TagInfo(Tag.Italic, TagType.Opening) } },

            {Tag.None,   new[] { new TagInfo(Tag.Italic, TagType.Opening),
                                 new TagInfo(Tag.Strong, TagType.Opening)} }
	    };

	    public MarkdownRenderer(IMarkdownEnumerable markdown)
	    {
	        this.markdown = markdown;
	    }

	    public MarkdownRenderer(string markdown)
	    {
	        this.markdown = new StringMarkdownEnumerable(markdown);
	    }

		public string RenderToHtml()
		{
		    return Render(Tag.None, new HtmlTagsRepresentation());
		}
        
	    private string Render(Tag curTag, ITagsRepresentation tagsRepresentation)
	    {
	        var stopTags = stopTagsDict[curTag];
	        var resultBuilder = new StringBuilder();

	        while (true)
	        {
	            TagInfo stoppedAt;
	            var parsedPart = markdown.ParseUntil(stopTags, out stoppedAt);
                resultBuilder.Append(parsedPart);
	            if (ShouldCloseTag(stoppedAt.TagType))
	                return resultBuilder.ToString();
                var renderedInsideTag = Render(stoppedAt.Tag, tagsRepresentation);
                WrapIntoTag(resultBuilder, renderedInsideTag, stoppedAt.Tag, tagsRepresentation));
            }
	    }

	    private bool ShouldCloseTag(TagType tagType)
	    {
	        return tagType == TagType.Closing || tagType == TagType.None; // none if markdown finished
	    }

        private void WrapIntoTag(StringBuilder builderToAddResult, string str, Tag tag, ITagsRepresentation representation)
        {
            builderToAddResult.Append(representation.GetOpeningTag(tag));
            builderToAddResult.Append(str);
            builderToAddResult.Append(representation.GetClosingTag(tag));
        }
    }

   
}