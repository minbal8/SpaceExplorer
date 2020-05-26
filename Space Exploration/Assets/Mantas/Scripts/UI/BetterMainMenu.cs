using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BetterMainMenu : MonoBehaviour
{
    public AudioSettings audioSettings;

    bool isLoadingScene = false;
    private AsyncOperation sceneLoadingOperation;

    // Start is called before the first frame update
    void Start()
    {
        audioSettings.UpdateSound();
    }

    // Update is called once per frame
    void Update()
    {
        if (isLoadingScene)
        {
            if (!sceneLoadingOperation.isDone)
            {
                Debug.Log(sceneLoadingOperation.progress);
            }
        }
    }

    public void PlayGame()
    {
        sceneLoadingOperation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        isLoadingScene = true;
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Game exited");
    }


}
