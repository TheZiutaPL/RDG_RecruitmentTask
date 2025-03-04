using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LeaderboardsUI : MonoBehaviour
{
    [SerializeField] private GameObject noRunsObject;
    [SerializeField] private LeaderboardRunDisplay[] runDisplayObjects = new LeaderboardRunDisplay[0];

    private List<SaveUnit> saveUnits;

    [Header("Navigation")]
    [SerializeField] private GameObject pageNavigationObject;
    [SerializeField] private Button previousPageButton;
    [SerializeField] private Button nextPageButton;
    [SerializeField] private TextMeshProUGUI pageIndexText;
    private int currentPage;
    private void SetPage(int page)
    {
        int pageBound = GetMaxPageIndex() + 1;
        currentPage = (pageBound + page) % pageBound;

        RefreshLeaderboards();
    }
    private int GetMaxPageIndex() => GetPageIndexFromUnitIndex(saveUnits.Count - 1);
    private int GetPageIndexFromUnitIndex(int unitIndex) => unitIndex / runDisplayObjects.Length;
    private void NextPage() => SetPage(currentPage + 1);
    private void PreviousPage() => SetPage(currentPage - 1);

    private void Awake()
    {
        previousPageButton.onClick.AddListener(PreviousPage);
        nextPageButton.onClick.AddListener(NextPage);
    }

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
        int pageIndex = lastAddedIndex < 0 ? currentPage : GetPageIndexFromUnitIndex(lastAddedIndex);
        currentPage = Mathf.Clamp(pageIndex, 0, GetMaxPageIndex());

        //Show or hide noRunsObject
        noRunsObject.SetActive(saveUnits.Count == 0);

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

        RefreshPageNavigation();
    }

    private void RefreshPageNavigation()
    {
        pageNavigationObject.SetActive(saveUnits.Count > runDisplayObjects.Length);

        int maxPageIndex = GetMaxPageIndex();

        //Set buttons interactability
        previousPageButton.interactable = currentPage > 0;
        nextPageButton.interactable = currentPage < maxPageIndex;

        pageIndexText.SetText($"{currentPage + 1}/{maxPageIndex + 1}");
    }
}
