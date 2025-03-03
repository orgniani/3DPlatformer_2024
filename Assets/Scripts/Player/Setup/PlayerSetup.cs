using DataSources;
using Player.Body;
using Player.Brain;
using Player.Jump;
using Player.Rotation;
using Characters;
using UnityEngine;
using Camera;

namespace Player.Setup
{
    [RequireComponent(typeof(Character))]
    [RequireComponent(typeof(PlayerBody))]
    [RequireComponent(typeof(PlayerBrain))]
    [RequireComponent(typeof(PlayerJump))]
    [RequireComponent(typeof(PlayerRotation))]
    public class PlayerSetup : MonoBehaviour
    {
        [Header("Data Sources")]
        [SerializeField] private DataSource<PlayerSetup> playerDataSource;
        [SerializeField] private DataSource<Character> characterDataSource;
        [SerializeField] private DataSource<CameraSetup> cameraDataSource;

        [Header("Model Containers")]
        [SerializeField] private BodyModelContainer bodyModelContainer;
        [SerializeField] private BrainModelContainer brainModelContainer;
        [SerializeField] private JumpModelContainer jumpModelContainer;
        [SerializeField] private RotationModelContainer rotationModelContainer;

        private PlayerBody _playerBody;
        private PlayerBrain _playerBrain;
        private PlayerJump _playerJump;
        private PlayerRotation _playerRotation;

        private Character _character;
        private CameraSetup _camera;

        public BrainModelContainer BrainModelContainer
        {
            get
            {
                return brainModelContainer;
            }

            set
            {
                brainModelContainer = value;
                _playerBrain.Model = brainModelContainer.Model;
                _playerBrain.Acceleration = brainModelContainer.Model.Acceleration;
            }
        }

        private void Reset()
        {
            _character = GetComponent<Character>();
        }

        private void Awake()
        {
            _character ??= GetComponent<Character>();

            _playerBody = GetComponent<PlayerBody>();
            _playerBrain = GetComponent<PlayerBrain>();
            _playerJump = GetComponent<PlayerJump>();
            _playerRotation = GetComponent<PlayerRotation>();
        }

        private void OnEnable()
        {
            ValidateReferences();

            SetDataSourcesValues();

            ValidateAndAssignValues();
        }

        private void OnDisable()
        {
            if (playerDataSource.Value == this)
                playerDataSource.Value = null;

            if (characterDataSource.Value == _character)
                characterDataSource.Value = null;
        }

        private void SetDataSourcesValues()
        {
            playerDataSource.Value = this;

            if (characterDataSource.Value == null)
                characterDataSource.Value = _character;

            if (cameraDataSource.Value != null)
            {
                _camera = cameraDataSource.Value;
                _camera.SetUp(transform);
            }
        }

        private void ValidateReferences()
        {
            if (!playerDataSource)
            {
                Debug.LogError($"{name}: {nameof(playerDataSource)} is null!" +
                               $"\nDisabling object to avoid errors.");
                enabled = false;
                return;
            }

            if (!characterDataSource)
            {
                Debug.LogError($"{name}: {nameof(characterDataSource)} is null!" +
                               $"\nDisabling object to avoid errors.");
                enabled = false;
                return;
            }

            if (!cameraDataSource)
            {
                Debug.LogError($"{name}: {nameof(cameraDataSource)} is null!" +
                               $"\nDisabling object to avoid errors.");
                enabled = false;
                return;
            }
        }

        private void ValidateAndAssignValues()
        {
            // JUMP
            if (jumpModelContainer)
            {
                _playerJump.Model = jumpModelContainer.Model;
                _playerJump.enabled = true;
            }

            else
            {
                Debug.LogError($"{name}: {nameof(jumpModelContainer)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }

            // BODY
            if (bodyModelContainer)
            {
                _playerBody.Model = bodyModelContainer.Model;
                _playerBody.enabled = true;
            }

            else
            {
                Debug.LogError($"{name}: {nameof(bodyModelContainer)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }

            // BRAIN
            if (brainModelContainer)
            {
                _playerBrain.Model = brainModelContainer.Model;
                _playerBrain.Camera = cameraDataSource.Value;

                _playerBrain.enabled = true;
                _playerBrain.Acceleration = brainModelContainer.Model.Acceleration;
            }

            else
            {
                Debug.LogError($"{name}: {nameof(brainModelContainer)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }

            // ROTATION
            if (rotationModelContainer)
            {
                _playerRotation.Model = rotationModelContainer.Model;
                _playerRotation.enabled = true;
            }

            else
            {
                Debug.LogError($"{name}: {nameof(rotationModelContainer)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }
        }
    }
}
