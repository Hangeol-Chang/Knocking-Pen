using UnityEngine;

// Squashes the spring sprite as the pen moves down.
public class SpringScale : MonoBehaviour
{
    public GameObject pen;

    void Update() => transform.localScale = new Vector3(1, 1 + pen.transform.position.y / 5f, 1);
}
