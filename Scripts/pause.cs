using UnityEngine;

public class pause : MonoBehaviour
{
    public GameObject pauseUI;
    bool paused;

    void Start()
    {
        Application.targetFrameRate = 90;

        if (pauseUI) pauseUI.SetActive(false);
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    void TogglePause()
    {
        paused = !paused;
        ApplyPauseState();
    }

    public void Resume()
    {
        paused = false;
        ApplyPauseState();
    }

    public void Quit()
    {
        Time.timeScale = 1f;
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    void ApplyPauseState()
    {
        Time.timeScale = paused ? 0.15f : 1f;

        if (pauseUI)
            pauseUI.SetActive(paused);
    }

    void OnDisable()
    {
        Time.timeScale = 1f;
        Application.targetFrameRate = 90;
    }
}
