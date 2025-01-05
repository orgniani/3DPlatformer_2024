using Core;
using Core.Interactions;
using DataSources;
using Events;
using UnityEngine;

namespace Characters
{
    public class Character : MonoBehaviour, ITarget
    {
        public void ReceiveAttack()
        {
            if (EventManager<string>.Instance)
                EventManager<string>.Instance.InvokeEvent(GameEvents.LoseAction, true);

            Destroy(gameObject);
        }

        public void SetStartPosition(Vector3 levelStartPosition)
        {
            transform.position = levelStartPosition;
        }
    }
}