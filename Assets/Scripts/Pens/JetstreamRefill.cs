using UnityEngine;

// Jetstream refill: follows the pen body, but stays down while the knocked button is tapped.
public class JetstreamRefill : MonoBehaviour
{
    public KnockPen pen;
    public float followBelow = -1.7f;

    void Update()
    {
        float y = pen.transform.position.y;
        if (!pen.ClickPassed || (pen.IsKnocked && y < followBelow))
            transform.position = new Vector3(0, y, 0);
    }
}
