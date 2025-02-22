using System.Collections;
using UnityEngine;

namespace Core.Interfaces
{
    public interface IBounceable
    {
        IEnumerator TrampolineBounce(Vector3 bounceForce);
    }
}
