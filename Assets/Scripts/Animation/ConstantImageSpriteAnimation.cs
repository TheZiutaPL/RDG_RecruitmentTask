using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstantImageSpriteAnimation : MonoBehaviour
{
    [SerializeField] private Image image;
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

        image.sprite = animationFrames[Mathf.FloorToInt(timer)];
    }
}
