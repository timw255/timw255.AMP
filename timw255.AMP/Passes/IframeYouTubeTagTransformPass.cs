using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace timw255.AMP.Passes
{
    public class IframeYouTubeTagTransformPass : BasePass, IPass
    {
        public void Pass(HtmlDocument doc)
        {
            var nodes = doc.DocumentNode.SelectNodes("//iframe[not(ancestor::noscript)]");

            if (nodes != null)
            {
                var c = new Component();
                c.ElementName = "amp-youtube";
                c.ScriptPath = "https://cdn.ampproject.org/v0/amp-youtube-0.1.js";

                RequiredComponents.Add(c);

                foreach (HtmlNode n in nodes)
                {
                    if (!IsYouTubeIframe(n))
                    {
                        continue;
                    }

                    var ytCode = GetYouTubeCode(n);

                    if (string.IsNullOrWhiteSpace(ytCode))
                    {
                        continue;
                    }

                    HtmlNode ampYouTube = HtmlNode.CreateNode(string.Format("<amp-youtube data-videoid=\"{0}\" layout=\"responsive\"></amp-youtube>", ytCode));

                    EnsureHeightAndWidthAttributes(n);
                    ScrubAttributes(n, null, true);

                    foreach (HtmlAttribute attr in n.Attributes)
                    {
                        ampYouTube.Attributes.Add(attr.Name, attr.Value);
                    }

                    n.ParentNode.ReplaceChild(ampYouTube, n);
                }
            }
        }

        protected bool IsYouTubeIframe(HtmlNode n)
        {
            if (n.GetAttributeValue("src", false))
            {
                return false;
            }

            if (Regex.IsMatch(n.Attributes["src"].Value, @"(youtube\.com|youtu\.be)", RegexOptions.IgnoreCase))
            {
                return true;
            }

            return false;
        }

        protected string GetYouTubeCode(HtmlNode n)
        {
            Match match;
            
            match = Regex.Match(n.Attributes["src"].Value, @"/embed/([^/?]+)", RegexOptions.IgnoreCase);

            if (match.Success) {
                return match.Groups[1].Value;
            }

            match = Regex.Match(n.Attributes["src"].Value, @"youtu\.be/([^/?]+)", RegexOptions.IgnoreCase);

            if (match.Success) {
                return match.Groups[1].Value;
            }

            match = Regex.Match(n.Attributes["src"].Value, @"watch\?v=([^&]+)", RegexOptions.IgnoreCase);

            if (match.Success) {
                return match.Groups[1].Value;
            }

            return string.Empty;
        }
    }
}
