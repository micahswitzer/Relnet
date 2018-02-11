using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Relnet.Simulation
{
    [DebuggerDisplay("State: {State}, Stay: {Stay}")]
    public class Relationship
    {
        public Node NodeOne { get; private set; }
        public Node NodeTwo { get; private set; }
        public State State { get; internal set; }
        public int Stay { get; internal set; } = 100;
        public Dictionary<State, int> Weights { get; private set; }

        public Relationship(Node node1, Node node2, State initialState, IEnumerable<State> states)
        {
            NodeOne = node1;
            NodeTwo = node2;
            State = initialState;
            Weights = states.ToDictionary(x => x, x => 0);
        }

        public Node GetOther(Node node)
        {
            return (node == NodeOne) ? NodeTwo : (node == NodeTwo) ? NodeOne : null;
        }
    }
}
