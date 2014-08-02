using AutoMapper;
using CosStay.Core.Services;
using CosStay.Model;
using CosStay.Site.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CosStay.Site.Controllers
{
    public class PhotoController : BaseController
    {
        public PhotoController(IEntityStore entityStore, IAuthorizationService authorizationService)
            : base(entityStore, authorizationService)
        {
        }

        public class UploadedFile
        {
            public int FileSize { get; set; }
            public string Filename { get; set; }
            public string ContentType { get; set; }
            public byte[] Contents { get; set; }
        }

        //
        // POST: /Photo/upload
        [Route("photos/upload")]
        [HttpPost]
        public async Task<ActionResult> Upload(
            int? locationId = null,
            int? eventId = null,
            int? eventInstanceId = null,
            int? venueId = null, 
            int? bedId = null,
            int? accomodationRoomId = null,
            int? accomodationVenueId = null
            )
        {
            // TODO: Check for Authorisation

            if (!locationId.HasValue
                && !eventId.HasValue
                && !eventInstanceId.HasValue
                && !venueId.HasValue
                && !bedId.HasValue
                && !accomodationRoomId.HasValue
                && !accomodationVenueId.HasValue)
                throw new InvalidOperationException("Will not create orphan photo record");

            if (locationId.HasValue)
                DenyIfNotAuthorizedAsync(ActionType.Update, await _es.GetAsync<Location>(locationId.Value));
            if (eventId.HasValue)
                DenyIfNotAuthorizedAsync(ActionType.Update, await _es.GetAsync<Event>(eventId.Value));
            if (eventInstanceId.HasValue)
                DenyIfNotAuthorizedAsync(ActionType.Update, await _es.GetAsync<EventInstance>(eventInstanceId.Value));
            if (venueId.HasValue)
                DenyIfNotAuthorizedAsync(ActionType.Update, await _es.GetAsync<Venue>(venueId.Value));
            if (bedId.HasValue)
                DenyIfNotAuthorizedAsync(ActionType.Update, await _es.GetAsync<Bed>(bedId.Value));
            if (accomodationRoomId.HasValue)
                DenyIfNotAuthorizedAsync(ActionType.Update, await _es.GetAsync<AccomodationRoom>(accomodationRoomId.Value));
            if (accomodationVenueId.HasValue)
                DenyIfNotAuthorizedAsync(ActionType.Update, await _es.GetAsync<AccomodationVenue>(accomodationVenueId.Value));

            var photo = new Photo()
            {
                 PhotoId = Guid.NewGuid(),
                 IsDeleted = false,
                 DateAdded = DateTimeOffset.Now,
                 AccomodationRoomId = accomodationRoomId,
                 AccomodationVenueId = accomodationVenueId,
                 BedId = bedId,
                 EventId = eventId,
                 EventInstanceId = eventInstanceId,
                 LocationId = locationId,
                 VenueId = venueId
            };

            UploadedFile file = await RetrieveFileFromRequestAsync();
            string savePath = string.Empty;
            string virtualPath = await SaveFile(file, photo.PhotoId);

            _es.Add(photo);
            await _es.SaveAsync();

            var pVm = Mapper.Map<PhotoViewModel>(photo);
            return Json(pVm);
        }
 
        private async Task<UploadedFile> RetrieveFileFromRequestAsync()
        {
            string filename = null;
            string fileType = null;
            byte[] fileContents = null;
 
            if (Request.Files.Count > 0)
            { //we are uploading the old way
                var file = Request.Files[0];
                fileContents = new byte[file.ContentLength];
                await file.InputStream.ReadAsync(fileContents, 0, file.ContentLength);
                fileType = file.ContentType;
                filename = file.FileName;
            }
            else if (Request.ContentLength > 0)
            {
                // Using FileAPI the content is in Request.InputStream!!!!
                fileContents = new byte[Request.ContentLength];
                await Request.InputStream.ReadAsync(fileContents, 0, Request.ContentLength);
                filename = Request.Headers["X-File-Name"];
                fileType = Request.Headers["X-File-Type"];
            }
 
            return new UploadedFile()
            {
                Filename = filename,
                ContentType = fileType,
                FileSize = fileContents != null ? fileContents.Length : 0,
                Contents = fileContents
            };
        }

        private async Task<string> SaveFile(UploadedFile file, Guid id)
        {
            System.IO.FileStream stream = null;
            var virtualPath = string.Empty;
            try
            {
                var fileName = id.ToString();
                var physicalPath = Server.MapPath("~" + Url.Photo(id));
                virtualPath = Url.Photo(id);
                Directory.CreateDirectory(Path.GetDirectoryName(physicalPath));

                //fileName += System.IO.Path.GetExtension(file.Filename);
                //var path = System.IO.Path.Combine(physicalPath, fileName);
                //virtualPath = virtualPath + fileName;
                stream = new System.IO.FileStream(physicalPath, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                if (stream.CanWrite)
                {
                    await stream.WriteAsync(file.Contents, 0, file.Contents.Length);
                }
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
            return virtualPath;
        }

	}
}