using UnityEngine;


[DisallowMultipleComponent]
public class PlayerTankCleaner : MonoBehaviour
{
    public RoomCleaningTracker room;
    [Min(0.1f)] public float cleanRange = 1.25f;
    [Min(0.1f)] public float cleanDuration = 2f;
    public KeyCode cleanKey = KeyCode.C;
    [Header("Cleaning start sound (replace the placeholder clip here)")]
    public AudioSource cleaningAudio;
    public AudioClip cleaningStartClip;
    [Range(0f, 1f)] public float cleaningVolume = 0.5f;
    public CleanableTank NearbyTank { get; private set; }
    public float Progress => Mathf.Clamp01(heldSeconds / Mathf.Max(0.1f, cleanDuration));
    public bool IsCleaning => heldSeconds > 0f;
    public bool CanClean => NearbyTank != null && !NearbyTank.IsClean;
    private float heldSeconds;
    private bool mustRelease;
    private bool hasFocus = true;
    private PlayerMoveAround movement;
    private PlayerHealth health;

    private void Awake()
    {
        movement = GetComponent<PlayerMoveAround>();
        health = GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        StepCleaning(Input.GetKey(cleanKey), Time.deltaTime);
    }

    // Kept separate so the same interaction can be checked without fake key input.
    public void StepCleaning(bool keyHeld, float deltaTime)
    {
        if (!keyHeld) mustRelease = false;
        bool allowed = hasFocus && Time.timeScale > 0f &&
            (movement == null || movement.isAlive) &&
            (health == null || (health.currentHealth > 0 && !health.IsBeingKnockedBack));
        CleanableTank next = allowed && room != null
            ? room.NearestTank(transform.position, cleanRange) : null;
        if (next != NearbyTank) heldSeconds = 0f;
        NearbyTank = next;
        if (!allowed || !CanClean || !keyHeld || mustRelease)
        {
            heldSeconds = 0f;
            StopCleaningSound();
            return;
        }
        if (heldSeconds == 0f && deltaTime > 0f && cleaningAudio != null && cleaningStartClip != null)
        {
            cleaningAudio.Stop();
            cleaningAudio.PlayOneShot(cleaningStartClip, cleaningVolume);
        }
        heldSeconds += Mathf.Max(0f, deltaTime);
        if (heldSeconds >= Mathf.Max(0.1f, cleanDuration))
        {
            NearbyTank.CompleteCleaning();
            heldSeconds = 0f;
            mustRelease = true; // another tank needs a fresh hold
            StopCleaningSound();
        }
    }

    private void OnApplicationFocus(bool focused)
    {
        hasFocus = focused;
        if (!focused) { heldSeconds = 0f; mustRelease = true; StopCleaningSound(); }
    }

    private void OnDisable()
    {
        heldSeconds = 0f;
        NearbyTank = null;
        mustRelease = true;
        StopCleaningSound();
    }

    private void StopCleaningSound()
    {
        if (cleaningAudio != null) cleaningAudio.Stop();
    }
}
