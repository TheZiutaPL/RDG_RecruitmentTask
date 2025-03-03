using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionEventAnimationTrigger : MonoBehaviour
{
    public void TriggerInteractableEvent() => PlayerInteraction.Instance.TriggerInteractableEvent();
}
