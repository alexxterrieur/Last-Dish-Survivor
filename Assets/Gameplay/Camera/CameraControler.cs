using System.Collections;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
    Vector3 originalLocalPosition;

    public IEnumerator CameraShake(float duration, float magnitude)
    {
        originalLocalPosition = transform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = originalLocalPosition + new Vector3(offsetX, offsetY, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalLocalPosition;
    }
}
