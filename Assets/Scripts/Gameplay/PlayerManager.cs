//TODO: using Player;
using DataSources;
using Events;
using UnityEngine;
using Core;
using UnityEngine.TextCore.Text;

namespace Gameplay
{
    public class PlayerManager : MonoBehaviour
    {
        [Header("References")]
        [Header("Data Sources")]
        [SerializeField] private DataSource<PlayerManager> playerDataSource;
        //[SerializeField] private DataSource<Character> characterDataSource;

        //private PlayerBrain _player; //TODO: Should it be character?

        

        private void Awake()
        {
            if (!playerDataSource)
            {
                Debug.LogError($"{name}: {nameof(playerDataSource)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }
        }

        private void OnEnable()
        {
            //TODO: Subscribe to inputs via event manager/event channel | DONE

            if (EventManager<string>.Instance)
            {
                
                //EventManager<string>.Instance.SubscribeToEvent(GameEvents.MoveAction, OnMoveEvent);
                //EventManager<string>.Instance.SubscribeToEvent(GameEvents.RunAction, OnRunEvent);
            }

            //TODO: Set itself as player reference via ReferenceManager/DataSource | DONE
            if (playerDataSource.Value == null)
                playerDataSource.Value = this;

        }

        private void OnDisable()
        {
            //TODO: Unsubscribe from all inputs via event manager/event channel | DONE

            if (EventManager<string>.Instance)
            {
                //EventManager<string>.Instance.UnsubscribeFromEvent(GameEvents.MoveAction, OnMoveEvent);
                //EventManager<string>.Instance.UnsubscribeFromEvent(GameEvents.RunAction, OnRunEvent);
            }

            //TODO: Remove itself as player reference via reference manager/dataSource | DONE
            if (playerDataSource.Value == this)
                playerDataSource.Value = null;
        }

        public void SetPlayerAtLevelStartAndEnable(Vector3 levelStartPosition)
        {
            transform.position = levelStartPosition;
        }

        /*
        private void OnMoveEvent(params object[] args)
        {
            if (args.Length > 0 && args[0] is Vector3 direction)
            {
                HandleMove(direction);
            }
        }

        private void OnRunEvent(params object[] args)
        {
            if (args.Length > 0 && args[0] is bool shouldRun)
            {
                HandleRun(shouldRun);
            }
        }

        private void HandleMove(Vector2 direction)
        {
            _player.SetDirection(new Vector3(direction.x, 0, direction.y));
        }

        private void HandleRun(bool shouldRun)
        {
            if (shouldRun)
                _player.StartRunning();
            else
                _player.StopRunning();
        }

        public void ReceiveAttack()
        {

        }
        */
    }
}

