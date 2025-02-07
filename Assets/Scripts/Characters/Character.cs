using Core.Interactions;
using Events;
using UnityEngine;
using Characters.Health;

namespace Characters
{
    public class Character : MonoBehaviour, ITarget //TODO: is the interface necessary?
    {
        [Header("Logs")]
        [SerializeField] private bool enableLogs = true;

        [Header("Models")]
        [SerializeField] private DamageModelContainer damageModelContainer; //TODO: Validate reference

        private Rigidbody _rigidBody;

        public DamageModel Model { get; set; }

        public DamageModelContainer DamageModelContainer
        {
            get
            {
                return damageModelContainer;
            }

            set
            {
                damageModelContainer = value;
                Model = damageModelContainer.Model;
            }
        }

        private void Awake()
        {
            if (TryGetComponent<Rigidbody>(out var rigidbody))
                _rigidBody = rigidbody;

            Model = damageModelContainer.Model;
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
            if (Model.IsInvincible) return;

            if (EventManager<string>.Instance)
                EventManager<string>.Instance.InvokeEvent(GameEvents.LoseAction);

            if (enableLogs) Debug.Log($"<color=red> {name}: received an attack! </color>");
        }

        public void SetStartPosition(Vector3 levelStartPosition, Quaternion levelStartRotation)
        {
            transform.SetPositionAndRotation(levelStartPosition, levelStartRotation);
            if (_rigidBody) _rigidBody.isKinematic = false;
        }
    }
}