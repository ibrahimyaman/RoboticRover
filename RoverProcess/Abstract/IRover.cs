using RoverProcess.Objects;

namespace RoverProcess.Abstract
{
    public interface IRover
    {
        Location Location { get; set; }
        Compass.DirectionNode Direction { get; set; }
    }
}
