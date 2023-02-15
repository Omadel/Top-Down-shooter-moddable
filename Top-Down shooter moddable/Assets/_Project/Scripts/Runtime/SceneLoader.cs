using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadGameScene()
    {
        SceneManager.LoadScene(0);
    }
    public void LoadMapEditorScene()
    {
        SceneManager.LoadScene(1);
    }
}
