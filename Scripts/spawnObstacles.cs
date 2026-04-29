using UnityEngine;
using System.Collections.Generic;

public class spawnObstacles : MonoBehaviour
{
    [Header("Pools")]
    public GameObject[] RampsAndObstacles;
    public GameObject[] UpgradeRings;
    public GameObject[] Effects;
    public GameObject[] Pickables;

    [Header("Spawn Points")]
    public Transform[] RampObstacleSpawns;
    public Transform[] UpgradeRingSpawns;
    public Transform[] EffectSpawns;
    public Transform[] PickableSpawns;

    bool spawned;

    void OnEnable()
    {
        if (spawned) return;
        spawned = true;

        ClearChildren(RampObstacleSpawns);
        ClearChildren(EffectSpawns);
        ClearChildren(PickableSpawns);
        ClearChildren(UpgradeRingSpawns);

        SpawnGroup(RampsAndObstacles, RampObstacleSpawns, 3, 6);
        SpawnGroup(Effects, EffectSpawns, 0, 4);
        SpawnGroup(Pickables, PickableSpawns, 1, 3);
        SpawnGroup(UpgradeRings, UpgradeRingSpawns, 0, 1);
    }

    void OnDisable()
    {
        spawned = false;
    }

    void ClearChildren(Transform[] spawns)
    {
        if (spawns == null) return;

        for (int i = 0; i < spawns.Length; i++)
        {
            Transform t = spawns[i];
            if (!t) continue;

            for (int c = t.childCount - 1; c >= 0; c--)
                Destroy(t.GetChild(c).gameObject);
        }
    }

    void SpawnGroup(GameObject[] pool, Transform[] spawns, int minCount, int maxCount)
    {
        if (pool == null || pool.Length == 0) return;
        if (spawns == null || spawns.Length == 0) return;

        int count = Random.Range(minCount, maxCount + 1);
        count = Mathf.Clamp(count, 0, spawns.Length);

        List<int> indices = new List<int>(spawns.Length);
        for (int i = 0; i < spawns.Length; i++) indices.Add(i);

        for (int i = 0; i < count; i++)
        {
            int pickIndex = Random.Range(0, indices.Count);
            int spawnIndex = indices[pickIndex];
            indices.RemoveAt(pickIndex);

            Transform t = spawns[spawnIndex];
            Instantiate(Pick(pool), t.position, t.rotation, t);
        }
    }

    GameObject Pick(GameObject[] arr)
    {
        return arr[Random.Range(0, arr.Length)];
    }
}
