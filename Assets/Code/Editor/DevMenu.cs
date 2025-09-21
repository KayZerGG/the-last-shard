#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;

public static class DevMenu
{
    [MenuItem("TLS/Play From Bootstrap %#p")] // Ctrl/Cmd+Shift+P
    static void PlayFromBootstrap()
    {
        if (EditorApplication.isPlaying) return;
        EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        EditorSceneManager.OpenScene("Assets/Scenes/Bootstrap.unity");
        EditorApplication.isPlaying = true;
    }
}
#endif