using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.MarkdownEnumerable;
using Markdown.TagsRepresentation;

namespace Markdown
{
    public static class MarkdownRenderingUtils
    {
        public static string RenderToHtml(string markdown)
        {
            var renderer = new MarkdownRenderer(new StringMarkdownEnumerable(markdown), new HtmlTagsRepresentation());
            return renderer.RenderToHtml();
        }
    }
}
