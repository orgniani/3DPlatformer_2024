using Events;
using UnityEngine;
using Characters.Health;

namespace Characters
{
    public class Character : MonoBehaviour
    {
        [Header("Logs")]
        [SerializeField] private bool enableLogs = true;

        [Header("Models")]
        [SerializeField] private DamageModelContainer damageModelContainer;

        public Rigidbody CharacterRigidBody { get; private set; }

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
                CharacterRigidBody = rigidbody;

            if (!damageModelContainer)
            {
                Debug.LogError($"{name}: {nameof(damageModelContainer)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }

            Model = damageModelContainer.Model;
        }

        private void OnEnable()
        {
            if (!CharacterRigidBody) return;
            EnableIsKinematic();

            if (EventManager<string>.Instance)
                EventManager<string>.Instance.SubscribeToEvent(GameEvents.WinAction, EnableIsKinematic);
        }

        private void OnDisable()
        {
            if (!CharacterRigidBody) return;

            if (EventManager<string>.Instance)
                EventManager<string>.Instance.UnsubscribeFromEvent(GameEvents.WinAction, EnableIsKinematic);
        }

        private void EnableIsKinematic(params object[] args)
        {
            CharacterRigidBody.isKinematic = true;
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
            if (CharacterRigidBody) CharacterRigidBody.isKinematic = false;
        }
    }
}