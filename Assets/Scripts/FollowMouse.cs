using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    public Rigidbody targetRB;
    public Camera cam;

    public float followForce = 1f;
    public float lookZOffset = 1f;

    void Update()
    {
        Vector3 targetPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, targetRB.position.z));

        Vector3 forceToApply = (targetPos - targetRB.position) * followForce;

        targetRB.AddForce(forceToApply, ForceMode.Force);
            
        targetRB.transform.LookAt(cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, targetRB.position.z + lookZOffset)));
    }
}
