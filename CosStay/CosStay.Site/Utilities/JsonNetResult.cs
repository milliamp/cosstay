﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CosStay.Site
{
    public class JsonNetResult : JsonResult
    {
        
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            var response = context.HttpContext.Response;

            response.ContentType = !String.IsNullOrEmpty(ContentType) ? ContentType : "application/json";

            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;

            var settings = new JsonSerializerSettings
            {
                Converters = new[] {new IsoDateTimeConverter
            {
                DateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fffK"
            }}
            };
            var jsonSerializer = JsonSerializer.Create(settings);

            jsonSerializer.Serialize(response.Output, Data);
        }
    }

}