using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using RoomBookingApp.Persistence;

namespace RoomBookingApp.Api
{
    public class RoomBookingContextFactory : IDesignTimeDbContextFactory<RoomBookingAppDbContext>
    {
        public RoomBookingAppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<RoomBookingAppDbContext>();
            optionsBuilder.UseSqlite("Data Source=:memory:");
            return new RoomBookingAppDbContext(optionsBuilder.Options);
        }
    }
}
