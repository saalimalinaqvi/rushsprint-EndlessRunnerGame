using UnityEngine;

public class AnimalRotate : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float maxHeight = 5f;
    public float minHeight = 0f;

    private bool movingUp = true;

    void Update()
    {
        if (movingUp)
        {
            transform.position += Vector3.up * moveSpeed * Time.deltaTime; // Move Up
        }
        else
        {
            transform.position += Vector3.down * moveSpeed * Time.deltaTime; // Move Down
        }

        if (transform.position.y >= maxHeight && movingUp)
        {
            Flip();
            movingUp = false;
        }
        else if (transform.position.y <= minHeight && !movingUp)
        {
            Flip();
            movingUp = true;
        }
    }

    void Flip()
    {
        transform.Rotate(180, 0, 0); // Flip the object
    }
}
