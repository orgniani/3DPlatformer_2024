using UnityEngine;

namespace Player.Brain
{
    public class PlayerBrain : MonoBehaviour
    {
        private float acceleration;
        public BrainModel Model { get; set; }

        public float Acceleration
        {
            set { acceleration = value; }
        }
    }

}
