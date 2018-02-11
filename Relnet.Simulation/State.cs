using System.Diagnostics;

namespace Relnet.Simulation
{
    [DebuggerDisplay("{Name}")]
    public class State
    {
        public string Name { get; set; }
        public override string ToString() => Name;
    }
}