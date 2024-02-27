using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practicod2
{
    public static class HtmlElementExtensions
    {
        public static IEnumerable<HtmlElement> Select(this HtmlElement element, Selector selector)
        {
            HashSet<HtmlElement> results = new HashSet<HtmlElement>();
            SelectInternal(element, selector, results);
            return results;
        }

        private static void SelectInternal(HtmlElement element, Selector selector, HashSet<HtmlElement> results)
        {
            if (element.MatchesSelector(selector))
            {
                results.Add(element);
            }

            foreach (var child in element.Children)
            {
                SelectInternal(child, selector, results);
            }
        }

        public static bool MatchesSelector(this HtmlElement element, Selector selector)
        {
            if (element == null || selector == null)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(selector.TagName) && !element.Name.Equals(selector.TagName, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (!string.IsNullOrEmpty(selector.Id) && element.Id != selector.Id)
            {
                return false;
            }

            if (selector.Classes.Any() && !element.Classes.Intersect(selector.Classes).Any())
            {
                return false;
            }

            return true;
        }

    }
}

