using Player.Body;
using Player.Brain;

using System;
using System.Collections;
using UnityEngine;

namespace Player.Jump
{
    public class PlayerJump : MonoBehaviour
    {
        [Header("Player")]
        [SerializeField] private PlayerBody body;
        [SerializeField] private PlayerBrain brain; //TODO: is this really necessary?

        private bool shouldJump = true;
        private bool shouldJumpOnRamp = true;

        public event Action onJump = delegate { };

        public JumpModel Model { get; set; }

        private void Awake()
        {
            ValidateReferences();
        }

        public bool TryJump(float normalAcceleration)
        {
            //TODO: There MUST be a better way to do this
            if (!shouldJump) return false;

            if (!shouldJumpOnRamp) return false;

            if (!body.IsOnLand) return false;

            StartCoroutine(JumpSequence(normalAcceleration));

            return true;
        }

        private IEnumerator JumpSequence(float normalAcceleration)
        {
            shouldJump = false;

            body.RequestBrake(Model.BrakeMultiplier);
            brain.Acceleration = Model.JumpAcceleration;

            onJump?.Invoke();

            yield return new WaitForSeconds(Model.WaitToJump);

            body.RequestImpulse(new ImpulseRequest(Vector3.up, Model.Force));

            yield return new WaitForSeconds(Model.Cooldown);

            brain.Acceleration = normalAcceleration;
        }

        private void OnCollisionEnter(Collision collision)
        {
            var contact = collision.contacts[0];
            var contactAngle = Vector3.Angle(contact.normal, Vector3.up); //TODO: Research vector3.angle

            if (contactAngle >= 90)
                contactAngle = 0;

            if (contactAngle <= Model.FloorAngle)
            {
                shouldJump = true;
                shouldJumpOnRamp = true;
            }

            else
            {
                shouldJumpOnRamp = false;
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
