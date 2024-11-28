using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;



public class KeySelectScreen : MonoBehaviour
{
    public float fadeDuration = 1f;

    // Reference the image to fade
    [SerializeField] private Image imageToFade;

    // The name of the scene to load
    [SerializeField] private string sceneName;

    // Method to transition and load a new scene
    public void TransitionAndLoadScene()
    {
        StartCoroutine(FadeImageAndLoadScene(imageToFade, sceneName));
    }

    private IEnumerator FadeImageAndLoadScene(Image image, string sceneName)
    {
        if (image == null)
        {
            Debug.LogError("Image object is null!");
            yield break;
        }

        // Ensure the image is visible and fully transparent initially
        image.gameObject.SetActive(true);
        Color color = image.color;
        color.a = 0f;
        image.color = color;

        // Gradually increase the alpha to 1 over the fade duration
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            image.color = color;
            yield return null;
        }

        // Ensure alpha is exactly 1
        color.a = 1f;
        image.color = color;

        // Load the specified scene
        SceneManager.LoadScene(sceneName);
    }
}
