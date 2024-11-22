using UnityEngine;

public class RepulsorTrigger : MonoBehaviour
{
    public float force;
    public Vector3 centerOffset = Vector3.zero;

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody otherRB = other.gameObject.GetComponentInParent<Rigidbody>();
        if(otherRB != null)
        {
            Vector3 center = transform.position + centerOffset;
            Vector3 dir = (other.transform.position - center).normalized;
            otherRB.AddForce(dir * force * otherRB.mass, ForceMode.Impulse);
            Debug.Log("Wrecked!! " + dir);
        }
    }
}
