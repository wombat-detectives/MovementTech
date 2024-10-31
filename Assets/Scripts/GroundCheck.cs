using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public PlayerMovement playerMovement;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == playerMovement.gameObject)
        {
            return;
        }

        playerMovement.SetGrounded(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == playerMovement.gameObject)
        {
            return;
        }

        playerMovement.SetGrounded(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == playerMovement.gameObject)
        {
            return;
        }

        playerMovement.SetGrounded(true);
    }
}
