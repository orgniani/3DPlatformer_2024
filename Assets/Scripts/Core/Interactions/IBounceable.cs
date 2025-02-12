using System.Collections;
using UnityEngine;

namespace Core.Interactions
{
    public interface IBounceable
    {
        IEnumerator TrampolineBounce(Vector3 bounceForce);
    }
}
