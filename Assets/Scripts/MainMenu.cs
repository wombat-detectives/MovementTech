using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        LevelLoader.instance.LoadNextLevel();
    }

    public void NewGame()
    {
        GameMaster.NewGame();
        LevelLoader.instance.LoadNextLevel();
    }

    public void LoadGame()
    {
        GameMaster.LoadGame();
        if (GameMaster.tutorialComplete)
            LevelLoader.instance.LoadLevelByString("HubWorld");
        else
            LevelLoader.instance.LoadNextLevel();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
