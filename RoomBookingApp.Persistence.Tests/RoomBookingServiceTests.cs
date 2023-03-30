using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RoomBookingApp.Core.DataServices;
using RoomBookingApp.Domain;
using RoomBookingApp.Persistence.Repositories;
using Shouldly;

namespace RoomBookingApp.Persistence.Tests
{
    public class RoomBookingServiceTests
    {

        [Fact]
        public void Should_Return_Available_Rooms()
        {
            //Arrange
            var date = new DateTime(2023, 03, 29);

            //you create your db options, in this case inmemory, but this could be sql, or sqllite, or choose database
            var dbOptions = new DbContextOptionsBuilder<RoomBookingAppDbContext>()
                .UseInMemoryDatabase("AvailableRoomTest")
                .Options;

            using var context = new RoomBookingAppDbContext(dbOptions);
            
            context.Add(new Room { Id = 1, Name = "Room 1" });
            context.Add(new Room { Id = 2, Name = "Room 2" });
            context.Add(new Room { Id = 3, Name = "Room 3" });

            context.Add(new RoomBooking {  RoomId = 1, Date = date, FullName = "John Doe", Email = "ddd@gmail.com" });
            context.Add(new RoomBooking { RoomId = 2, Date = date.AddDays(-1) , FullName = "Jane Doe", Email = "aaa@gmail.com" });

            context.SaveChanges();

            var roomBookingService = new RoomBookingService(context);

            //Act
            var availableRooms = roomBookingService.GetAvailableRooms(date);

            //Assert
            availableRooms.Count().ShouldBe(2);
            availableRooms.ShouldContain(q => q.Id == 2);
            availableRooms.ShouldContain(q => q.Id == 3);
            availableRooms.ShouldNotContain(q => q.Id == 1);
        }

        [Fact]
        public void Should_Save_Room_Booking()
        {
            var dbOptions = new DbContextOptionsBuilder<RoomBookingAppDbContext>()
            .UseInMemoryDatabase("AvailableRoomTest")
             .Options;

            var roomBooking = new RoomBooking { RoomId = 1, Date = new DateTime(2021, 06, 09), FullName = "John Doe", Email = "ddd@gmail.com" };

            using var context = new RoomBookingAppDbContext(dbOptions);
            var roomBookingService = new RoomBookingService(context);
            //Act
            roomBookingService.Save(roomBooking);

            //Assert
            var bookings = context.RoomBookings.ToList();
            var booking = Assert.Single(bookings);

            Assert.Equal(roomBooking.Date, booking.Date);
            Assert.Equal(roomBooking.RoomId, booking.RoomId);
        }

    }
}
