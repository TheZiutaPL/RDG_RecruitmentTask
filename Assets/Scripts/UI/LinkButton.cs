using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LinkButton : MonoBehaviour
{
    [SerializeField] private string url;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OpenUrl);
    }

    public void OpenUrl()
    {
        if (string.IsNullOrWhiteSpace(url))
            return;

        Application.OpenURL(url);
    }
}
