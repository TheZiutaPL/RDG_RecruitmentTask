using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class PlayerSaveName
{
    public const int MAX_NAME_CHARACTERS = 15;
    public static string PlayerName { get; private set; }
    public static Action<string> OnNameChanged;
    public static void SetPlayerName(string newName)
    {
        if (newName == PlayerName)
            return;

        //Limits name characters
        newName = newName.Substring(0, Mathf.Min(newName.Length, PlayerSaveName.MAX_NAME_CHARACTERS));

        //Sets name
        PlayerName = newName;
        OnNameChanged?.Invoke(newName);
    }
}
