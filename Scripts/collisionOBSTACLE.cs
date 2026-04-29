using UnityEngine;
using System.Collections;

public class collisionOBSTACLE : MonoBehaviour
{
    [Header("Hit Effect")]
    [SerializeField] Material targetMaterial;
    [SerializeField] Color hitColor = Color.red;
    [SerializeField] float flashTime = 0.1f;

    Color originalColor;
    bool flashing;
    AudioSource sfx;

    void Awake()
    {
        sfx = GetComponent<AudioSource>();

        if (targetMaterial)
            originalColor = targetMaterial.color;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerMovement pm = other.GetComponentInParent<PlayerMovement>();
        if (!pm) return;

        pm.SpeedBoost(-50);

        if (sfx) sfx.Play();

        if (targetMaterial && !flashing)
            StartCoroutine(FlashMaterial());
    }

    IEnumerator FlashMaterial()
    {
        flashing = true;

        targetMaterial.color = hitColor;
        yield return new WaitForSeconds(flashTime);
        targetMaterial.color = originalColor;

        flashing = false;
    }
}
