using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform followedObject;
    [SerializeField] private float cameraSmoothing = .1f;
    private Vector3 velocity;

    private void LateUpdate()
    {
        Vector3 desiredPosition = new Vector3(followedObject.position.x, followedObject.position.y, transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, cameraSmoothing);
    }
}
