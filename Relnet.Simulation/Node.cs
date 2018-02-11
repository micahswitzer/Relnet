using System.Collections.Generic;

namespace Relnet.Simulation
{
    public class Node
    {
        public string Name { get; set; }
        public Dictionary<Node, Relationship> Relationships { get; internal set; }

        public Node()
        {
            Relationships = new Dictionary<Node, Relationship>();
        }
    }
}
