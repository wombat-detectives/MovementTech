using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    public string uniqueTag;
    public bool musicPlayer;

    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag(uniqueTag);

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

}
