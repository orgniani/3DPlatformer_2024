using System;
using UnityEngine;

namespace Player
{
    public readonly struct MovementRequest : IEquatable<MovementRequest> //TODO: Research IEquatable
    {
        public readonly Vector3 Direction;

        public readonly float Acceleration;

        public readonly float GoalSpeed;

        public static MovementRequest InvalidRequest => new(Vector3.zero, 0, 0);

        public MovementRequest(Vector3 direction, float goalSpeed, float acceleration)
        {
            Direction = direction;
            GoalSpeed = goalSpeed;
            Acceleration = acceleration;
        }

        public Vector3 GetGoalVelocity() => Direction * GoalSpeed;

        public Vector3 GetAccelerationVector() => Direction * Acceleration;

        public bool IsValid() => this != InvalidRequest;


        public bool Equals(MovementRequest other)
        {
            return Direction.Equals(other.Direction)
                   && GoalSpeed.Equals(other.GoalSpeed)
                   && Acceleration.Equals(other.Acceleration);
        }

        public override bool Equals(object obj)
        {
            return obj is MovementRequest other && Equals(other);
        }

        public override int GetHashCode() //TODO: GetHashCode?
        {
            unchecked
            {
                var hashCode = Direction.GetHashCode();
                hashCode = (hashCode * 397) * GoalSpeed.GetHashCode(); //TODO: Why 397?
                return hashCode;
            }
        }

        public static bool operator ==(MovementRequest one, MovementRequest two) //TODO: Where is this used?
        {
            return one.Equals(two);
        }

        public static bool operator !=(MovementRequest one, MovementRequest two) //TODO: Where is this used?
        {
            return !(one == two);
        }
    }
}