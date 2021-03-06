﻿using System;
using Markdown.MarkdownEnumerable;
using Markdown.MarkdownEnumerable.Tags;

namespace Markdown.TagsRepresentation
{
    internal class HyperlinkParsedTag : SimpleParsedTag
    {
        public HyperlinkParsedTag() : base(Tag.Hyperlink)
        {
        }

        protected override string GetHtmlTagName() => "a";

        public override void AddValueOrProperty(string valueOrProperty, TagType tagType, string baseUrl)
        {
            if (tagType.TagPart == HyperlinkTagInfo.VALUE_PART)
                Value = valueOrProperty;
            else
            {
                var correctLink = MarkdownParsingUtils.ToCorrectLink(valueOrProperty);
                if (correctLink == null)
                    throw new InvalidOperationException("Link is incorrect"); // should not happen
                var absoluteLink = IsRelativeLink(correctLink) ? GetAbsoluteLink(correctLink, baseUrl) : correctLink;
                AddProperty("href", absoluteLink);
            }
        }

        private string GetAbsoluteLink(string relativeLink, string baseUrl)
        {
            if (baseUrl == null)
                throw new InvalidOperationException("Base url was not specified.");
            return MarkdownParsingUtils.CombineLinks(baseUrl, relativeLink);
        }

        private bool IsRelativeLink(string correctLink)
        {
            return correctLink.Length == 0 || correctLink[0] == '/';
        }

    }
}
