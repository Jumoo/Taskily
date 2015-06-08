using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Web; 
using System.Web.Mvc;
using System.IO;

using MarkdownDeep;

namespace Jumoo.Content.Markdown
{
    public static class HtmlContentHelper
    {
        public static IHtmlString ContentBlob(this HtmlHelper helper, string path)
        {
            var context = helper.ViewContext.HttpContext;
            var file = context.Server.MapPath(string.Format("~/app_content/{0}.md", path));

            if ( File.Exists(file) )
            {
                using (StreamReader sr = new StreamReader(file))
                {
                    string text = sr.ReadToEnd();

                    var markdown = new MarkdownDeep.Markdown();
                    return MvcHtmlString.Create(markdown.Transform(text));
                }
            }

            return MvcHtmlString.Create("<em>Missing " + file + "</em>");
            
        }
    }
}
