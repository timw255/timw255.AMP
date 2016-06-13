using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace timw255.AMP.Passes
{
    public class VideoTagTransformPass : BasePass, IPass
    {
        public void Pass(HtmlDocument doc)
        {
            var nodes = doc.DocumentNode.SelectNodes("//video");

            if (nodes != null)
            {
                foreach (HtmlNode n in nodes)
                {
                    n.Name = "amp-video";

                    if (n.Attributes["layout"] == null)
                    {
                        n.Attributes.Add("layout", "responsive");
                    }

                    if (n.Attributes["controls"] == null && n.Attributes["autoplay"] == null)
                    {
                        n.Attributes.Add("controls", null);
                    }

                    EnsureHeightAndWidthAttributes(n);
                    ScrubAttributes(n, new string[] { "src", "poster", "controls" });
                }
            }   
        }
    }
}
