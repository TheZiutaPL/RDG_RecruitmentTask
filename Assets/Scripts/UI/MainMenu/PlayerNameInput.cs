using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerNameInput : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInputField;

    void Start()
    {
        nameInputField.onValueChanged.AddListener(PlayerSaveName.SetPlayerName);
    }

    private void OnEnable()
    {
        nameInputField.SetTextWithoutNotify(PlayerSaveName.PlayerName);
        PlayerSaveName.OnNameChanged += nameInputField.SetTextWithoutNotify;
    }

    private void OnDisable()
    {
        PlayerSaveName.OnNameChanged -= nameInputField.SetTextWithoutNotify;
    }
}
