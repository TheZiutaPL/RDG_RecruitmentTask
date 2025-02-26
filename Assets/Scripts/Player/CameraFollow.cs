using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform followedObject;

    private void LateUpdate()
    {
        transform.position = new Vector3(followedObject.position.x, followedObject.position.y, transform.position.z);
    }
}
