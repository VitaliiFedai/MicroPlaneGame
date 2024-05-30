#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public static class StartUpSceneLoader
{
    private const bool ENABLED = false;
    private const string START_UP_SCENE_NAME = "StartUp";

    static StartUpSceneLoader()
    {
        EditorApplication.playModeStateChanged += LoadDefaultScene;
    }

    static void LoadDefaultScene(PlayModeStateChange state)
    {
        if (!ENABLED)
        {
            return;
        }

        if (state == PlayModeStateChange.ExitingEditMode)
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        }

        if (state == PlayModeStateChange.EnteredPlayMode)
        {
            Scene scene = EditorSceneManager.GetActiveScene();
            if (scene.buildIndex != 0)
            {
                Debug.Log($"Wrong scene at {nameof(PlayModeStateChange.EnteredPlayMode)}. Loading the {START_UP_SCENE_NAME} instead.");
                EditorSceneManager.LoadScene(START_UP_SCENE_NAME);
            }
        }
    }
}
#endif