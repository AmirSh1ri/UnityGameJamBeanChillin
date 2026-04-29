using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BeginningScene : MonoBehaviour
{
    [SerializeField] Animator young;
    [SerializeField] Animator old;

    public void Play()
    {
        StartCoroutine(PlaySequence());
    }
    public void PlayEnd()
    {
        SceneManager.LoadScene(0);
    }

    IEnumerator PlaySequence()
    {
        if (young)
            young.Play("young2");

        yield return new WaitForSeconds(5f);

        if (old)
            old.Play("Old2");

        yield return new WaitForSeconds(15f);

        SceneManager.LoadScene(1);
    }
}
