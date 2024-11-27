using UnityEngine;

public class RepulsorTrigger : MonoBehaviour
{
    public float force;
    public Vector3 centerOffset = Vector3.zero;

    private void OnTriggerEnter(Collider other)
    {
        //For player
        Rigidbody otherRB = other.gameObject.GetComponentInParent<Rigidbody>();

        //For regular props
        if (otherRB == null )
            otherRB = other.gameObject.GetComponent<Rigidbody>();

        if (otherRB != null)
        {
            Vector3 center = transform.position + centerOffset;
            Vector3 dir = (other.transform.position - center).normalized;
            otherRB.AddForce(dir * force * otherRB.mass, ForceMode.Impulse);
        }
    }
}
