using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Relnet.Simulation
{
    public class RelnetWorld
    {
        public List<Node> Nodes { get; private set; }
        public List<Relationship> Relationships { get; private set; }
        public List<State> States { get; private set; }

        public RelnetWorld(List<Node> nodes, List<State> states)
        {
            Nodes = nodes;
            States = states;
            Relationships = new List<Relationship>();
            BuildWorld();
        }

        private void BuildWorld()
        {
            var random = new Random((int)DateTime.Now.Ticks);
            foreach (var node1 in Nodes)
            {
                foreach (var node2 in Nodes)
                {
                    if (node1 == node2) continue;
                    if (node2.Relationships.ContainsKey(node1)) continue;
                    var rel = new Relationship(node1, node2, States[random.Next(0, States.Count)]);
                    node1.Relationships.Add(node2, rel);
                    node2.Relationships.Add(node1, rel);
                    Relationships.Add(rel);
                }
            }
        }

        public void Step()
        {

        }
    }
}
