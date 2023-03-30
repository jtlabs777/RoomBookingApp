using Moq;
using RoomBookingApp.Core.DataServices;
using RoomBookingApp.Core.Models;
using RoomBookingApp.Core.Processors;
using RoomBookingApp.Domain;
using Shouldly;


namespace RoomBookingApp.Core
{
    public class RoomBookingRequestProcessorTest
    {
        private RoomBookingRequestProcessor _processor;
        private RoomBookingRequest _request;
        private Mock<IRoomBookingService> _roomBookingServiceMock;
        private List<Room> _availableRooms;
        public RoomBookingRequestProcessorTest()
        {

            _request = new RoomBookingRequest
            {
                FullName = "Test Name",
                Email = "test@request.com",
                Date = new DateTime(2023, 10, 20)
            };
            _availableRooms = new List<Room>()
            {
                new Room()
                {
                     Id = 1
                }

            };
            _roomBookingServiceMock = new Mock<IRoomBookingService>();
            _roomBookingServiceMock.Setup(q => q.GetAvailableRooms(_request.Date)).Returns(_availableRooms);
            _processor = new RoomBookingRequestProcessor(_roomBookingServiceMock.Object);
        }

        [Fact]
        public void Should_Return_Room_Booking_Request_With_Request_Values()
        {




            //Act

            RoomBookingResult result = _processor.BookRoom(_request);

            //Assert

            result.ShouldNotBeNull();
            result.FullName.ShouldBe(_request.FullName);
            result.Email.ShouldBe(_request.Email);
            result.Date.ShouldBe(_request.Date);
        }

        [Fact]
        public void Should_Throw_Exception_For_Null_Request()
        {
            //arrange
            var exception = Should.Throw<ArgumentNullException>(() => _processor.BookRoom(null));
            //act

            //assert
            exception.ParamName.ShouldBe("bookingRequest");
        }

        [Fact]
        public void Should_Save_Room_Booking_Request()
        {
            RoomBooking savedBooking = null;
            _roomBookingServiceMock.Setup(x => x.Save(It.IsAny<RoomBooking>())).Callback<RoomBooking>(booking =>
            {
                savedBooking = booking;
                savedBooking.RoomId = _availableRooms.First().Id;//added as field is now nullable
            });
            _processor.BookRoom(_request);

            _roomBookingServiceMock.Verify(x => x.Save(It.IsAny<RoomBooking>()), Times.Once);

            savedBooking.ShouldNotBeNull();
            savedBooking.FullName.ShouldBe(_request.FullName);
            savedBooking.Email.ShouldBe(_request.Email);
            savedBooking.Date.ShouldBe(_request.Date);
            savedBooking.RoomId.ShouldBe(_availableRooms.First().Id); 
        }

        [Fact]
        public void Should_Not_Save_Room_Booking_Request_If_None_Available()
        {
            _availableRooms.Clear();
            _processor.BookRoom(_request);
            _roomBookingServiceMock.Verify(x => x.Save(It.IsAny<RoomBooking>()), Times.Never);

        }

        [Theory]
        [InlineData(BookingResultFlag.Successful, true)]
        [InlineData(BookingResultFlag.NotSuccessful, false)]
        public void Should_Return_Success_Flag_In_Result(BookingResultFlag bookingSuccessFlag, bool isAvailable)
        {
            if (!isAvailable)
            {
                _availableRooms.Clear();
            }
            var result = _processor.BookRoom(_request);
            result.Flag.ShouldBe(bookingSuccessFlag);

        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(null, false)]
        public void Should_Return_RoomBookingId_In_Result(int? roomBookingId, bool isAvailable)
        {
            if (!isAvailable)
            {
                _availableRooms.Clear();
            }
            else
            {
                _roomBookingServiceMock.Setup(x => x.Save(It.IsAny<RoomBooking>())).Callback<RoomBooking>(booking =>
                {
                    booking.Id = roomBookingId.Value;
                });

            }

            var result = _processor.BookRoom(_request);
            result.RoomBookingId.ShouldBe(roomBookingId);
        }

    }
}
