using UnityEngine;
using System.Collections;

public class pickables : MonoBehaviour
{
    public string playerTag = "Player";
    public Animator anim;
    public Collider myCol;

    bool picked;
    AudioSource sfx;

    void Awake()
    {
        if (!anim) anim = GetComponentInChildren<Animator>();
        if (!myCol) myCol = GetComponent<Collider>();
        sfx = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (picked) return;
        if (!other.CompareTag(playerTag)) return;

        picked = true;

        ApplyEffect(other.gameObject);

        if (sfx) sfx.Play();

        if (anim) anim.Play("pickup", 0, 0f);
        if (myCol) myCol.enabled = false;

        StartCoroutine(WaitToRemove());
    }

    void ApplyEffect(GameObject player)
    {
        PlayerMovement movement = player.GetComponent<PlayerMovement>();
        playerData data = player.GetComponent<playerData>();

        if (!movement || !data) return;

        int mult = movement.multiplier;

        if (CompareTag("Note"))
        {
            data.AddNotes(1 * mult);
        }
        else if (CompareTag("Energy"))
        {
            movement.SpeedBoost(20);
        }
        else if (CompareTag("Clock"))
        {
            data.AddClock(0.25f * (mult * 2));
        }
    }

    IEnumerator WaitToRemove()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
}
