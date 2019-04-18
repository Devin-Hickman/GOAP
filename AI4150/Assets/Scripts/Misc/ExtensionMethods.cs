using UnityEngine;

public static class ExtensionMethods
{
    public static bool IsVisible(this Camera cam, Vector3 pos)
    {
        return cam.WorldToViewportPoint(pos).x > 0 && cam.WorldToViewportPoint(pos).x < 1 &&
            cam.WorldToViewportPoint(pos).y > 0 && cam.WorldToViewportPoint(pos).y < 1;
    }
}
