using UnityEngine;

public static class Pointer
{
    public static Vector3 World => Camera.main.ScreenToWorldPoint(Input.mousePosition);
}
