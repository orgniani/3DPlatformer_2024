using Core.Interactions;
using Events;
using UnityEngine;

namespace Characters
{
    public class Character : MonoBehaviour, ITarget
    {
        [Header("Logs")]
        [SerializeField] private bool enableLogs = true;

        private Rigidbody _rigidBody;

        private void Awake()
        {
            if (TryGetComponent<Rigidbody>(out var rigidbody))
                _rigidBody = rigidbody;
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
            if (_rigidBody) _rigidBody.isKinematic = true;
            transform.position = Vector3.zero;

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
            if (_rigidBody) _rigidBody.isKinematic = false;
        }
    }
}