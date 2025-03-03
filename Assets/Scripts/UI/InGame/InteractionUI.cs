using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractionUI : MonoBehaviour
{
    [SerializeField] private float uiYOffset;
    [SerializeField] private TextMeshProUGUI interactableNameText;
    [SerializeField] private TextMeshProUGUI interactionText;

    [SerializeField] private string interactionTextPrefix;

    private Transform currentInteractableTransform;

    private void Start()
    {
        PlayerInteraction.Instance.OnInteractableChanged += HandleNewInteractable;
        gameObject.SetActive(false);
    }

    private void Update()
    {
        UpdateUIPosition();
    }

    private void HandleNewInteractable(Interactable interactable)
    {
        //Checks if it is null
        bool isNull = interactable == null;
        gameObject.SetActive(!isNull);

        if (isNull)
            return;

        currentInteractableTransform = interactable.transform;
        UpdateUIPosition();

        //Sets ui text values
        interactableNameText.SetText(interactable.GetInteractableName());
        interactionText.SetText($"{interactionTextPrefix} {interactable.GetInteractionTextSufix()}.");
    }

    private void UpdateUIPosition()
    {
        transform.position = Camera.main.WorldToScreenPoint(currentInteractableTransform.position) + new Vector3(0, uiYOffset);
    }
}
