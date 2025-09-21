using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrapper : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void EnsureBootstrapInEditor()
    {
#if UNITY_EDITOR
        // If you press Play from Space_Hub or Planet_Proto, force-load Bootstrap first.
        if (SceneManager.GetActiveScene().name != "Bootstrap") SceneManager.LoadScene("Bootstrap", LoadSceneMode.Single);
#endif
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        // Find or add the SceneLoader
        var loader = FindFirstObjectByType<SceneLoader>(FindObjectsInactive.Include);
        if (!loader) loader = gameObject.AddComponent<SceneLoader>();

        // Hook up the Fade panel if it exists
        var fadeGO = GameObject.Find("Fader");
        if (fadeGO) loader.fadeCanvas = fadeGO.GetComponent<CanvasGroup>();

        // Kick off the very first gameplay scene under the black fade.
        loader.LoadInitial("Space_Hub");
    }
}
