using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayerFollow : MonoBehaviour
{
    public List<Transform> players;
    public Vector3 offset = new Vector3(1f, 50f, -50f);
    public float zoomOffset = 1.75f;

    public float minZoom = 40f;
    public float maxZoom = 10f;
    public float zoomLimiterX = 6f;
    public float zoomLimiterZ = 100f;



    private float smoothTime = 5f;
    private Vector3 velocity;

    private Camera cam;

    private void Start()
    {
        cam = this.GetComponent<Camera>();
    }


    private void LateUpdate()
    {
        if (players.Count == 0)
            return;

        CameraMove();
        CameraZoom();

        cam.transform.LookAt(new Vector3(GetCenterPoint().x, GetCenterPoint().y + zoomOffset, GetCenterPoint().z));
    }

    private void CameraMove()
    {
        Vector3 centerPoint = GetCenterPoint();
        Vector3 newPosition = centerPoint + offset;

        this.transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    private void CameraZoom()
    {
        float newZoomX = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistanceX() / zoomLimiterX);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoomX, Time.deltaTime);

        //float newZoomZ = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistanceZ() / zoomLimiterZ);
        //cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoomZ, Time.deltaTime);

    }

    private float GetGreatestDistanceX()
    {
        var bounds = new Bounds(players[0].position, Vector3.zero);

        for (int i = 0; i < players.Count; i++)
        {
            bounds.Encapsulate(players[i].position);
        }

        return bounds.size.x;
    }

    private float GetGreatestDistanceZ()
    {
        var bounds = new Bounds(players[0].position, Vector3.zero);

        for (int i = 0; i < players.Count; i++)
        {
            bounds.Encapsulate(players[i].position);
        }

        return bounds.size.z;
    }

    private Vector3 GetCenterPoint()
    {
        if (players.Count == 1)
        {
            return players[0].position;
        }

        var bounds = new Bounds(players[0].position, Vector3.zero);

        for (int i = 0; i < players.Count; i++)
        {
            bounds.Encapsulate(players[i].position);
        }

        return bounds.center;
    }

}
