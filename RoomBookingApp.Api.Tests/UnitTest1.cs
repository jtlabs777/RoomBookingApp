
using Microsoft.Extensions.Logging;
using Moq;
using RoomBookingApp.Api.Controllers;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomBookingApp.Api.Tests
{
    public class UnitTest1
    {

        [Fact]
        public void Should_Return_Foracast_results()
        {
            var loggerMock = new Mock<ILogger<WeatherForecastController>>();
            var controller = new WeatherForecastController(loggerMock.Object);

            var result = controller.Get();

            result.Count().ShouldBeGreaterThan(1);
            result.ShouldNotBeNull();


        }
    }
}
