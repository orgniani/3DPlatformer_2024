using Player.Body;
using Player.Brain;
using Core.Interactions;
using System;
using System.Collections;
using UnityEngine;

namespace Player.Jump
{
    public class PlayerJump : MonoBehaviour, IBounceable
    {
        [Header("Player")]
        [SerializeField] private PlayerBody body;
        [SerializeField] private PlayerBrain brain;

        [SerializeField] private Collider footCollider;

        private bool _shouldJump = true;
        private bool _shouldJumpOnRamp = true;

        public event Action OnJump = delegate { };

        public JumpModel Model { get; set; }

        private void Awake()
        {
            ValidateReferences();
        }

        public bool TryJump(float normalAcceleration)
        {
            if (!_shouldJump || !_shouldJumpOnRamp || !body.IsOnLand)
                return false;

            _shouldJump = false;
            StartCoroutine(JumpSequence(normalAcceleration));

            return true;
        }

        private IEnumerator JumpSequence(float normalAcceleration)
        {
            _shouldJump = false;

            body.RequestBrake(Model.BrakeMultiplier);
            brain.Acceleration = Model.JumpAcceleration;

            OnJump?.Invoke();

            yield return new WaitForSeconds(Model.WaitToJump);

            body.RequestImpulse(new ImpulseRequest(Vector3.up, Model.Force));

            yield return new WaitForSeconds(Model.Cooldown);

            brain.Acceleration = normalAcceleration;
        }

        public IEnumerator TrampolineBounce(Vector3 bounceForce)
        {
            _shouldJump = false;
            body.RequestBrake(Model.BrakeMultiplier);

            OnJump?.Invoke();

            yield return new WaitForSeconds(Model.WaitToJump);

            body.RequestImpulse(new ImpulseRequest(Vector3.up, Model.Force + bounceForce.magnitude));
        }

        private void OnCollisionEnter(Collision collision)
        {
            /// <summary>
            /// Ensures that collision calculations are only processed when the contact point  
            /// belongs to the character's foot collider. This prevents unintended interactions  
            /// from other parts of the body.
            /// </summary>
            if (collision.GetContact(0).thisCollider != footCollider) return;

            var contact = collision.contacts[0];
            var contactAngle = Vector3.Angle(contact.normal, Vector3.up); //TODO: Research vector3.angle

            if (contactAngle >= 90)
                contactAngle = 0;

            if (contactAngle <= Model.FloorAngle)
            {
                _shouldJump = true;
                _shouldJumpOnRamp = true;
            }

            else
            {
                _shouldJumpOnRamp = false;
            }
        }

        private void ValidateReferences()
        {
            if (!body)
            {
                Debug.LogError($"{name}: {nameof(body)} is null!" +
                               $"\nDisabling object to avoid errors.");
                enabled = false;
                return;
            }

            if (!brain)
            {
                Debug.LogError($"{name}: {nameof(brain)} is null!" +
                               $"\nDisabling object to avoid errors.");
                enabled = false;
                return;
            }
        }
    }

}
