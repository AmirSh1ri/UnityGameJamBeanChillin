using UnityEngine;

public class Upgrade : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerMovement pm = other.GetComponent<PlayerMovement>();
        playerData pd = other.GetComponent<playerData>();

        if (pm == null || pd == null) return;
        if (pd.notes < 100) return;

        pd.RemoveNotes(100);

        if (CompareTag("gravity"))
        {
            if (pm.gravity < 6) { return; }
            pm.gravity -= 10f;
        }
        else
        {
            pd.AddClock(15);
        }
    }
}
