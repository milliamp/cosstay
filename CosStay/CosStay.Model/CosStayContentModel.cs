using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosStay.Model
{
    public class ContentPage:IEntity
    {
        public int Id { get; set; }
        public string Uri  {get; set;}
        public virtual List<ContentPageVersion> Versions { get; set; }
    }

    public class ContentPageVersion:IEntity
    {
        public int Id { get; set; }
        public int Version { get; set; }
        public virtual ContentPage Page { get; set; }

        public string Title { get; set; }
        public string MarkdownContent { get; set; }

        public ContentPageVersionStatus Status { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset PublishDate { get; set; }
    }

    public enum ContentPageVersionStatus
    {
        Draft,
        Accepted,
        Pending,
        Published,
        Deleted
    }
}
