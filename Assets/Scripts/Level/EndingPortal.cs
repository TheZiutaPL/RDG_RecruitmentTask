using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingPortal : MonoBehaviour
{
    private const string PORTAL_TOGGLE_BOOL = "isOn";
    [SerializeField] private Animator animator;
    [SerializeField] private Interactable portalInteractable;
    [SerializeField] private AudioClip portalOpenSound;
    private bool isOpen = false;

    private void Start()
    {
        GameManager.Instance.OnRequiredCoinsGathered += () =>
        {
            if (isOpen)
                return;

            isOpen = true;

            //Sets portal as a player goal
            List<Transform> portalGoal = new List<Transform>();
            portalGoal.Add(transform);
            GameManager.Instance.SetPlayerGoals(portalGoal);

            AudioManager.Instance.PlaySFX(portalOpenSound);

            TogglePortal(true);
        };
    }

    private void TogglePortal(bool toggle)
    {
        animator.SetBool(PORTAL_TOGGLE_BOOL, toggle);
        portalInteractable.isInteractable = toggle;
    }

    public void UsePortal()
    {
        portalInteractable.isInteractable = false;

        //Ends Game
        GameManager.Instance.EndGame();

        //Saves scores
        SaveUnit newSaveUnit = new SaveUnit(PlayerStats.Instance.TimePassed, PlayerStats.Instance.Coins, PlayerStats.Instance.Deaths, LevelGenerator.LastGeneratedSeed);
        SaveSystem.SaveFile.AddSaveUnit(newSaveUnit);
        SaveSystem.SaveData();

        //Fade and return to main menu
        UITransition.ToggleTransition(true, () => SceneManager.LoadScene(0));
    }
}
