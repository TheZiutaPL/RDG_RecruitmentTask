using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static GameInputs GameInputs { get; private set; }

    private void Awake()
    {
        GameInputs = new GameInputs();
    }

    private void OnEnable()
    {
        GameInputs.Enable();
    }

    private void OnDisable()
    {
        GameInputs.Disable();
    }
}
