using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManagerMain : UIManager
{
    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }
}
