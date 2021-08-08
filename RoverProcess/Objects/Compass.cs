namespace RoverProcess.Objects
{
    public class Compass
    {
        private DirectionNode startDirection;
        public void InsertDirection(Directions direction)
        {
            DirectionNode newDirection = new() { Facing = direction };
            if (startDirection is null)
            {
                startDirection = newDirection;
                startDirection.Next = startDirection.Prev = startDirection;
                return;
            }

            DirectionNode last = startDirection.Prev;

            newDirection.Next = startDirection;

            startDirection.Prev = newDirection;

            newDirection.Prev = last;

            last.Next = newDirection;
        }
        public DirectionNode GetDirection(Directions direction)
        {
            DirectionNode directionNode = startDirection;
            do
            {
                if (directionNode.Facing.Equals(direction))
                {
                    return directionNode;
                }

                directionNode = directionNode.Next;
            }
            while (directionNode != startDirection);
            

            return null;
        }

        public class DirectionNode
        {
            public Directions Facing { get; set; }
            public DirectionNode Prev { get; set; }
            public DirectionNode Next { get; set; }
        }
    }
}
