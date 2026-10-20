using System.Collections;
using UnityEngine;

public class Approaching_Water : MonoBehaviour
{
    [Header("Puddle Growth")]
    public Vector3 targetScale = new Vector3(5f, 5f, 1f);

    [Min(0.01f)]
    public float duration = 10f;

    [Header("Water Collision")]
    public CircleCollider2D waterCollider;

    [Header("Water Animation")]
    public Animator waterAnimator;
    public SpriteRenderer waterSpriteRenderer;

    private Vector3 initialScale;
    private Vector3 initialColliderScale;
    private Coroutine growthCoroutine;
    private bool waterStopped = false;

    void Start()
    {
        initialScale = transform.localScale;

        if (waterCollider != null)
        {
            initialColliderScale =
                waterCollider.transform.localScale;
        }

        growthCoroutine = StartCoroutine(GrowingMaskRoutine());
    }

    IEnumerator GrowingMaskRoutine()
    {
        float elapsedTime = 0f;

        if (duration <= 0f)
        {
            SetGrowth(1f);
            growthCoroutine = null;
            yield break;
        }

        while (elapsedTime < duration && !waterStopped)
        {
            elapsedTime += Time.deltaTime;

            float t = Mathf.Clamp01(elapsedTime / duration);
            SetGrowth(t);

            yield return null;
        }

        if (!waterStopped)
            SetGrowth(1f);

        growthCoroutine = null;
    }

    private void SetGrowth(float t)
    {
        Vector3 currentScale = Vector3.Lerp(
            initialScale, targetScale, t
        );

        transform.localScale = currentScale;

        if (waterCollider == null)
            return;

        float scaleX = currentScale.x /
            Mathf.Max(Mathf.Abs(initialScale.x), 0.001f);

        float scaleY = currentScale.y /
            Mathf.Max(Mathf.Abs(initialScale.y), 0.001f);

        waterCollider.transform.localScale = new Vector3(
            initialColliderScale.x * scaleX,
            initialColliderScale.y * scaleY,
            initialColliderScale.z
        );
    }

    public void StopWater()
    {
        if (waterStopped)
            return;

        waterStopped = true;

        // Stop mask and hitbox growth.
        if (growthCoroutine != null)
        {
            StopCoroutine(growthCoroutine);
            growthCoroutine = null;
        }

        // Stop and hide the animated water sprite.
        if (waterAnimator != null)
            waterAnimator.enabled = false;

        if (waterSpriteRenderer != null)
            waterSpriteRenderer.enabled = false;

        Debug.Log("Water growth and animation stopped.");
    }
}