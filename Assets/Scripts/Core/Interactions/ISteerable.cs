using UnityEngine;

namespace Core.Interactions
{
    public interface ISteerable
    {
        void SetDirection(Vector3 direction);

        void SetStartPosition(Vector3 levelStartPosition);
    }
}
