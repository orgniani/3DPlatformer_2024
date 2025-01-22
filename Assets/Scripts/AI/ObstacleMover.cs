using Codice.CM.Client.Differences;
using System.Collections;
using UnityEngine;

//TODO: Change name of script
[RequireComponent((typeof(Rigidbody)))]
public class ObstacleMover : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private Vector3 targetOffset = new Vector3(0, 0, 0.2f);
    [SerializeField] private float moveSpeed = 2f;

    [SerializeField] private float movementDuration = 1f;


    private Rigidbody _rigidBody;
    private Vector3 targetPosition;
    private Vector3 movementDirection;

    [Header("Logs")]
    [SerializeField] private bool enableLogs = true;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        targetPosition = transform.position + targetOffset;

        movementDirection = (targetPosition - transform.position).normalized; // Direction towards target
    }

    public IEnumerator MoveToTarget()
    {
        float elapsedTime = 0f;

        while (elapsedTime < movementDuration)
        {
            Vector3 force = movementDirection * moveSpeed;

            _rigidBody.AddForce(force, ForceMode.Force);

            elapsedTime += Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();
        }

        OnMovementComplete();
    }

    private void OnMovementComplete()
    {
        if (enableLogs) Debug.Log($"{name}: Movement complete.");
        enabled = false;
    }
}
