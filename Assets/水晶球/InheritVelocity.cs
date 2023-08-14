using UnityEngine;
using UnityEngine.VFX;

public class InheritVelocity : MonoBehaviour
{

    private VisualEffect visualEffect;
    private Rigidbody rb;
    public GameObject Plane;
    void Start()
    {
        visualEffect = GetComponent<VisualEffect>();
        rb = GetComponentInParent<Rigidbody>();


    }

    void Update()
    {


        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * 1, ForceMode.Impulse);

        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            rb.AddTorque(Vector3.forward * 30, ForceMode.Force);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            rb.Sleep();
        }
        visualEffect.SetVector3("inheritVelocity", transform.InverseTransformVector(rb.velocity) * 0.8f);
        visualEffect.SetVector3("inheritAngVelocity", transform.InverseTransformVector(rb.angularVelocity) * 5f);

        Debug.Log(transform.InverseTransformVector(rb.velocity) * 0.8f);
    }
}