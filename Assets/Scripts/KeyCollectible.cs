using UnityEngine;

public class KeyCollectible : MonoBehaviour
{
    public int keyID;
    public Timer timer;

    [SerializeField] private PlayerAnimations playerAnimations;
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerMovement playerMovement;
    private void Awake()
    {
        timer = FindFirstObjectByType<Timer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
          
            GameMaster.CollectKey(keyID);
            CoinManager.instance.SaveCoins();

            if (timer != null)
                timer.EndTimer(keyID);

            GameMaster.SaveGame();
            if (playerMovement != null) {
                playerMovement.canMove = false;
            }
          
            if (playerAnimations != null)
            {
                playerAnimations.PlayKeyPickupAnimation();
            }

          
            if (animator != null)
            {
                animator.applyRootMotion = true;
            }
        }
    }
}
