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
        public List<TriangleConfiguration> TriConfigs { get; private set; }
        public int NodeCount { get; private set; }
        private Random Random = new Random((int)DateTime.Now.Ticks);

        public RelnetWorld(List<Node> nodes, List<State> states, List<TriangleConfiguration> triConfigs)
        {
            Nodes = nodes;
            NodeCount = nodes.Count;
            States = states;
            Relationships = new List<Relationship>();
            TriConfigs = triConfigs;
            BuildWorld();
        }

        private void BuildWorld()
        {
            foreach (var node1 in Nodes)
            {
                foreach (var node2 in Nodes)
                {
                    if (node1 == node2) continue;
                    if (node2.Relationships.ContainsKey(node1)) continue;
                    var rel = new Relationship(node1, node2, States[Random.Next(0, States.Count)], States);
                    node1.Relationships.Add(node2, rel);
                    node2.Relationships.Add(node1, rel);
                    Relationships.Add(rel);
                }
            }
        }

        public void Step()
        {
            foreach (var rel in Relationships)
            {
                var node1 = rel.NodeOne;
                var node2 = rel.NodeTwo;
                var relationships = new List<Relationship>();
                foreach (var node in Nodes)
                {
                    if (node == node1) continue;
                    if (node == node2) continue;
                    relationships.Clear();
                    relationships.Add(node1.Relationships[node]);
                    relationships.Add(node2.Relationships[node]);
                    relationships.Add(rel);
                    var triConfig = GetTriConfigForRels(relationships);
                    ComputeRelProbs(triConfig, rel);
                }
            }
            ComputeStateChanges();
        }

        private void ComputeRelProbs(TriangleConfiguration config, Relationship relationship)
        {
            var relState = relationship.State;
            var weights = config.StateWeights[relState];
            foreach (var stateWeight in weights)
            {
                relationship.Weights[stateWeight.Key] += stateWeight.Value;
            }
        }

        private void ComputeStateChanges()
        {
            foreach (var rel in Relationships)
            {
                int total = rel.Stay * (NodeCount - 2);
                var tempList = new List<(State, int)>
                {
                    (rel.State, total)
                };
                foreach (var item in rel.Weights)
                {
                    if (item.Key == rel.State) continue;
                    total += item.Value;
                    tempList.Add((item.Key, total));
                }
                var val = Random.NextDouble() * total;
                var newState = tempList.First(x => val <= x.Item2).Item1;
                if (newState == rel.State) rel.Stay += 50;
                else rel.Stay = 100;
                rel.State = newState;
                foreach (var key in rel.Weights.Keys.ToList())
                    rel.Weights[key] = 0;
            }
        }

        public TriangleConfiguration GetTriConfigForRels(List<Relationship> rels)
        {
            var tempStateCount = new Dictionary<State, int>();
            foreach (var rel in rels)
            {
                if (!tempStateCount.ContainsKey(rel.State))
                {
                    tempStateCount.Add(rel.State, 0);
                }
                tempStateCount[rel.State]++;
            }
            foreach (var config in TriConfigs)
            {
                var valid = true;
                foreach (var state in tempStateCount)
                {
                    if (!config.StateCounts.ContainsKey(state.Key))
                    {
                        valid = false;
                        break;
                    }
                    if (config.StateCounts[state.Key] != state.Value)
                    {
                        valid = false;
                        break;
                    }
                }
                if (valid) return config;
            }
            return null;
        }
    }
}
