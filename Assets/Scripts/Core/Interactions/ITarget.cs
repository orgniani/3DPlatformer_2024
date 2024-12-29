using UnityEngine;

namespace Core.Interactions
{
    public interface ITarget : IAttackable
    {
        Transform transform { get; }
    }
}