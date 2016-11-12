using FluentAssertions;
using Markdown;
using NUnit.Framework;

namespace MarkdownTests
{
    [TestFixture]
    internal class MarkdownRenderer_Should
    {
        [TestCase("ab _c_ de", "ab <em>c</em> de", TestName = "Simple italic tag")]
        [TestCase("ab __c__ de", "ab <strong>c</strong> de", TestName = "Simple strong tag")]
        [TestCase("__a _b_ c__", "<strong>a <em>b</em> c</strong>", TestName = "Italic inside strong")]
        [TestCase("_a __b__ c_", "<em>a __b__ c</em>", TestName = "Strong not inside italic")]
        [TestCase(@"\_a\_", "_a_", TestName = "Escape italic")]
        [TestCase("_1_", "_1_", TestName = "digit near to highlight")]
        [TestCase("___abc___", "___abc___", TestName = "underscore near to highlight")]
        [TestCase("__a_", "<strong>a_</strong>", TestName = "Not matching highlights")]
        [TestCase("_ b_", "_ b_", TestName = "Space symbol after start of highlight is not allowed")]
        [TestCase("_a _", "<em>a _</em>", TestName = "Space symbol before end of highlight is not allowed")]
        [TestCase("a_b_", "a_b_", TestName = "Non-space symbol before the start of highlight")]
        [TestCase("_a b_c", "<em>a b_c</em>", TestName = "Non-space symbol after end of highlight")]
        public void RenderToHtml(string markdown, string expectedHtmlResult)
        {
            var renderer = new MarkdownRenderer(markdown);

            var htmlResult = renderer.RenderToHtml();

            htmlResult.Should().Be(expectedHtmlResult);
        }
    }
}
