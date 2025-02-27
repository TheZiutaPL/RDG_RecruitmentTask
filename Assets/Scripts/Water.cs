using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Water : MonoBehaviour
{
    [SerializeField] private Transform followedPosition;

    private SpriteRenderer sr;
    [SerializeField] private float framesPerSecond = 4;
    [SerializeField] private Sprite[] animationFrames = new Sprite[0];
    private float timer;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        HandlePosition();

        HandleAnimation();
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

    private void HandleAnimation()
    {
        if (animationFrames.Length == 0)
            return;

        //calculates current animation frame
        timer += Time.deltaTime * framesPerSecond;
        timer %= animationFrames.Length;

        sr.sprite = animationFrames[Mathf.FloorToInt(timer)];
    }
}
