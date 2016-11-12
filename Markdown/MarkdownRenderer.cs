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

	    /// <summary>
	    /// Recursive method.
	    /// If we meet closing tag - return parsed string.
	    /// If we meet tag of different type - recursively call this method.
	    /// <param name="curTag">Last tag we are inside</param>
	    /// </summary>
	    private string Render(Tag curTag, ITagsRepresentation tagsRepresentation)
	    {
	        // here will be a switch on curTag
	        // for parsing we will use MarkdownParsingExtentions.ParseUntil
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
                resultBuilder.Append(WrapIntoTag(renderedInsideTag, stoppedAt.Tag, tagsRepresentation));
            }
	    }

	    private bool ShouldCloseTag(TagType tagType)
	    {
	        return tagType == TagType.Closing || tagType == TagType.None; // none if markdown finished
	    }

        private string WrapIntoTag(string str, Tag tag, ITagsRepresentation representation)
        {
            StringBuilder result = new StringBuilder();
            result.Append(representation.GetOpeningTag(tag));
            result.Append(str);
            result.Append(representation.GetClosingTag(tag));
            return result.ToString();
        }
    }

   
}