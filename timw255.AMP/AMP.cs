using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using timw255.AMP.Passes;

namespace timw255.AMP
{
    public class AMP
    {
        private HtmlDocument _doc;
        private List<IPass> _passes;

        public List<Component> RequiredComponents { get; set; }

        public AMP()
        {
            _passes = new List<IPass>
            {
                new RemoveProhibitedTagPass(),
                new ImgTagTransformPass(),
                new IframeYouTubeTagTransformPass(),
                new VideoTagTransformPass(),
                new AudioTagTransformPass(),
                new HtmlCommentPass()
            };

            RequiredComponents = new List<Component>();
        }

        public void LoadHtml(string html)
        {
            _doc = new HtmlDocument();
            _doc.LoadHtml(html);
        }

        public string ConvertToAmpHtml()
        {
            string result = null;

            foreach (IPass pass in _passes)
            {
                pass.Pass(_doc);

                if (pass.RequiredComponents.Count > 0)
                {
                    var components = pass.RequiredComponents.Where(c1 => !RequiredComponents.Any(c2 => c1.ElementName == c2.ElementName));

                    foreach (var c in components)
                    {
                        RequiredComponents.Add(c);
                    }
                }
            }

            using (StringWriter writer = new StringWriter())
            {
                _doc.Save(writer);
                result = writer.ToString();
            }

            return result;
        }
    }
}
