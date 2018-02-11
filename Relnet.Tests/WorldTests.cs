using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Relnet.Simulation;

namespace Relnet.Tests
{
    [TestClass]
    public class WorldTests
    {
        [TestMethod]
        public void BuildWorld()
        {
            const int NUM_NODES = 6;
            const int NUM_STATES = 2;
            var nodes = new List<Node>();
            for (int i = 1; i <= NUM_NODES; i++)
            {
                nodes.Add(new Node { Name = $"N{i}" });
            }
            var states = new List<State>();
            for (int i = 1; i <= NUM_STATES; i++)
            {
                states.Add(new State { Name = $"S{i}" });
            }
            var triConfigs = new List<TriangleConfiguration>();
            var tempConfig = new TriangleConfiguration(states);
            tempConfig.StateCounts[states[0]] = 3;
            tempConfig.StateWeights[states[0]][states[1]] = 100;
            triConfigs.Add(tempConfig);
            tempConfig = new TriangleConfiguration(states);
            tempConfig.StateCounts[states[1]] = 3;
            tempConfig.StateWeights[states[1]][states[0]] = 100;
            triConfigs.Add(tempConfig);
            tempConfig = new TriangleConfiguration(states);
            tempConfig.StateCounts[states[0]] = 2;
            tempConfig.StateCounts[states[1]] = 1;
            tempConfig.StateWeights[states[0]][states[1]] = 100;
            tempConfig.StateWeights[states[1]][states[0]] = 50;
            triConfigs.Add(tempConfig);
            tempConfig = new TriangleConfiguration(states);
            tempConfig.StateCounts[states[1]] = 2;
            tempConfig.StateCounts[states[0]] = 1;
            tempConfig.StateWeights[states[1]][states[0]] = 100;
            tempConfig.StateWeights[states[0]][states[1]] = 50;
            triConfigs.Add(tempConfig);
            var world = new RelnetWorld(nodes, states, triConfigs);
            Assert.AreEqual((NUM_NODES * (NUM_NODES - 1)) / 2, world.Relationships.Count, "Failed to create the proper number of relationships");
            for (int i = 0; i < 100; i++)
            {
                world.Step();
            }
        }
    }
}
