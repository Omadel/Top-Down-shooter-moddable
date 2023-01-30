using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class UIManager : Etienne.Singleton<UIManager>
{
    private UIDocument document;
    private VisualElement buttonHolder;
    private Button playButton, modButton, optionsButton, quitButton;

    private void Start()
    {
        document = GetComponent<UIDocument>();
        VisualElement root = document.rootVisualElement;

        buttonHolder = root.Q<VisualElement>("Buttons");

        playButton = buttonHolder.Q<Button>("PlayButton");
        modButton = buttonHolder.Q<Button>("ModButton");
        optionsButton = buttonHolder.Q<Button>("OptionsButton");
        quitButton = buttonHolder.Q<Button>("QuitButton");

        playButton.clicked += PlayGame;
        modButton.clicked += StartModCreation;
        optionsButton.clicked += ShowOptions;
        quitButton.clicked += QuitGame;
    }

    private void PlayGame()
    {
        StartCoroutine(LoadRoutine(1));
    }

    private IEnumerator LoadRoutine(int sceneBuildIndex)
    {
        Time.timeScale = 0f;
        ProgressBar loadingBar = document.rootVisualElement.Q<ProgressBar>("Loading");
        loadingBar.style.display = DisplayStyle.Flex;
        loadingBar.highValue = 1f;
        loadingBar.lowValue = 0f;
        buttonHolder.style.display = DisplayStyle.None;
        AsyncOperation loadTask = SceneManager.LoadSceneAsync(sceneBuildIndex, LoadSceneMode.Additive);
        while (!loadTask.isDone)
        {
            loadingBar.value = loadTask.progress * .5f;
            yield return new WaitForEndOfFrame();
        }
        loadingBar.value = .5f;

        AsyncOperation unloadTask = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        while (!unloadTask.isDone)
        {
            loadingBar.value = unloadTask.progress * .5f + .5f;
            yield return new WaitForEndOfFrame();
        }
        loadingBar.value = 1f;

        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        document.enabled = false;
        Time.timeScale = 1f;
    }

    private void StartModCreation()
    {
        StartCoroutine(LoadRoutine(2));
    }

    private void ShowOptions()
    {
        Debug.LogWarning("Option Not Implemented");
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
	Application.Quit();
#endif
    }
}
