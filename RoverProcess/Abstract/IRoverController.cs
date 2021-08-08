using RoverProcess.Utilities.Results;
using System.Collections.Generic;

namespace RoverProcess.Abstract
{
    public interface IRoverController
    {
        IList<IRover> Rovers { get; }
        IResult SetPlanetArea(string boundries);
        IDataResult<IRover> AddRover(IRover rover, string location, string commands);
        IResult ExecuteCommands();
        bool IsPlateauReady { get; }
        IResult Move(IRover rover);
        IResult Left(IRover rover);
        IResult Right(IRover rover);
    }
}
