using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Practicod2
{
    public class HtmlHelper
    {
        private readonly static HtmlHelper instance = new HtmlHelper();
        public static HtmlHelper Instance=>instance;
        public string[] AllHtmlTags { get; private set; }
        public string[] SelfClosingHtmlTags { get; private set; }

        private HtmlHelper()
        {
          
            var allTagsJson = File.ReadAllText("seed/AllTags.json");
            var allTags = JsonSerializer.Deserialize<string[]>(allTagsJson);

            // קריאה לקובץ JSON עם תגיות שלא דורשות סגירה
            var selfClosingTagsJson = File.ReadAllText("seed/SelfClosingTags.json");
            var selfClosingTags = JsonSerializer.Deserialize<string[]>(selfClosingTagsJson);

            // טעינת הנתונים למאפיינים
            AllHtmlTags = allTags;
            SelfClosingHtmlTags = selfClosingTags;
        }

       
    }
}
