using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuUtility : MonoBehaviour
{
    public void ResumeGame() => UIManagerMain.Instance.SetUIScreenToDefault();
    public void ReturnToMainMenu() => UITransition.ToggleTransition(true, () => { Time.timeScale = 1; SceneManager.LoadScene(0); });
}
