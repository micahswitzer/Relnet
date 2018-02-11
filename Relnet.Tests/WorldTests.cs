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
            const int NUM_NODES = 50;
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
            var world = new RelnetWorld(nodes, states);
            Assert.AreEqual((NUM_NODES * (NUM_NODES - 1)) / 2, world.Relationships.Count, "Failed to create the proper number of relationships");
        }
    }
}
