using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [SerializeField] private string interactableName;
    public string GetInteractableName() => interactableName;

    [SerializeField, Tooltip("Press [] to ____ .")] private string interactionTextSufix = "use";
    public string GetInteractionTextSufix() => interactionTextSufix;

    [SerializeField] private string interactionAnimationTrigger;
    public string GetInteractionAnimationTrigger() => interactionAnimationTrigger;

    [SerializeField] private UnityEvent onInteraction;
    public UnityEvent GetOnInteractionEvent() => onInteraction;
}
