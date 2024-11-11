using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{

    public Animator transition;

    public float transitionTime;

    //Singleton pattern, so CoinManager is available everywhere
    public static LevelLoader instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("ERROR: more than one CoinManager in scene");
            return;
        }
        instance = this;
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void LoadLevelByInt(int levelIndex)
    {
        StartCoroutine(LoadLevel(levelIndex));
    }

    public void LoadLevelByString(string levelName)
    {
        StartCoroutine(LoadLevel(levelName));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        // play animation
        transition.SetTrigger("Start");

        // wait for anim
        yield return new WaitForSeconds(transitionTime);

        // load scene
        SceneManager.LoadScene(levelIndex);
    }

    IEnumerator LoadLevel(string levelName)
    {
        // play animation
        transition.SetTrigger("Start");

        // wait for anim
        yield return new WaitForSeconds(transitionTime);

        // load scene
        SceneManager.LoadScene(levelName);
    }
}
