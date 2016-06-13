using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace timw255.AMP.Passes
{
    public class RemoveProhibitedTagPass : BasePass, IPass
    {
        public void Pass(HtmlDocument doc)
        {
            List<string> prohibited = new List<string>()
            {
                "//script[not(type=application/ld+json)]",
                "//base",
                "//frame",
                "//frameset",
                "//object",
                "//param",
                "//applet",
                "//embed",
                "//form",
                "//input",
                "//textarea",
                "//select",
                "//option",
                "//style"
            };

            var nodes = doc.DocumentNode.SelectNodes(string.Join("|", prohibited));

            if (nodes != null)
            {
                foreach (HtmlNode n in nodes)
                {
                    n.Remove();
                }
            }

            nodes = doc.DocumentNode.SelectNodes("@*[starts-with(name(), 'on')]");

            if (nodes != null)
            {
                foreach (HtmlNode n in nodes)
                {
                    var attributes = n.Attributes.Where(a => a.Name.StartsWith("on"));

                    foreach (HtmlAttribute a in attributes)
                    {
                        n.Attributes.Remove(a);
                    }
                }
            }

            nodes = doc.DocumentNode.SelectNodes("a[starts-with(@href, 'javascript')]");

            if (nodes != null)
            {
                foreach (HtmlNode n in nodes)
                {
                    n.Attributes["href"].Value = "#";
                }
            }
        }
    }
}
