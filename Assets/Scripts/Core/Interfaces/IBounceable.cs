using System.Collections;
using UnityEngine;

namespace Core.Interfaces
{
    public interface IBounceable
    {
        bool IsBouncing { get; set; }
        IEnumerator TrampolineBounce(Vector3 bounceForce);
    }
}
