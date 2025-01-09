using Core;
using Core.Interactions;
using Events;
using UnityEngine;

namespace Characters
{
    public class Character : MonoBehaviour, ITarget
    {
        [Header("Logs")]
        [SerializeField] private bool enableLogs = true;

        private Rigidbody rb;

        private void Awake()
        {
            if (TryGetComponent<Rigidbody>(out var rigidbody))
                rb = rigidbody;
        }

        private void OnEnable()
        {
            if (EventManager<string>.Instance)
                EventManager<string>.Instance.SubscribeToEvent(GameEvents.WinAction, EnableIsKinematic);
        }

        private void OnDisable()
        {
            if (EventManager<string>.Instance)
                EventManager<string>.Instance.UnsubscribeFromEvent(GameEvents.WinAction, EnableIsKinematic);
        }

        //TODO: RESEARCH IF THERE'S A BETTER WAY TO DO THIS
        private void EnableIsKinematic(params object[] args)
        {
            if (rb) rb.isKinematic = true;

        }

        public void ReceiveAttack()
        {
            if (EventManager<string>.Instance)
                EventManager<string>.Instance.InvokeEvent(GameEvents.LoseAction, true);

            if (enableLogs) Debug.Log($"<color=red> {name}: received an attack! </color>");
        }

        public void SetStartPosition(Vector3 levelStartPosition)
        {
            transform.position = levelStartPosition;
            if (rb) rb.isKinematic = false;
        }
    }
}