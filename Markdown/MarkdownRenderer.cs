using System;
using Markdown.MarkdownEnumerable;
using Markdown.TagsRepresentation;
using NUnit.Framework;

namespace Markdown
{
	public class MarkdownRenderer
	{
	    private readonly IMarkdownEnumerable markdown;

	    public MarkdownRenderer(IMarkdownEnumerable markdown)
	    {
	        this.markdown = markdown;
	    }

	    public MarkdownRenderer(string markdown)
	    {
	        
	    }

		public string RenderToHtml()
		{
		    throw new NotImplementedException();
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
	        throw new NotImplementedException();
	    }
    }

	[TestFixture]
	internal class MarkdownRenderer_Should
	{
	}
}