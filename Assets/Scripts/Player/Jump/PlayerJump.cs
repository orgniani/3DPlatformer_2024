using Player.Body;
using Player.Brain;
using Events;
using Audio;

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
            //TODO: There MUST be a better way to do this
            if (!_shouldJump) return false;

            if (!_shouldJumpOnRamp) return false;

            if (!body.IsOnLand) return false;

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

            if (EventManager<string>.Instance)
                EventManager<string>.Instance.InvokeEvent(GameEvents.PlayAudioAction, Model.JumpAudio, gameObject);

            yield return new WaitForSeconds(Model.WaitToJump);

            body.RequestImpulse(new ImpulseRequest(Vector3.up, Model.Force));

            yield return new WaitForSeconds(Model.Cooldown);

            brain.Acceleration = normalAcceleration;
        }

        //TODO: Check if this can be improved
        public IEnumerator TrampolineJump(Vector3 extraForce)
        {
            OnJump?.Invoke();

            //TODO: Get rid of hardcoded value
            yield return new WaitForSeconds(0.1f);

            _shouldJump = false;
            body.RequestImpulse(new ImpulseRequest(Vector3.up, Model.Force + extraForce.magnitude));
        }

        private void OnCollisionEnter(Collision collision)
        {
            //TODO: Check if this is truly necesary
            if (!body.IsOnLand) return;

            //TODO: Add summary explaing how this should only calculate the collisions with the collider used for the characters feet rather than both feet and body.!
            if (collision.GetContact(0).thisCollider != footCollider) return;

            var contact = collision.contacts[0];
            var contactAngle = Vector3.Angle(contact.normal, Vector3.up); //TODO: Research vector3.angle

            if (contactAngle >= 90)
                contactAngle = 0;

            Debug.Log("CONTACT ANGLE: " + contactAngle);

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
