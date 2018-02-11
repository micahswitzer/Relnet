namespace Relnet.Simulation
{
    public class Relationship
    {
        public Node NodeOne { get; private set; }
        public Node NodeTwo { get; private set; }
        public State State { get; internal set; }
        public int Stay { get; internal set; } = 0;

        public Relationship(Node node1, Node node2, State initialState)
        {
            NodeOne = node1;
            NodeTwo = node2;
            State = initialState;
        }

        public Node GetOther(Node node)
        {
            return (node == NodeOne) ? NodeTwo : (node == NodeTwo) ? NodeOne : null;
        }
    }
}
