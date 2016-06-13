using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace timw255.AMP.Passes
{
    public abstract class BasePass
    {
        protected const double DEFAULT_ASPECT_RATIO = 1.7778;
        protected const int DEFAULT_VIDEO_WIDTH = 560;
        protected const int DEFAULT_VIDEO_HEIGHT = 315;

        public List<Component> RequiredComponents { get; set; }

        public BasePass()
        {
            RequiredComponents = new List<Component>();
        }

        protected void EnsureHeightAndWidthAttributes(HtmlNode n)
        {
            int width = 0;
            int height = 0;

            Dictionary<string, string> ytAttrs = new Dictionary<string, string>();

            foreach (HtmlAttribute attr in n.Attributes)
            {
                if (attr.Name == "height")
                {
                    height = int.Parse(attr.Value);
                    continue;
                }

                if (attr.Name == "width")
                {
                    width = int.Parse(attr.Value);
                    continue;
                }
            }

            if (height == 0 && width != 0)
            {
                height = (int)(width / DEFAULT_ASPECT_RATIO);
            }

            if (height != 0 && width == 0)
            {
                width = (int)(height * DEFAULT_ASPECT_RATIO);
            }

            if (height == 0 && width == 0)
            {
                height = DEFAULT_VIDEO_HEIGHT;
                width = DEFAULT_VIDEO_WIDTH;
            }

            if (n.Attributes["height"] == null)
            {
                n.Attributes.Add("height", height.ToString());
            }

            if (n.Attributes["width"] == null)
            {
                n.Attributes.Add("width", width.ToString());
            }
        }

        /// <summary>
        /// <para>Remove attributes from an HtmlNode (Leaves 'height', 'width', and 'layout' by default)</para>
        /// </summary>
        /// <param name="n"></param>
        /// <param name="additionalAttributeNames">Additional attributes to preserve</param>
        /// <param name="preserveDataAttributres">If 'true' preserves all 'data-' attributes</param>
        protected virtual void ScrubAttributes(HtmlNode n, string[] additionalAttributeNames = null, bool preserveDataAttributres = false)
        {
            List<string> preserve = new List<string>() { "height", "width", "layout" };

            if (additionalAttributeNames != null)
            {
                preserve.AddRange(additionalAttributeNames);
            }

            var attrCollection = n.Attributes.ToList();

            foreach (HtmlAttribute attr in attrCollection)
            {
                if (!preserve.Contains(attr.Name) && !(preserveDataAttributres && attr.Name.StartsWith("data-")))
                {
                    n.Attributes.Remove(attr);
                }
            }
        }
    }
}
