using RoverProcess.Abstract;
using RoverProcess.Constants;
using RoverProcess.Extention;
using RoverProcess.Objects;
using RoverProcess.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace RoverProcess.Concrete
{
    public class RobotController : IRoverController
    {
        private IList<IRover> _rovers;
        private IDictionary<IRover, IList<Func<IResult>>> _commandList;
        private readonly IPlanet _planet;
        private Compass _compass;
        private Directions[] directions = new Directions[] { Directions.N, Directions.E, Directions.S, Directions.W };
        private Commands[] _commands = new Commands[] { Commands.L, Commands.R, Commands.M };

        public IList<IRover> Rovers => _rovers;
        public RobotController(IPlanet planet)
        {
            _planet = planet;
            _rovers = new List<IRover>();
            _commandList = new Dictionary<IRover, IList<Func<IResult>>>();
            PrepareCompass();
        }
        private void PrepareCompass()
        {
            _compass = new();
            foreach (var direction in directions)
            {
                _compass.InsertDirection(direction);
            }
        }
        public IResult SetPlanetArea(string boundries)
        {
            if (boundries.IsNUllOrWhiteSpace())
                return new ErrorResult(Messages.BoundriesNotCorrectFormat);

            var boundryArray = boundries.Split(' ');
            if (boundryArray.Length != 2)
                return new ErrorResult(Messages.BoundriesNotCorrectFormat);

            if (!int.TryParse(boundryArray[0], out _) || !int.TryParse(boundryArray[1], out _))
                return new ErrorResult(Messages.BoundriesNotCorrectFormat);

            _planet.Plateau = new Plateau
            {
                BoundryCoordinates = new Boundry { Bottom = 0, Left = 0, Top = int.Parse(boundryArray[0]), Right = int.Parse(boundryArray[0]) }
            };

            return new SuccessResult();
        }

        public IDataResult<IRover> AddRover(IRover rover, string location, string commands)
        {
            if (rover is null)
                return new ErrorDataResult<IRover>(Messages.NoRover);

            if (location.IsNUllOrWhiteSpace())
               return new ErrorDataResult<IRover>(Messages.LocationsNotCorrectFormat);
            var locationParts = location.Split(' ');

            if (locationParts.Length != 3)
                return new ErrorDataResult<IRover>(Messages.LocationsNotCorrectFormat);

            if (!int.TryParse(locationParts[0], out int x) || !int.TryParse(locationParts[1], out int y))
               return new ErrorDataResult<IRover>(Messages.LocationsNotCorrectFormat);

            var stringArrayOfDirections = directions.Select(s => s.ToString()).ToArray();
            if (!stringArrayOfDirections.Contains(locationParts[2]))
               return new ErrorDataResult<IRover>(Messages.LocationsNotCorrectFormat);

            if (commands.IsNUllOrWhiteSpace())
                return new ErrorDataResult<IRover>(Messages.CommandsNotCorrectFormat);

            var stringArrayOfCommands = _commands.Select(s => s.ToString()).ToArray();
            if (commands.ToCharArray().Any(w => !stringArrayOfCommands.Contains(w.ToString())))
                return new ErrorDataResult<IRover>(Messages.CommandsNotCorrectFormat);

            rover.Location = new Location { X = x, Y = y };

            Enum.TryParse(locationParts[2], out Directions direction);
            rover.Direction = _compass.GetDirection(direction);

            _rovers.Add(rover);

            IList<Func<IResult>> actionList = new List<Func<IResult>>();
            foreach (var command in commands)
            {
                if (command.Equals('L'))
                {
                    actionList.Add(() =>
                    {
                        return Left(rover);
                    });
                }
                else if (command.Equals('R'))
                {
                    actionList.Add(() =>
                    {
                        return Right(rover);
                    });
                }
                else if (command.Equals('M'))
                {
                    actionList.Add(() =>
                    {
                        return Move(rover);
                    });
                }
            }

            _commandList[rover] = actionList;

            return new SuccessDataResult<IRover>(rover);
        }

        public IResult Move(IRover rover)
        {
            if (rover is null)
                return new ErrorDataResult<IRover>(Messages.NoRover);

            if (rover.Location is null)
                return new ErrorDataResult<IRover>(Messages.RoverLocationNotDefined);

            if (rover.Direction is null)
                return new ErrorDataResult<IRover>(Messages.RoverDirectionNotDefined);

            if (!IsPlateauReady)
                return new ErrorResult(Messages.PlateauNotReady);

            switch (rover.Direction.Facing)
            {
                case Directions.N:
                    if (rover.Location.Y < _planet.Plateau.BoundryCoordinates.Top)
                    {
                        rover.Location.Y++;
                        Thread.Sleep(2000);
                    }
                    break;
                case Directions.E:
                    if (rover.Location.X < _planet.Plateau.BoundryCoordinates.Right)
                    {
                        rover.Location.X++;
                        Thread.Sleep(2000);
                    }
                    break;
                case Directions.S:
                    if (rover.Location.Y > _planet.Plateau.BoundryCoordinates.Bottom)
                    {
                        rover.Location.Y--;
                        Thread.Sleep(2000);
                    }
                    break;
                case Directions.W:
                    if (rover.Location.X > _planet.Plateau.BoundryCoordinates.Left)
                    {
                        rover.Location.X--;
                        Thread.Sleep(2000);
                    }
                    break;
            }

            return new SuccessResult();
        }
        public IResult Left(IRover rover)
        {
            if (rover is null)
                return new ErrorDataResult<IRover>(Messages.NoRover);

            if (rover.Direction is null)
                return new ErrorDataResult<IRover>(Messages.RoverDirectionNotDefined);

            rover.Direction = rover.Direction.Prev ?? _compass.GetDirection(rover.Direction.Facing).Prev;
            Thread.Sleep(1000);
            return new SuccessResult();
        }
        public IResult Right(IRover rover)
        {
            if (rover is null)
                return new ErrorDataResult<IRover>(Messages.NoRover);

            if (rover.Direction is null)
                return new ErrorDataResult<IRover>(Messages.RoverDirectionNotDefined);

            rover.Direction = rover.Direction.Next ?? _compass.GetDirection(rover.Direction.Facing).Next;
            Thread.Sleep(1000); 
            return new SuccessResult();
        }

        public IResult ExecuteCommands()
        {
            if (!IsPlateauReady)
                return new ErrorResult(Messages.PlateauNotReady);

            if(_rovers.Count < 1)
                return new ErrorResult(Messages.NoRover);

            foreach (var rover in _rovers)
            {
                var actionList = _commandList[rover];
                foreach (var action in actionList)
                {
                    var result = action();
                    if (!result.Success)
                        return result;
                }
            }
            return new SuccessResult();
        }

        public bool IsPlateauReady => _planet.Plateau != null;


    }


}
