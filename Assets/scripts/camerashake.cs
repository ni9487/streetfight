using UnityEngine;
using System.Collections;

public class camerashake : MonoBehaviour
{
    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPosition = transform.position;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-0.1f, 0.1f) * magnitude;
            float y = Random.Range(-0.1f, 0.1f) * magnitude;

            transform.position = new Vector3(x, y, originalPosition.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition;
    }
}
