using UnityEngine;

public class TriggerTest : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("TRIGGER TEST SCRIPT STARTED");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("SPACE KEY DETECTED");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("TRIGGER WORKS: " + other.name);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("COLLISION: " + collision.gameObject.name);
    }
}