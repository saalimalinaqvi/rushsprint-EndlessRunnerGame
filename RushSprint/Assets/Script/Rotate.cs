using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    void Update()
    {
        this.transform.Rotate(Vector3.forward * Time.deltaTime * speed);
    }
}
