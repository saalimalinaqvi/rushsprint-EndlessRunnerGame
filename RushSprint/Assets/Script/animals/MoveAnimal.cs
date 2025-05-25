using UnityEngine;

public class ObjectMoveReset : MonoBehaviour
{
    public Vector3 moveDirection = Vector3.right; // Change direction if needed
    public float moveSpeed = 5f;
    public float moveDistance = 10f;

    private Vector3 startPosition;
    private float traveledDistance = 0f;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // Move the object
        transform.position += moveDirection.normalized * moveSpeed * Time.deltaTime;
        traveledDistance += moveSpeed * Time.deltaTime;

        // Check if object has moved the required distance
        if (traveledDistance >= moveDistance)
        {
            transform.position = startPosition; // Reset position
            traveledDistance = 0f; // Reset distance tracker
        }
    }
}
