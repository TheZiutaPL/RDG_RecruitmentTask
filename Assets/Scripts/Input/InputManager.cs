using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    private static InputManager instance;
    public static bool IsNull() => instance == null;

    public static PlayerInput PlayerInput { get; private set; }

    private void Awake()
    {
        if (IsNull())
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        PlayerInput = GetComponent<PlayerInput>();
    }

    public static InputAction GetInputAction(string nameOrId) => PlayerInput.actions.FindAction(nameOrId);

    public static void ToggleInput(bool toggle)
    {
        if (IsNull())
            return;

        instance.gameObject.SetActive(toggle);
    }
}
