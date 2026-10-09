using UnityEngine;

public class FollowPosition : MonoBehaviour
{
    public GameObject target;

    void Update() => transform.position = target.transform.position;
}
