using UnityEngine;

namespace APGame.Abstractions
{
    public interface IClock
    {
        GameObject gameObject { get; }
        Transform transform { get; }
    }
}
