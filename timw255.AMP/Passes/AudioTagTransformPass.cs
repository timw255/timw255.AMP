using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace timw255.AMP.Passes
{
    public class AudioTagTransformPass : BasePass, IPass
    {
        public void Pass(HtmlDocument doc)
        {
            var nodes = doc.DocumentNode.SelectNodes("//audio");

            if (nodes != null)
            {
                var c = new Component();
                c.ElementName = "amp-audio";
                c.ScriptPath = "https://cdn.ampproject.org/v0/amp-audio-0.1.js";

                RequiredComponents.Add(c);

                foreach (HtmlNode n in nodes)
                {
                    n.Name = "amp-audio";

                    if (n.Attributes["width"] == null)
                    {
                        n.Attributes.Add("width", "auto");
                    }

                    n.Attributes["width"].Value = "auto";
                }
            }   
        }
    }
}
