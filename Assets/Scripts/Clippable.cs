using UnityEngine;

public class Clippable : MonoBehaviour
{
    public float requiredSpeed;
    private Collider thisColl;

    [SerializeField] private bool stayClippable;

    private void Start()
    {
        thisColl = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody otherRB = other.GetComponentInParent<Rigidbody>();
        if (otherRB == null) return;

        Debug.Log("Trigger");

        if (otherRB.linearVelocity.magnitude > requiredSpeed)
        {
            Physics.IgnoreCollision(other, thisColl);
        } else if (!stayClippable)
        {
            Physics.IgnoreCollision(other, thisColl, false);
        }
    }
}
