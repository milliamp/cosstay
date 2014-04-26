using CosStay.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosStay.Core.Services.Impl
{
    public class AccomodationVenueService : IAccomodationVenueService
    {
        private IEntityStore _es;
        public AccomodationVenueService(IEntityStore entityStore)
        {
            _es = entityStore;
        }
        public async Task<Model.AccomodationVenue> UpdateVenueAsync(Model.AccomodationVenue dtoInstance)
        {
            AccomodationVenue instance = await _es.GetAsync<AccomodationVenue>(dtoInstance.Id);
            /*var priorRooms = _es.Get<AccomodationVenue>(venue.AccomodationVenueId).Rooms;

            var nope = new List<AccomodationRoom>();
            foreach (var room in venue.Rooms)
            {
                if (room.AccomodationRoomId == 0)
                    continue;
                if (priorRooms.Any(r => r.AccomodationRoomId == room.AccomodationRoomId))
                    continue;

                nope.Add(room);
            }
            
            foreach (var room in nope)
            {
                venue.Rooms.RemoveAll(r => r.AccomodationRoomId == room.AccomodationRoomId);
            }*/

            //_es.Update(venue);

            //_es.AttachStub(venue);
            // Bed Size and Bed Type are Automapped as Stub entities, they need attaching

            // TODO: Do we really have to this whole pile of crap manually!?!?!
            // we have automapper right!?
            instance.Address = dtoInstance.Address;
            instance.PublicAddress = dtoInstance.PublicAddress;
            instance.AllowsBedSharing = dtoInstance.AllowsBedSharing;
            instance.AllowsMixedRooms = dtoInstance.AllowsMixedRooms;
            instance.Features = dtoInstance.Features;
            instance.Name = dtoInstance.Name;
            foreach (var room in dtoInstance.Rooms)
            {
                bool bedNeedsAdding = false;
                AccomodationRoom roomInstance = room;
                if (room.Id != 0)
                {
                    roomInstance = instance.Rooms.FirstOrDefault(r => r.Id == room.Id);
                    roomInstance.Name = room.Name;
                    roomInstance.IsDeleted = room.IsDeleted;
                    bedNeedsAdding = true;
                   
                }
                else
                {
                    instance.Rooms.Add(room);
                }

                foreach (var bed in room.Beds)
                {
                    Bed bedInstance = bed;
                    if (bed.Id != 0)
                    {
                        bedInstance = roomInstance.Beds.FirstOrDefault(b => b.Id == bed.Id);
                        bedInstance.IsDeleted = bed.IsDeleted;
                    }
                    else if (bedNeedsAdding)
                    {
                        roomInstance.Beds.Add(bedInstance);
                    }

                    //TODO: Can we use Stub Entities here?
                    bedInstance.BedSize = await _es.GetAsync<BedSize>(bed.BedSize.Id);
                    bedInstance.BedType = await _es.GetAsync<BedType>(bed.BedType.Id);
                }
            }

            /*foreach (var thisRoom in dtoInstance.Rooms)
            {
                foreach (var bed in thisRoom.Beds)
                {
                    //TODO: Can we use Stub Entities here?
                    if (bed.BedSize != null)
                        bed.BedSize = _es.Get<BedSize>(bed.BedSize.Id);
                    if (bed.BedType != null)
                        bed.BedType = _es.Get<BedType>(bed.BedType.Id);
                }
            }*/

            /*foreach (var room in dtoInstance.Rooms)
                _es.Update(room, v => v.Beds);
            _es.Update(dtoInstance, v => v.Rooms);*/
            await _es.SaveAsync();
            return dtoInstance;
        }
    }
}
