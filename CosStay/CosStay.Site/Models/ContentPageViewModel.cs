using CosStay.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CosStay.Site.Models
{
    public class ContentPageViewModel
    {
        public string Uri { get; set; }

        public string Title { get; set; }
        public string MarkdownContent { get; set; }
        public string HtmlContent { get; set; }

        public ContentPageVersionStatus Status { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset PublishDate { get; set; }
    }

}