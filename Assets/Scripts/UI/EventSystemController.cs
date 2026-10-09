using UnityEngine;
using UnityEngine.EventSystems;

// Keeps the first EventSystem across scene loads and drops the copies each scene brings.
public class EventSystemController : MonoBehaviour
{
    void Awake()
    {
        if (FindObjectsByType<EventSystem>(FindObjectsSortMode.None).Length == 1) DontDestroyOnLoad(gameObject);
        else Destroy(gameObject);
    }
}
