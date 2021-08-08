using RoverProcess.Abstract;
using RoverProcess.Objects;

namespace RoverProcess.Concrete
{
    public class Mars : IPlanet
    {
        public Plateau Plateau { get; set; }
    }
}
