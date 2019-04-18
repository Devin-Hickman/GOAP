using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomCam : MonoBehaviour
{

    public int defaultCameraSize = 100;
    public int zoomSpeed;
    Camera cam;

    public int minCameraSize = 40;
    public int maxCameraSize = 200;
    // Used to allow ui buttons to zoom

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void Start()
    {
        ResetZoomScale();
    }

    void Update()
    {
        if (Input.mouseScrollDelta.y != 0 && cam.IsVisible(cam.ScreenToWorldPoint(Input.mousePosition)))
        {
            ZoomCamera(Input.mouseScrollDelta.y);
        }
    }


    private void ZoomCamera(float scrollDelta)
    {
        int tmp = scrollDelta > 0 ? -zoomSpeed : zoomSpeed;
        float oldSize = cam.orthographicSize;
        cam.orthographicSize += tmp;
        //Cap the minimum size
        cam.orthographicSize = Mathf.Max(minCameraSize, cam.orthographicSize);
        //Cap the maximum size
        cam.orthographicSize = Mathf.Min(maxCameraSize, cam.orthographicSize);
    }

    public void ResetZoomScale()
    {
        cam.orthographicSize = defaultCameraSize;
    }
}
