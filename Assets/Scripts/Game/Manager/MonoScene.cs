using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPS;
using UnityEngine.SceneManagement;
using MEC;

public class MonoScene : MonoSingleton<MonoScene>
{
    private AsyncOperation asyncLoad;
    private bool active = true;

    public void Load(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void LoadAsync(string name)
    {
        if (name == SceneManager.GetActiveScene().name) return;
        if (!active) return;

        active = false;
        Application.backgroundLoadingPriority = ThreadPriority.Normal;

        StartCoroutine(LoadSceneAsync());

        asyncLoad = SceneManager.LoadSceneAsync(name);
        asyncLoad.allowSceneActivation = false;
    }

    public void Active(float delay = 0f)
    {
        Timing.RunCoroutine(_Active(delay));
    }

    private IEnumerator<float> _Active(float delay)
    {
        yield return Timing.WaitForSeconds(delay);
        active = true;
    }

    private IEnumerator LoadSceneAsync()
    {
        yield return null;

        while (true)
        {
            if (asyncLoad != null && active) asyncLoad.allowSceneActivation = true;

            if (asyncLoad != null && asyncLoad.isDone)
            {
                break;
            }

            yield return null;
        }
    }
}
