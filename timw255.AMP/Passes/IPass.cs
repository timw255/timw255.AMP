using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace timw255.AMP.Passes
{
    public interface IPass
    {
        List<Component> RequiredComponents { get; set; }

        void Pass(HtmlDocument doc);
    }
}
