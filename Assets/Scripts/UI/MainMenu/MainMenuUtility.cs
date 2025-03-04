using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUtility : MonoBehaviour
{
    public void Play() => UITransition.ToggleTransition(true, () => SceneManager.LoadScene(1));

    public void Exit() => UITransition.ToggleTransition(true, () => Application.Quit());
}
