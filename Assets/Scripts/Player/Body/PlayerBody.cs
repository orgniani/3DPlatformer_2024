using System.Collections.Generic;
using UnityEngine;

namespace Player.Body
{
    [RequireComponent((typeof(Rigidbody)))]
    public class PlayerBody : MonoBehaviour
    {
        private Rigidbody rigidBody;

        //private MovementRequest currentMovement = MovementRequest.InvalidRequest;

        public BodyModel Model { get; set; }

    }
}
