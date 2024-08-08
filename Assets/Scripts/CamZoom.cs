using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamZoom : MonoBehaviour
{
    public float zoom;
    public float ZoomMultiplier = 4f;
    public float MinZoom = 2f;
    public float MaxZoom = 8f;
    public float velocity = 0f;
    public float smoothTime = 0.25f;
    public Transform player; 
    public Vector3 offset; 

    [SerializeField] public Camera Cam;

    void Start()
    {
        zoom = Cam.orthographicSize;
    }

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        zoom -= scroll * ZoomMultiplier;
        zoom = Mathf.Clamp(zoom, MinZoom, MaxZoom);
        Cam.orthographicSize = Mathf.SmoothDamp(Cam.orthographicSize, zoom, ref velocity, smoothTime);
    }

    private void LateUpdate()
    {
       
        transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y, transform.position.z);
    }
}
