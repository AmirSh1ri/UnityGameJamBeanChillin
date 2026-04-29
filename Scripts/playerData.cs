using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;
using UnityEngine.SceneManagement;

public class playerData : MonoBehaviour
{
    [Header("Data")]
    public float scoreTime;
    public float napClock = 60f;
    float napClockStart = 60f;
    public int notes;
    public float ClockSpeed;
    public bool wait15sec = true;

    [Header("Post Processing")]
    public Volume globalVolume;
    public float maxVignette = 1f;
    Vignette vignette;
    public Animator Death;

    [Header("UI")]
    [SerializeField] Image napBar;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text notesText;


    void Start()
    {
        StartCoroutine(waitBeforeClock());
        napClockStart = napClock;
        if (!globalVolume) return;

        var p = globalVolume.profile ? globalVolume.profile : globalVolume.sharedProfile;
        if (p) p.TryGet(out vignette);

        UpdateUI();

    }

    void Update()
    {
        if (vignette)
        {
            float t = 1f - (napClock / napClockStart);
            vignette.intensity.value = Mathf.Lerp(0f, maxVignette, t);
        }
        if (napClock == 0f)
        {
            StartCoroutine(Lose());
        }
        scoreTime += Time.deltaTime;
        napClock -= Time.deltaTime*2* (ClockSpeed * ClockSpeed);

        if (napClock < 0f)
            napClock = 0f;

        UpdateUI();
    }

    void UpdateUI()
    {
        if (napBar)
            napBar.fillAmount = napClockStart <= 0f ? 0f : napClock / napClockStart;

        if (scoreText)
            scoreText.text = GetScoreTimeMMSS();

        if (notesText)
            notesText.text = notes.ToString();
    }

    public void AddNotes(int amount)
    {
        notes += amount;
        UpdateUI();
    }
    public void AddClock(float seconds)
{
    napClock += seconds;
    if (napClock > napClockStart) napClock = napClockStart;
    UpdateUI();
}

    public void RemoveNotes(int amount)
    {
        notes -= amount;
        if (notes < 0) notes = 0;
        UpdateUI();
    }
        IEnumerator Lose()
    {
        Death.Play("WakeUp");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(2);
    }
    IEnumerator waitBeforeClock()
    {
        ClockSpeed = 0f;
        yield return new WaitForSeconds(15f);
        ClockSpeed = 1f;
        wait15sec = false;
    }
    public string GetScoreTimeMMSS()
    {
        int minutes = Mathf.FloorToInt(scoreTime / 60f);
        int seconds = Mathf.FloorToInt(scoreTime % 60f);
        return minutes.ToString("00") + ":" + seconds.ToString("00");
    }
}
