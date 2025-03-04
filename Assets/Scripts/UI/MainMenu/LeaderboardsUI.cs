using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardsUI : MonoBehaviour
{
    [SerializeField] private GameObject noRunsObject;
    [SerializeField] private LeaderboardRunDisplay[] runDisplayObjects = new LeaderboardRunDisplay[0];
    [SerializeField] private GameObject pageNavigationObject;

    private List<SaveUnit> saveUnits;

    private int currentPage;
    private int GetPageIndexFromUnitIndex(int unitIndex) => unitIndex / runDisplayObjects.Length;

    private void OnEnable()
    {
        RefreshLeaderboards();
    }

    private void RefreshLeaderboards()
    {
        if (runDisplayObjects.Length == 0)
            return;

        //Get saved runs
        saveUnits = SaveSystem.SaveFile.GetSaveUnits();

        //Bounds page to bounds of a list
        int lastAddedIndex = SaveSystem.SaveFile.GetLastAddedSaveUnitIndex();
        currentPage = Mathf.Clamp(GetPageIndexFromUnitIndex(lastAddedIndex), 0, GetPageIndexFromUnitIndex(saveUnits.Count - 1));

        //Show or hide noRunsObject
        noRunsObject.SetActive(saveUnits.Count == 0);

        pageNavigationObject.SetActive(saveUnits.Count > runDisplayObjects.Length);

        //Shows and hides displays
        int turnedDisplays = Mathf.Min(saveUnits.Count - currentPage * runDisplayObjects.Length, runDisplayObjects.Length);
        for (int i = 0; i < runDisplayObjects.Length; i++)
        {
            LeaderboardRunDisplay display = runDisplayObjects[i];

            //Turns displays active
            bool turned = i < turnedDisplays;
            display.gameObject.SetActive(turned);

            if (!turned)
                continue;

            //Set display to unit data
            int saveUnitIndex = currentPage * runDisplayObjects.Length + i;
            display.SetRunDisplay(saveUnits[saveUnitIndex], saveUnitIndex, saveUnitIndex == lastAddedIndex);
        }
    }
}
