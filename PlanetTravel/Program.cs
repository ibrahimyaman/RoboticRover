using RoverProcess.Abstract;
using RoverProcess.Concrete;
using RoverProcess.Utilities.Results;
using System;

namespace PlanetTravel
{
    class Program
    {
        static IRoverController _roverController;
        static IPlanet _planet;
        static void Main(string[] args)
        {
            _planet = new Mars();
            _roverController = new RobotController(_planet);
            IResult result;
            string input1;
            string input2;
            while(true){
                Console.Write("Type plateau boundries (1 2) : ");
                input1 = Console.ReadLine();
                result = _roverController.SetPlanetArea(input1);
                Console.WriteLine();
                if (result.Success)
                    break;
                else
                    Console.WriteLine(result.Message);
            }

            while (true)
            {
                Console.Write("Type first rover's location coordinates and facing direction (1 2 N) : ");
                input1 = Console.ReadLine();
                Console.Write("Type commands to move rover direction (MLMRM) : ");
                input2 = Console.ReadLine();
                result = _roverController.AddRover(new Robot(), input1, input2);
                Console.WriteLine();
                if (result.Success)
                    break;
                else
                    Console.WriteLine(result.Message);
            }


            while (true)
            {
                Console.Write("Type second rover's location coordinates and facing direction (1 2 N) : ");
                input1 = Console.ReadLine();
                Console.Write("Type commands to move rover direction (MLMRM) : ");
                input2 = Console.ReadLine();
                result = _roverController.AddRover(new Robot(), input1, input2);
                Console.WriteLine();
                if (result.Success)
                    break;
                else
                    Console.WriteLine(result.Message);
            }

            Console.WriteLine("Rovers have started to move, please wait to see last location");

            _roverController.ExecuteCommands();

            Console.WriteLine("Rovers have finished to move");
            Console.WriteLine();
            Console.WriteLine("Rovers' last locations are :");
            foreach (var rover in _roverController.Rovers)
            {
                Console.WriteLine($"{rover.Location.X} {rover.Location.Y} {rover.Direction.Facing}");
            }
            Console.ReadLine();
        }
    }
}
