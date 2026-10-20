using UnityEngine;

public class CleanableTank : MonoBehaviour
{
    [Header("Tank Settings")]
    public string tankName = "Aquarium";
    public bool startsClean;

    [Header("Tank Appearance")]
    public SpriteRenderer tankArt;
    public Color dirtyTint = new Color(0.65f, 0.78f, 0.32f, 1f);
    public Color cleanTint = Color.white;

    [SerializeField] private bool isClean;
    public bool IsClean => isClean;

    [Header("Water")]
    public Approaching_Water waterSystem;

    private Collider2D tankCollider;

    private void Awake()
    {
        tankCollider = GetComponent<Collider2D>();

        isClean = startsClean;
        UpdateAppearance();
    }

    public float DistanceTo(Vector2 position)
    {
        if (tankCollider == null)
            tankCollider = GetComponent<Collider2D>();

        if (tankCollider == null)
            return Vector2.Distance(position, transform.position);

        return Vector2.Distance(
            position,
            tankCollider.ClosestPoint(position)
        );
    }

    public bool CompleteCleaning()
    {
        if (isClean)
            return false;

        isClean = true;
        UpdateAppearance();

        // Stop the water when the tank is cleaned.
        if (waterSystem != null)
            waterSystem.StopWater();

        return true;
    }

    private void UpdateAppearance()
    {
        if (tankArt != null)
            tankArt.color = isClean ? cleanTint : dirtyTint;
    }
}