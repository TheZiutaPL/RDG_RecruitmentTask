using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantSpriteAnimation : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float framesPerSecond = 4;
    [SerializeField] private Sprite[] animationFrames = new Sprite[0];
    private float timer;

    void Update()
    {
        HandleAnimation();
    }

    private void HandleAnimation()
    {
        if (animationFrames.Length == 0)
            return;

        //calculates current animation frame
        timer += Time.deltaTime * framesPerSecond;
        timer %= animationFrames.Length;

        spriteRenderer.sprite = animationFrames[Mathf.FloorToInt(timer)];
    }
}
