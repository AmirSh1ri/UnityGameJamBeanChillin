using UnityEngine;
using System.Collections;

public class EffectLogic : MonoBehaviour
{
    [SerializeField] float newMass = 1f;
    [SerializeField] float newDamping = 0f;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Rigidbody playerRb = other.attachedRigidbody;
        if (!playerRb) playerRb = other.GetComponent<Rigidbody>();
        if (!playerRb) return;

        StartCoroutine(ApplyEffect(playerRb));
    }

    IEnumerator ApplyEffect(Rigidbody playerRb)
    {
        playerRb.mass = newMass;
        playerRb.linearDamping = newDamping;

        yield return new WaitForSeconds(2f);

        playerRb.mass = 0.7f;
        playerRb.linearDamping = 0.3f;
    }
}
