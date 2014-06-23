using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CosStay.Site
{
    public static class PhotoHelper
    {
        public const string PHOTO_FOLDER = "/photos/";
        public static string Photo(this UrlHelper helper, Guid id, int? width = null, int? height = null)
        {
            var str = id.ToString();
            var path = PHOTO_FOLDER + str.Substring(0, 1) + "/" + str + ".jpg";
            var options = new Dictionary<string, string>();
            if (width != null)
                options.Add("width", width.ToString());
            if (height != null)
                options.Add("height", height.ToString());
            if (options.Any())
                path += "?" + string.Join("&", options.Select(o => o.Key + "=" + o.Value));

            return path;
        }


    }
}