using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using RoomBookingApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RoomBookingApp.Persistence
{
    public  class RoomBookingAppDbContext : DbContext
    {

        public RoomBookingAppDbContext(DbContextOptions<RoomBookingAppDbContext> options) : base(options)
        {
            
        }


        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomBooking> RoomBookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); //this line of code is just good practice for ovverrides in case any logic exist in the baseClass that needs to be executed. Current logic is empy for version 7.x -- does nothing


         modelBuilder.Entity<Room>().HasData(
             new Room { Id = 1, Name = "Conference Room A" },
             new Room { Id = 2, Name = "Conference Room B" },
             new Room { Id = 3, Name = "Conference Room C" }
             );
            
            
        }
    }
}
