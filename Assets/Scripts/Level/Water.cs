using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField] private Transform followedPosition;

    private void LateUpdate()
    {
        HandlePosition();
    }

    private void HandlePosition()
    {
        if (followedPosition == null)
            return;

        transform.position = new Vector3(
            Mathf.Round(followedPosition.position.x),
            Mathf.Round(followedPosition.position.y),
            Mathf.Round(followedPosition.position.z));
    }
}
