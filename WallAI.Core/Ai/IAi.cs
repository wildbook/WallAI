using System.Runtime.Serialization;

namespace WallAI.Core.Ai
{
    public interface IAi
    {
        [DataMember(Name = nameof(Name))]
        string Name { get; }

        void Tick(IAiCore ai);
    }
}