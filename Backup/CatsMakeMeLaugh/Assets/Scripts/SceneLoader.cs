using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] string prevSceneName;
    [SerializeField] public string nextSceneName;
    public bool isLoading = false;
    public bool unloadPreviousScene = false;

    public void LoadScene()
    {
        if(!isLoading) StartCoroutine(LoadSceneAsync());
    }

    private IEnumerator LoadSceneAsync()
    {
        isLoading = true;
        int possibleScenes = SceneManager.sceneCount;
        if(possibleScenes > 1 )
            prevSceneName = SceneManager.GetSceneAt(1).name;

        AsyncOperation operation = SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Additive);

        while (!operation.isDone)
        {
            yield return null;
        }
        
        isLoading = false;

        if(unloadPreviousScene)
        {
            StartCoroutine(UnloadSceneAsync());
        }
    }

    private IEnumerator UnloadSceneAsync()
    {
        if(string.IsNullOrEmpty(prevSceneName)) { yield break; }

        AsyncOperation operation = SceneManager.UnloadSceneAsync(prevSceneName);
        
        while (!operation.isDone) { yield return null; }

    }
}
