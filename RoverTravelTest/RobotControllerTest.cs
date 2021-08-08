using RoverProcess.Abstract;
using RoverProcess.Concrete;
using RoverProcess.Constants;
using RoverProcess.Objects;
using Xunit;

namespace RoverTravelTest
{
    public class RobotControllerTest
    {
        IRoverController GetRoverController() { return new RobotController(new Mars()); }

        #region Plateau Testing
        [Fact]
        public void IsPlateauReady_AnyValues_ReturnErrorResult()
        {
            IRoverController roverController = GetRoverController();

            Assert.False(roverController.IsPlateauReady);
        }
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void IsPlateauReady_WithNone_ReturnErrorResult(string boundries)
        {
            IRoverController roverController = GetRoverController();
            var result = roverController.SetPlanetArea(boundries);
            Assert.False(result.Success);
            Assert.Equal(result.Message, Messages.BoundriesNotCorrectFormat);
            Assert.False(roverController.IsPlateauReady);
        }
        [Fact]
        public void IsPlateauReady_WithLetter_ReturnErrorResult()
        {
            IRoverController roverController = GetRoverController();
            var result = roverController.SetPlanetArea("5 Y");
            Assert.False(result.Success);
            Assert.Equal(result.Message, Messages.BoundriesNotCorrectFormat);
            Assert.False(roverController.IsPlateauReady);
        }
        [Fact]
        public void IsPlateauReady_OnlyOneNumber_ReturnErrorResult()
        {
            IRoverController roverController = GetRoverController();
            var result = roverController.SetPlanetArea("54");
            Assert.False(result.Success);
            Assert.Equal(result.Message, Messages.BoundriesNotCorrectFormat);
            Assert.False(roverController.IsPlateauReady);
        }
        [Fact]
        public void IsPlateauReady_OnlyOneNumberWithSpace_ReturnErrorResult()
        {
            IRoverController roverController = GetRoverController();
            var result = roverController.SetPlanetArea("5 ");
            Assert.False(result.Success);
            Assert.Equal(result.Message, Messages.BoundriesNotCorrectFormat);
            Assert.False(roverController.IsPlateauReady);
        }
        [Fact]
        public void IsPlateauReady_WithMoreThanTwoNumbers_ReturnErrorResult()
        {
            IRoverController roverController = GetRoverController();
            var result = roverController.SetPlanetArea("5 4 3");
            Assert.False(result.Success);
            Assert.Equal(result.Message, Messages.BoundriesNotCorrectFormat);
            Assert.False(roverController.IsPlateauReady);
        }
        [Fact]
        public void IsPlateauReady_ReturnSuccesResult()
        {
            IRoverController roverController = GetRoverController();
            var result = roverController.SetPlanetArea("5 3");
            Assert.True(result.Success);
            Assert.True(roverController.IsPlateauReady);
        }
        #endregion

        #region Add Rover Testing
        [Fact]
        public void AddRover_WithNullObject_ReturnErrorResult()
        {
            IRoverController roverController = GetRoverController();
            var result = roverController.AddRover(null, "", "");
            Assert.Equal(0, roverController.Rovers.Count);
            Assert.False(result.Success);
        }
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void AddRover_WithoutLocationInfo_ReturnErrorResult(string locations)
        {
            IRoverController roverController = GetRoverController();
            Robot rover = new();
            var result = roverController.AddRover(rover, locations, "");
            Assert.Equal(0, roverController.Rovers.Count);
            Assert.False(result.Success);
            Assert.Equal(Messages.LocationsNotCorrectFormat, result.Message);
        }

        [Theory]
        [InlineData("1")]
        [InlineData("1 2")]
        [InlineData("1 2 3 4")]
        public void AddRover_WithIncorrectLocationFormat_ReturnErrorResult(string locations)
        {
            IRoverController roverController = GetRoverController();
            Robot rover = new();
            var result = roverController.AddRover(rover, locations, "");
            Assert.Equal(0, roverController.Rovers.Count);
            Assert.False(result.Success);
            Assert.Equal(Messages.LocationsNotCorrectFormat, result.Message);
        }

        [Theory]
        [InlineData("1 N 2")]
        [InlineData("N 1 2")]
        public void AddRover_PlacedIncorrectFacingDirectionInfo_ReturnErrorResult(string locations)
        {
            IRoverController roverController = GetRoverController();
            Robot rover = new();
            var result = roverController.AddRover(rover, locations, "");
            Assert.Equal(0, roverController.Rovers.Count);
            Assert.False(result.Success);
            Assert.Equal(Messages.LocationsNotCorrectFormat, result.Message);
        }

        [Theory]
        [InlineData("1 2 T")]
        [InlineData("1 2 Z")]
        [InlineData("1 2 A")]
        [InlineData("1 2 3")]
        [InlineData("1 2 /")]
        [InlineData("1 2 *")]
        public void AddRover_WithIncorrectFacingDirectionInfo_ReturnErrorResult(string locations)
        {
            IRoverController roverController = GetRoverController();
            Robot rover = new();
            var result = roverController.AddRover(rover, locations, "");
            Assert.Equal(0, roverController.Rovers.Count);
            Assert.False(result.Success);
            Assert.Equal(Messages.LocationsNotCorrectFormat, result.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void AddRover_WithoutCommandInfo_ReturnErrorResult(string commands)
        {
            IRoverController roverController = GetRoverController();
            Robot rover = new();
            var result = roverController.AddRover(rover, "1 2 N", commands);
            Assert.Equal(0, roverController.Rovers.Count);
            Assert.False(result.Success);
            Assert.Equal(Messages.CommandsNotCorrectFormat, result.Message);
        }

        [Theory]
        [InlineData("LMR ")]
        [InlineData(" LMR")]
        [InlineData("L MR")]
        public void AddRover_CommandsWithSpace_ReturnErrorResult(string commands)
        {
            IRoverController roverController = GetRoverController();
            Robot rover = new();
            var result = roverController.AddRover(rover, "1 2 N", commands);
            Assert.Equal(0, roverController.Rovers.Count);
            Assert.False(result.Success);
            Assert.Equal(Messages.CommandsNotCorrectFormat, result.Message);
        }

        [Theory]
        [InlineData("LMRT")]
        [InlineData("ALMR")]
        [InlineData("LDMR")]
        public void AddRover_WithIncorrectCommandLetter_ReturnErrorResult(string commands)
        {
            IRoverController roverController = GetRoverController();
            Robot rover = new();
            var result = roverController.AddRover(rover, "1 2 N", commands);
            Assert.Equal(0, roverController.Rovers.Count);
            Assert.False(result.Success);
            Assert.Equal(Messages.CommandsNotCorrectFormat, result.Message);
        }
        [Theory]
        [InlineData("LMR1")]
        [InlineData("2LMR")]
        [InlineData("L3MR")]
        public void AddRover_IncludingNumberInCommands_ReturnErrorResult(string commands)
        {
            IRoverController roverController = GetRoverController();
            Robot rover = new();
            var result = roverController.AddRover(rover, "1 2 N", commands);
            Assert.Equal(0, roverController.Rovers.Count);
            Assert.False(result.Success);
            Assert.Equal(Messages.CommandsNotCorrectFormat, result.Message);
        }
        [Theory]
        [InlineData("LMR*")]
        [InlineData("-LMR")]
        [InlineData("L/MR")]
        [InlineData("LM+R")]
        public void AddRover_IncludingSignsInCommands_ReturnErrorResult(string commands)
        {
            IRoverController roverController = GetRoverController();
            Robot rover = new();
            var result = roverController.AddRover(rover, "1 2 N", commands);
            Assert.Equal(0, roverController.Rovers.Count);
            Assert.False(result.Success);
            Assert.Equal(Messages.CommandsNotCorrectFormat, result.Message);
        }
        [Theory]
        [InlineData("lMR")]
        [InlineData("LmR")]
        [InlineData("LMr")]
        public void AddRover_CorrectCommandsWihtLowerCase_ReturnErrorResult(string commands)
        {
            IRoverController roverController = GetRoverController();
            Robot rover = new();
            var result = roverController.AddRover(rover, "1 2 N", commands);
            Assert.Equal(0, roverController.Rovers.Count);
            Assert.False(result.Success);
            Assert.Equal(Messages.CommandsNotCorrectFormat, result.Message);
        }
        [Fact]
        public void AddRover_AddOne_ReturnSuccessResult()
        {
            IRoverController roverController = GetRoverController();
            Robot rover = new();
            var result = roverController.AddRover(rover, "1 2 N", "LMR");

            Assert.Equal(1, rover.Location.X);
            Assert.Equal(2, rover.Location.Y);
            Assert.Equal(Directions.N, rover.Direction.Facing);
            Assert.Equal(1, roverController.Rovers.Count);
            Assert.True(result.Success);
            Assert.Equal(rover, result.Data);
        }
        [Fact]
        public void AddRover_AddTwo_ReturnSuccessResult()
        {
            IRoverController roverController = GetRoverController();
            Robot rover1 = new();
            Robot rover2 = new();

            roverController.AddRover(rover1, "1 2 N", "LMR");
            var result = roverController.AddRover(rover2, "2 3 W", "RML");

            Assert.Equal(rover1, roverController.Rovers[0]);
            Assert.Equal(rover2, roverController.Rovers[1]);
            Assert.Equal(2, roverController.Rovers.Count);
            Assert.True(result.Success);
            Assert.Equal(rover2, result.Data);
        }
        #endregion

        #region Move Testing
        [Fact]
        public void Move_WithNullRover_ReturnErrorResult()
        {
            IRoverController roverController = GetRoverController();
            var result = roverController.Move(default(IRover));

            Assert.False(result.Success);
            Assert.Equal(Messages.NoRover, result.Message);
        }
        [Fact]
        public void Move_WithoutRoverLocationInfo_ReturnErrorResult()
        {
            IRoverController roverController = GetRoverController();
            var result = roverController.Move(new Robot());

            Assert.False(result.Success);
            Assert.Equal(Messages.RoverLocationNotDefined, result.Message);
        }
        [Fact]
        public void Move_WithoutRoverDirectionInfo_ReturnErrorResult()
        {
            IRoverController roverController = GetRoverController();
            var result = roverController.Move(new Robot() { Location = new Location { X = 1, Y = 2 } });
            Assert.False(result.Success);
            Assert.Equal(Messages.RoverDirectionNotDefined, result.Message);
        }
        [Fact]
        public void Move_WithoutPlateauInfo_ReturnErrorResult()
        {
            IRoverController roverController = GetRoverController();
            var result = roverController.Move(new Robot()
            {
                Location = new Location { X = 1, Y = 2 },
                Direction = new Compass.DirectionNode { Facing = Directions.N }
            });
            Assert.False(result.Success);
            Assert.Equal(Messages.PlateauNotReady, result.Message);
        }
      
        [Theory]
        [InlineData(4, 5, Directions.N, 4, 5)]
        [InlineData(4, 4, Directions.N, 4, 5)]
        [InlineData(5, 4, Directions.E, 5, 4)]
        [InlineData(4, 4, Directions.E, 5, 4)]
        [InlineData(1, 0, Directions.S, 1, 0)]
        [InlineData(1, 1, Directions.S, 1, 0)]
        [InlineData(0, 1, Directions.W, 0, 1)]
        [InlineData(1, 1, Directions.W, 0, 1)]
        public void Move_ReturnSuccessResult(int currentX, int currentY, Directions facingDirection, int expectedX, int expectedY)
        {
            IRoverController roverController = GetRoverController();
            roverController.SetPlanetArea("5 5");
            var rover = new Robot()
            {
                Location = new Location { X = currentX, Y = currentY },
                Direction = new Compass.DirectionNode { Facing = facingDirection }
            };
            var result = roverController.Move(rover);

            Assert.True(result.Success);
            Assert.Equal(expectedX, rover.Location.X);
            Assert.Equal(expectedY, rover.Location.Y);
        }
        #endregion

        #region Left Testing
        [Fact]
        public void Left_WithNullRover_ReturnErrorResult()
        {
            IRoverController roverController = GetRoverController();
            var result = roverController.Left(default(IRover));

            Assert.False(result.Success);
            Assert.Equal(Messages.NoRover, result.Message);
        }
        [Fact]
        public void Left_WithoutRoverDirectionInfo_ReturnErrorResult()
        {
            IRoverController roverController = GetRoverController();
            var result = roverController.Left(new Robot());

            Assert.False(result.Success);
            Assert.Equal(Messages.RoverDirectionNotDefined, result.Message);
        }

        [Theory]
        [InlineData(Directions.N, Directions.W)]
        [InlineData(Directions.W, Directions.S)]
        [InlineData(Directions.S, Directions.E)]
        [InlineData(Directions.E, Directions.N)]
        public void Left_ReturnSuccessResult(Directions currentDirection, Directions expected)
        {
            IRoverController roverController = GetRoverController();
            var rover = new Robot()
            {
                Direction = new Compass.DirectionNode { Facing = currentDirection }
            };
            var result = roverController.Left(rover);
            Assert.True(result.Success);
            Assert.Equal(expected, rover.Direction.Facing);
        }
        #endregion

        #region Right Testing
        [Fact]
        public void Right_WithNullRover_ReturnErrorResult()
        {
            IRoverController roverController = GetRoverController();
            var result = roverController.Right(default(IRover));

            Assert.False(result.Success);
            Assert.Equal(Messages.NoRover, result.Message);
        }
        [Fact]
        public void Right_WithoutRoverDirectionInfo_ReturnErrorResult()
        {
            IRoverController roverController = GetRoverController();
            var result = roverController.Right(new Robot());

            Assert.False(result.Success);
            Assert.Equal(Messages.RoverDirectionNotDefined, result.Message);
        }

        [Theory]
        [InlineData(Directions.N, Directions.E)]
        [InlineData(Directions.E, Directions.S)]
        [InlineData(Directions.S, Directions.W)]
        [InlineData(Directions.W, Directions.N)]
        public void Right_ReturnSuccessResult(Directions currentDirection, Directions expected)
        {
            IRoverController roverController = GetRoverController();
            var rover = new Robot()
            {
                Direction = new Compass.DirectionNode { Facing = currentDirection }
            };
            var result = roverController.Right(rover);
            Assert.True(result.Success);
            Assert.Equal(expected, rover.Direction.Facing);
        }
        #endregion

        #region Executed Commands Testing
        [Fact]
        public void ExecuteCommands_WithoutPlateauInfo_ReturnErrorResult()
        {
            IRoverController roverController = GetRoverController();
            var result = roverController.ExecuteCommands();

            Assert.False(result.Success);
            Assert.Equal(Messages.PlateauNotReady, result.Message);
        }

        [Fact]
        public void ExecuteCommands_WithoutRover_ReturnErrorResult()
        {
            IRoverController roverController = GetRoverController();
            roverController.SetPlanetArea("5 5");
            var result = roverController.ExecuteCommands();

            Assert.False(result.Success);
            Assert.Equal(Messages.NoRover, result.Message);
        }

        [Fact]
        public void ExecuteCommands_ReturnSuccessResult()
        {
            IRoverController roverController = GetRoverController();
            roverController.SetPlanetArea("5 5");
            var rover = new Robot();
            roverController.AddRover(rover, "1 2 N", "LMLMLMLMM");
            var result = roverController.ExecuteCommands();

            Assert.True(result.Success);
            Assert.Equal(1, rover.Location.X);
            Assert.Equal(3, rover.Location.Y);
            Assert.Equal(Directions.N, rover.Direction.Facing);
        }
        #endregion
    }
}
