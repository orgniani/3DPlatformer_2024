using UnityEngine;
using DataSources;

namespace Characters.Health
{
    public class Hazard : MonoBehaviour
    {
        [Header("References")]
        [Header("Data Sources")]
        [SerializeField] private DataSource<Character> targetDataSource;

        [Header("Logs")]
        [SerializeField] private bool enableLogs = true;

        private Character _target;
        private bool _shouldCollide = true;

        private void Awake()
        {
            if (!targetDataSource)
            {
                Debug.LogError($"{name}: {nameof(targetDataSource)} is null!" +
                               $"\nDisabling component to avoid errors.");
                enabled = false;
                return;
            }
        }

        private void Start()
        {
            if (targetDataSource.Value != null)
                _target = targetDataSource.Value;

            else if (enableLogs) Debug.LogError($"{name}: <color=red> Target not found in level! </color>");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == _target.gameObject && _shouldCollide)
            {
                _shouldCollide = false;

                _target.ReceiveAttack();
                if (enableLogs) Debug.Log($"{name}: <color=orange> {_target.name} has received an attack! </color>");
            }

            _shouldCollide = true;
        }
    }
}