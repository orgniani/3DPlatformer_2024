using Player.Body;
using Player.Brain;
using Player.Jump;
using Player.Rotation;
using UnityEngine;

namespace Player
{
    public class PlayerSetup : MonoBehaviour
    {
        [Header("Character Body")]
        [SerializeField] private BodyModelContainer bodyModelContainer;
        [SerializeField] private PlayerBody playerBody;

        [Header("Character Brain")]
        [SerializeField] private BrainModelContainer brainModelContainer;
        [SerializeField] private PlayerBrain playerBrain;

        [Header("Jump")]
        [SerializeField] private JumpModelContainer jumpModelContainer;
        [SerializeField] private PlayerJump playerJump;

        [Header("Rotation")]
        [SerializeField] private RotationModelContainer rotationModelContainer;
        [SerializeField] private PlayerRotation playerRotation;


        private void OnEnable()
        {
            // JUMP
            if (playerJump && jumpModelContainer)
            {
                playerJump.Model = jumpModelContainer.Model;
                playerJump.enabled = true;
            }

            else
            {
                Debug.LogError($"{name}: {nameof(playerJump)} or {nameof(jumpModelContainer)} is null!" +
                   $"\nDisabling component to avoid errors.");
            }

            // BODY
            if (playerBody && bodyModelContainer)
            {
                playerBody.Model = bodyModelContainer.Model;
                playerBody.enabled = true;
            }

            else
            {
                Debug.LogError($"{name}: {nameof(playerBody)} or {nameof(bodyModelContainer)} is null!" +
                   $"\nDisabling component to avoid errors.");
            }

            // BRAIN
            if (playerBrain && brainModelContainer)
            {
                playerBrain.Model = brainModelContainer.Model;
                playerBrain.enabled = true;
                playerBrain.Acceleration = brainModelContainer.Model.Acceleration;
            }

            else
            {
                Debug.LogError($"{name}: {nameof(playerBrain)} or {nameof(brainModelContainer)} is null!" +
                   $"\nDisabling component to avoid errors.");
            }

            // ROTATION
            if (playerRotation && rotationModelContainer)
            {
                playerRotation.Model = rotationModelContainer.Model;
                playerRotation.enabled = true;
            }

            else
            {
                Debug.LogError($"{name}: {nameof(playerRotation)} or {nameof(rotationModelContainer)} is null!" +
                   $"\nDisabling component to avoid errors.");
            }
        }
    }
}
