using RoverProcess.Abstract;
using RoverProcess.Objects;

namespace RoverProcess.Concrete
{
    public class Robot : IRover
    {
        public Location Location { get; set; }
        public Compass.DirectionNode Direction { get; set; }
    }
}
