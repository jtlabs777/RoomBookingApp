using RoomBookingApp.Core.DataServices;
using RoomBookingApp.Core.Models;
using RoomBookingApp.Domain;
using RoomBookingApp.Domain.BaseModels;

namespace RoomBookingApp.Core.Processors
{
    public class RoomBookingRequestProcessor : IRoomBookingRequestProcessor
    {
        private readonly IRoomBookingService _roomBookingService;

        public RoomBookingRequestProcessor(IRoomBookingService roomBookingService)
        {
            this._roomBookingService = roomBookingService;
        }

        public RoomBookingResult BookRoom(RoomBookingRequest bookingRequest)
        {

            if (bookingRequest == null)
            {
                throw new ArgumentNullException(nameof(bookingRequest));
            }

            var availableRooms = _roomBookingService.GetAvailableRooms(bookingRequest.Date);
            var result = CreateRoomBookingObject<RoomBookingResult>(bookingRequest);

            if (availableRooms.Any())
            {
                var room = availableRooms.First();
                var roomBooking = CreateRoomBookingObject<RoomBooking>(bookingRequest);
                result.RoomBookingId = room.Id;
                _roomBookingService.Save(roomBooking);
                result.Flag = BookingResultFlag.Successful;
            }
            else
            {
                result.Flag = BookingResultFlag.NotSuccessful;
            }


            return result;

        }



        private TRoomBooking CreateRoomBookingObject<TRoomBooking>(RoomBookingRequest bookingRequest) where TRoomBooking : RoomBookingBase,
        new()
        {
            return new TRoomBooking
            {
                FullName = bookingRequest.FullName,
                Date = bookingRequest.Date,
                Email = bookingRequest.Email

            };

        }
    }


}