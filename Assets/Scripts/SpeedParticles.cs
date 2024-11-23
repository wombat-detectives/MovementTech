using UnityEngine;

public class SpeedParticles : MonoBehaviour
{
    [SerializeField]
    private Rigidbody playerRB;
    [SerializeField]
    private float posOffset;
    [SerializeField]
    private float requiredSpeed;

    private ParticleSystem particles;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        particles = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        setPosition();

        //Toggle based on speed
        if(playerRB.linearVelocity.magnitude > requiredSpeed)
        {
            particles.Play();
            ParticleSystem.MainModule main = particles.main;
            main.startSpeed = playerRB.linearVelocity.magnitude;
        } else
        {
            particles.Stop();
        }
    }

    private void setPosition()
    {
        Vector3 lateralVel = new Vector3(playerRB.linearVelocity.x, 0, playerRB.linearVelocity.z);

        // Position in front of move direction so particles appear around player
        transform.position = playerRB.position + lateralVel.normalized * posOffset;

        // Rotate away from move direction so particles go opposite
        Vector3 lookTarget = playerRB.position - lateralVel;
        transform.LookAt(lookTarget);
    }
}
