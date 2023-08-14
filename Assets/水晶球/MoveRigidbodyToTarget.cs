using UnityEngine;

public class MoveRigidbodyToTarget : MonoBehaviour
{

    public Transform target;
    public int speed;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        rb.MovePosition(Vector3.Lerp(transform.position, target.transform.position, speed * Time.deltaTime));
        rb.MoveRotation(Quaternion.Slerp(transform.rotation, target.rotation, 10 * Time.deltaTime));
    }
}