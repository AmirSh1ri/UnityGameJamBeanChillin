using UnityEngine;

public class triggerSpawnPlatform : MonoBehaviour
{
    public GameObject platform;
    bool used;

    void OnEnable()
    {
        used = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (used) return;
        if (!other.CompareTag("Player")) return;

        used = true;

        GameObject spawnPoint = GameObject.Find("SPAWNPOINTOFPLATFORM");
        if (!spawnPoint) return;

        Instantiate(
            platform,
            spawnPoint.transform.position,
            spawnPoint.transform.rotation
        );

        Destroy(spawnPoint);
    }
}
