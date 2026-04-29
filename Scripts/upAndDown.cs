using UnityEngine;

public class upAndDown : MonoBehaviour
{
    public float amplitude = 0.5f;
    public float frequency = 1f;

    public Vector3 rotationSpeed = new Vector3(0f, 30f, 0f);

    Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float yOffset = Mathf.Sin(Time.time * frequency * Mathf.PI * 2f) * amplitude;
        transform.position = startPos + Vector3.up * yOffset;

        transform.Rotate(rotationSpeed * Time.deltaTime, Space.Self);
    }
}
