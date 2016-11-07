using System;
using Markdown.CharParsers;
using NUnit.Framework;

namespace Markdown
{
	public class MarkdownRenderer
	{
	    public readonly string Markdown;

	    public MarkdownRenderer(string markdown)
	    {
	        Markdown = markdown;
	    }

		public string RenderToHtml()
		{
			return Markdown; //TODO
		}

	    private string RenderToHtml(ICharParser parser, int startPosition, out int positionAfterLastRendered)
	    {
	        throw new NotImplementedException();
	    }
    }

	[TestFixture]
	internal class MarkdownRenderer_Should
	{
	}
}