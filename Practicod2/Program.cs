
using Practicod2;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using System.Text.RegularExpressions;
using System.Xml.Linq;
HtmlSerializer htmlSerializer = new HtmlSerializer();
var html = await htmlSerializer.Load("https://forum.netfree.link/category/1/%D7%94%D7%9B%D7%A8%D7%96%D7%95%D7%AA");
var dom = HtmlParser.ParseHtml(html);
var result = dom.Select(Selector.FromQueryString("ul.nav.navbar-nav"));
result.ToList().ForEach(x => Console.WriteLine(x.ToString()));


