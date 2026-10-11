using System.Collections;
using UnityEngine;

public class Approaching_Water : MonoBehaviour
{
    public Vector3 targetScale = new Vector3(5f, 5f, 1f);
    [Min(0.01f)]
    public float duration = 10.0f;

    void Start()
    {
        StartCoroutine(GrowingMaskRoutine());
    }

    IEnumerator GrowingMaskRoutine()
    {
        Vector3 initialScale = transform.localScale;
        float elapsedTime = 0f;

        if (duration <= 0f)
        {
            transform.localScale = targetScale;
            yield break;
        }

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float percentageComplete = elapsedTime / duration;
            transform.localScale = Vector3.Lerp(initialScale, targetScale, percentageComplete);
            yield return null;
        }

        transform.localScale = targetScale;
    }
}
