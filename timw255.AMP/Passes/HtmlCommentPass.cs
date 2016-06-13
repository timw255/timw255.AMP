using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace timw255.AMP.Passes
{
    public class HtmlCommentPass : BasePass, IPass
    {
        public void Pass(HtmlDocument doc)
        {
            HtmlNodeCollection comments = doc.DocumentNode.SelectNodes("//comment()");

            if (comments != null)
            {
                foreach (HtmlNode comment in comments)
                {
                    if (Regex.IsMatch(comment.InnerText, @"(*UTF8)\[if", RegexOptions.IgnoreCase) || Regex.IsMatch(comment.InnerText, @"(*UTF8)\[endif", RegexOptions.IgnoreCase))
                    {
                        comment.ParentNode.RemoveChild(comment);
                    }
                }
            }
        }
    }
}
