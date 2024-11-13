using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTrigger : MonoBehaviour
{

    [Tooltip("Only needed if target self is false")] public string targetScene;
    public bool targetSelf = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(targetSelf)
                LevelLoader.instance.LoadLevelByInt(SceneManager.GetActiveScene().buildIndex);
            else
                LevelLoader.instance.LoadLevelByString(targetScene);
        }
    }
}
