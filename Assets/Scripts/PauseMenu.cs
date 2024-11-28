using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject ui;

    private void Awake()
    {
        GameObject hud = GameObject.Find("HUD");
        ui = hud.transform.Find("PauseMenu").gameObject;
    }

    void Update()
    {
        var gamepad = Gamepad.current;
        if (gamepad != null)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || gamepad.startButton.wasPressedThisFrame)
            {
                Toggle();
            }
        } else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Toggle();
            }
        }
    }

    public void Toggle()
    {
        ui.SetActive(!ui.activeSelf);

        ToggleCursor();

        if (ui.activeSelf)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public void Restart()
    {
        Toggle();
        LevelLoader.instance.LoadLevelByInt(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToHub()
    {
        Toggle();
        LevelLoader.instance.LoadLevelByString("HubWorld");
    }

    public void Menu()
    {
        Debug.Log("go to menu");
    }

    private void ToggleCursor()
    {
        if (Cursor.lockState == CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
