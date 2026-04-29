using UnityEngine;
using System.Collections;

public class autoDelete : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float deleteDistance = 2500f;
    [SerializeField] float checkInterval = 10f;

    void Start()
    {
        if (!player)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p) player = p.transform;
        }

        StartCoroutine(CheckDistance());
    }

    IEnumerator CheckDistance()
    {
        while (true)
        {
            if (player)
            {
                if (Vector3.Distance(transform.position, player.position) > deleteDistance)
                {
                    Destroy(gameObject);
                    yield break;
                }
            }

            yield return new WaitForSeconds(checkInterval);
        }
    }
}
