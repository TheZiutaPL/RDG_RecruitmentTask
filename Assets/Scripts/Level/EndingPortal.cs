using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingPortal : MonoBehaviour
{
    private const string PORTAL_TOGGLE_BOOL = "isOn";
    [SerializeField] private Animator animator;
    [SerializeField] private Interactable portalInteractable;

    private void Start()
    {
        GameManager.Instance.OnRequiredCoinsGathered += () => TogglePortal(true);
    }

    private void TogglePortal(bool toggle)
    {
        animator.SetBool(PORTAL_TOGGLE_BOOL, toggle);
        portalInteractable.isInteractable = toggle;
    }

    public void UsePortal()
    {
        //Ends Game
        GameManager.Instance.EndGame();

        //Save scores
        //

        //Spawn "new highscore pass" object, which highlights new highscore in menu
        //

        //Fade and return to main menu
        UITransition.ToggleTransition(true, () => SceneManager.LoadScene(0));
    }
}
