using System.Collections.Generic;
using UnityEngine;

namespace Player.Body
{
    [RequireComponent((typeof(Rigidbody)))]
    public class PlayerBody : MonoBehaviour
    {
        private Rigidbody _rigidBody;

        private MovementRequest _currentMovement = MovementRequest.InvalidRequest;

        private bool _isBrakeRequested = false;
        private float _brakeMultiplier;

        private readonly List<ImpulseRequest> _impulseRequests = new();

        private bool _shouldBreak = false;
        private bool _shouldCheckIfOnLand = true;

        public bool IsFalling { private set; get; }

        public bool IsOnLand { private set; get; }

        public BodyModel Model { get; set; }

        private void Reset()
        {
            _rigidBody = GetComponent<Rigidbody>();
        }

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            if (_isBrakeRequested)
                Break();

            ManageMovement();
            ManageImpulseRequests();
        }

        public void SetMovement(MovementRequest movementRequest)
        {
            _currentMovement = movementRequest;
        }

        public void RequestBrake(float brake)
        {
            _brakeMultiplier = brake;
            _isBrakeRequested = true;
        }

        public void RequestImpulse(ImpulseRequest request)
        {
            _impulseRequests.Add(request);
        }

        private void Break()
        {
            _rigidBody.AddForce(-_rigidBody.velocity * _brakeMultiplier, ForceMode.Impulse); //TODO: Research forcemode
            _isBrakeRequested = false;
        }

        private void ManageMovement()
        {
            var velocity = _rigidBody.velocity;
            velocity.y = 0;

            //TODO: There must be a way to improve this bit
            RaycastHit hit;

            Vector3 lineOffset = new Vector3(0f, Model.FloorLineCheckOffset, 0f);
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - Model.FloorSphereCheckOffset, transform.position.z);

            bool onFloorLineCheck = Physics.Raycast(transform.position + lineOffset, -transform.up, out hit, Model.MaxFloorDistance, Model.FloorMask);
            bool onFloorSphereCheck = Physics.CheckSphere(spherePosition, Model.FloorSphereCheckRadius, Model.FloorMask, QueryTriggerInteraction.Ignore);

            IsFalling = !onFloorLineCheck && !onFloorSphereCheck;

            CheckIfOnLand(onFloorLineCheck, onFloorSphereCheck);

            if (!_currentMovement.IsValid() || velocity.magnitude >= _currentMovement.GoalSpeed)
                return;

            var accelerationVector = _currentMovement.GetAccelerationVector();

            if (IsOnLand)
            {
                accelerationVector = Vector3.ProjectOnPlane(accelerationVector, hit.normal);
                Debug.DrawRay(transform.position, accelerationVector, Color.cyan);
            }

            Debug.DrawRay(transform.position, accelerationVector, Color.red);
            _rigidBody.AddForce(accelerationVector, ForceMode.Force);
        }

        private void ManageImpulseRequests()
        {
            foreach (var request in _impulseRequests)
            {
                _rigidBody.AddForce(request.GetForceVector(), ForceMode.Impulse);
            }

            _impulseRequests.Clear();
        }

        private void CheckIfOnLand(bool onFloorLineCheck, bool onFloorSphereCheck)
        {
            if (onFloorLineCheck)
            {
                if (!_shouldCheckIfOnLand) return;
                IsOnLand = onFloorSphereCheck;
            }

            else
            {
                if (onFloorSphereCheck) IsOnLand = true;

                else
                {
                    _shouldCheckIfOnLand = true;
                    IsOnLand = false;
                    _shouldBreak = true;
                }
            }

            if (IsOnLand == true) _shouldCheckIfOnLand = false;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (Model.FloorMask == (Model.FloorMask | (1 << collision.gameObject.layer)))
            {
                if (!_shouldBreak) return;

                RequestBrake(Model.LandBrakeMultiplier);
                _shouldBreak = false;
            }
        }

        private void OnDrawGizmos()
        {
            if (Model == null) return;

            Gizmos.color = Color.green;

            Vector3 floorLineCheck = new Vector3(0f, Model.FloorLineCheckOffset, 0f);
            Gizmos.DrawRay(transform.position + floorLineCheck, -transform.up * Model.MaxFloorDistance);

            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - Model.FloorSphereCheckOffset, transform.position.z);
            Gizmos.DrawWireSphere(spherePosition, Model.FloorSphereCheckRadius);
        }
    }
}
