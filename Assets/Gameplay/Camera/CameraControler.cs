using System.Collections;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
    Vector3 originalPosition;

    public IEnumerator CameraShake(float duration, float magnitude)
    {
        originalPosition = transform.position;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;
            
            transform.position = originalPosition + new Vector3(offsetX, offsetY, 0f);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.position = originalPosition;
    }
}
