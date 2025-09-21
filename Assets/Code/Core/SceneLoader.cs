using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Header("UI Fade (optional)")]
    public CanvasGroup fadeCanvas;       // assign Bootstrap/Canvas/Fade
    [Min(0.1f)]public float fadeDuration = 0.25f;

    bool busy;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (fadeCanvas) fadeCanvas.alpha = 1f; // start faded
    }

    public void LoadInitial(string sceneName)
    {
        if (!gameObject.activeInHierarchy) return;
        StartCoroutine(LoadRoutine(null, sceneName, true));
    }

    public void SwitchTo(string nextScene)
    {
        if (!gameObject.activeInHierarchy || busy) return;
        var active = SceneManager.GetActiveScene();
        StartCoroutine(LoadRoutine(active, nextScene, false));
    }

    IEnumerator LoadRoutine(Scene? toUnloadNullable, string toLoad, bool firstLoad)
    {
        busy = true;

        // Fade in to black if not first load
        if (!firstLoad) yield return Fade(1f);

        // Load additively
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(toLoad, LoadSceneMode.Additive);
        loadOp.allowSceneActivation = false;

        // Pre-activation wait (simulate or hook to loading UI)
        while (loadOp.progress < 0.9f) yield return null;
        // Ready → activate
        loadOp.allowSceneActivation = true;
        while (!loadOp.isDone) yield return null;

        // Set active scene
        var loaded = SceneManager.GetSceneByName(toLoad);
        SceneManager.SetActiveScene(loaded);

        // Unload previous (if any and valid)
        if (toUnloadNullable.HasValue && toUnloadNullable.Value.IsValid())
        {
            AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(toUnloadNullable.Value);
            while (unloadOp != null && !unloadOp.isDone) yield return null;
        }

        // Fade out from black
        yield return Fade(0f);

        busy = false;
    }

    IEnumerator Fade(float target)
    {
        if (!fadeCanvas) yield break;
        float start = fadeCanvas.alpha;
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            fadeCanvas.alpha = Mathf.Lerp(start, target, t / fadeDuration);
            yield return null;
        }
        fadeCanvas.alpha = target;
    }
}
