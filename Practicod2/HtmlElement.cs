using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practicod2
{
    public class HtmlElement
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Attributes { get; set; }
        public List<string> Classes { get; set; }
        public string InnerHtml { get; set; }
        public HtmlElement Parent { get; set; }
        public List<HtmlElement> Children { get; set; }
        public HtmlElement(string id, string name, List<string> attributes, List<string> classes, string innerHtml)
        {
            Id = id;
            Name = name;
            Attributes = attributes;
            Classes = classes;
            InnerHtml = innerHtml;
            Parent = null;
            Children = new List<HtmlElement>();
        }
        public IEnumerable<HtmlElement> Descendants()
        {
            Queue<HtmlElement> queue = new Queue<HtmlElement>();
            queue.Enqueue(this);

            while (queue.Count > 0)
            {
                HtmlElement currentElement = queue.Dequeue();
                yield return currentElement;

                foreach (var child in currentElement.Children)
                {
                    queue.Enqueue(child);
                }
            }
        }
        public IEnumerable<HtmlElement> Ancestors()
        {
            HtmlElement current = this;

            // כל עוד קיים הורה
            while (current.Parent != null)
            {
                // תשואה של ההורה
                yield return current.Parent;

                // מעבר להורה הבא
                current = current.Parent;
            }
        }
        public override string ToString()
        {
            string idPart = string.IsNullOrEmpty(Id) ? "" : $" (Id: {Id})";
            string classesPart = Classes.Any() ? $" (Classes: {string.Join(", ", Classes)})" : "";
            return $"<{Name}{idPart}{classesPart}>";
        }

    }



}