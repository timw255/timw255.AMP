using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace timw255.AMP.Passes
{
    public class ImgTagTransformPass : BasePass, IPass
    {
        public void Pass(HtmlDocument doc)
        {
            var nodes = doc.DocumentNode.SelectNodes("//img[not(ancestor::noscript)]");

            if (nodes != null)
            {
                foreach (HtmlNode n in nodes)
                {
                    n.Name = "amp-img";

                    if (n.Attributes["layout"] == null)
                    {
                        n.Attributes.Add("layout", "responsive");
                    }

                    EnsureHeightAndWidthAttributes(n);
                    ScrubAttributes(n, new string[] { "src" });
                }
            }
        }
    }
}
