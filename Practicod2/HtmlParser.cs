using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Practicod2
{
    public class HtmlParser
    {
        private static readonly HtmlHelper helper = HtmlHelper.Instance;

        public static HtmlElement ParseHtml(string html)

        {

            var cleanHtml = new Regex("\\n").Replace(html, "");

            var htmlLines = new Regex("<(.*?)>").Split(cleanHtml).Where(s => s.Length > 0);

            Stack<HtmlElement> stack = new Stack<HtmlElement>();

            HtmlElement root = new HtmlElement("root", "root", new List<string>(), new List<string>(), "");

            HtmlElement currentElement = root;



            foreach (var token in htmlLines)

            {

                var firstWord = token.Split(' ')[0];



                if (firstWord == "/html")

                {

                    // הגענו לסוף הקוד

                    break;

                }

                else if (firstWord.StartsWith("/"))

                {

                    // תג סגירה, בדוק תקינות ונעלה לרמת האב

                    string closingTagName = firstWord.Substring(1);


                    currentElement = stack.Pop();

                }

                else if (helper.SelfClosingHtmlTags.Contains(firstWord))

                {

                    // תג עצמאי - צור אלמנט חדש והוסף כבן

                    HtmlElement newElement = new HtmlElement(null, firstWord, new List<string>(), new List<string>(), "");

                    currentElement.Children.Add(newElement);

                    UpdateAttributesAndContent(newElement, token);

                }

                else if (helper.AllHtmlTags.Contains(firstWord))

                {

                    // תג פתיחה - צור אלמנט חדש, הוסף כבן ועבור אליו

                    HtmlElement newElement = new HtmlElement(null, firstWord, new List<string>(), new List<string>(), "");

                    newElement.Parent = currentElement;

                    currentElement.Children.Add(newElement);

                    currentElement = newElement;

                    stack.Push(newElement);

                    UpdateAttributesAndContent(newElement, token);

                }

                else

                {

                    // טקסט פנימי - עדכן את InnerHtml

                    currentElement.InnerHtml += token;

                    continue;

                }

            }



            return root;

        }



        private static void UpdateAttributesAndContent(HtmlElement element, string token)

        {

            // פרקי את המשך המחרוזת (ללא המילה הראשונה) וצרי את רשימת ה-Attributes.

            var remainingToken = token.Substring(token.IndexOf(' ') + 1);

            var attributes = new List<string>();



            // פרק את המשך המחרוזת לפי רווחים, אם יש Attribute בשם class.

            var attributeParts = remainingToken.Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries).ToList();


            foreach (var attributePart in attributeParts)

            {

                var keyValue = attributePart.Split('=');

                var attributeName = keyValue[0];

                var attributeValue = keyValue.Length > 1 ? keyValue[1].Trim('"') : "";



                // אם ה-attribute הוא 'class', פרקי למספר חלקים.

                if (attributeName.ToLower() == "class")

                {

                    // שינוי: פירוק לפי רווחים ויצירת רשימה של classes

                    var classes = attributeValue.Split(' ');

                    element.Classes.AddRange(classes);

                }

                else if (attributeName.ToLower() == "id")

                {

                    // אם מצאנו id, הוסף את ערכו לתוך InnerHtml

                    element.Id = attributeValue;

                }



                attributes.Add($"{attributeName}={attributeValue}");

            }



            // שימי במאפיינים המתאימים ב- HtmlElement.

            element.Attributes = attributes;



            // כמו כן עדכני את ה-Parent.

            var parentElement = element.Parent;

            if (parentElement != null)

            {

                parentElement.Children.Remove(element);

                parentElement.Children.Add(element);

            }

        }
    }
}