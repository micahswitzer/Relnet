using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Relnet.Simulation
{
    public class TriangleConfiguration
    {
        public Dictionary<State, Dictionary<State, int>> StateWeights { get; private set; }
        public Dictionary<State, int> StateCounts { get; private set; }

        public TriangleConfiguration(IEnumerable<State> states)
        {
            StateWeights = new Dictionary<State, Dictionary<State, int>>();
            StateCounts = new Dictionary<State, int>();

            foreach (var state in states)
            {
                StateWeights.Add(state, states.ToDictionary(x => x, x => 0));
                StateCounts.Add(state, 0);
            }
        }
    }
}
