using Player.Body;
using Player.Brain;

using System;
using System.Collections;
using UnityEngine;

namespace Player.Jump
{
    public class PlayerJump : MonoBehaviour
    {
        //TODO: Review script
        [SerializeField] private PlayerBody body;
        [SerializeField] private PlayerBrain brain; //TODO: is this really necessary?

        private bool shouldJump = true;
        private bool shouldJumpOnRamp = true;

        public event Action onJump = delegate { };

        public JumpModel Model { get; set; }

        private void Awake()
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

        public bool TryJump(float normalAcceleration)
        {
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

            //shouldJump = true;
        }

        private void OnCollisionEnter(Collision collision)
        {
            var contact = collision.contacts[0];
            var contactAngle = Vector3.Angle(contact.normal, Vector3.up);

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
    }

}
