using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class PlayerInteraction : MonoBehaviour
{
    public static PlayerInteraction Instance { get; private set; }

    private bool canInteract = true;
    public void ToggleInteraction(bool toggle)
    {
        canInteract = toggle;

        if (toggle)
            RefreshInteraction();
        else
            SetCurrentInteractable(null);
    }

    [SerializeField] private Animator animator;
    [SerializeField] private float interactionRange;
    [SerializeField] private LayerMask interactionLayer;
    [SerializeField] private float interactionRefreshTime = .2f;
    private float refreshTimer;

    private Collider2D[] cols = new Collider2D[5];
    private Interactable currentInteractable;
    public Action<Interactable> OnInteractableChanged;
    private void SetCurrentInteractable(Interactable newInteractable, bool refreshesIfEqual = false)
    {
        if (!refreshesIfEqual && currentInteractable == newInteractable)
            return;

        currentInteractable = newInteractable;

        OnInteractableChanged?.Invoke(newInteractable);
    }

    private UnityEvent animationTriggeredInteractableEvent;
    /// <summary>
    /// Triggers event gained from interactable. Intended to be used as an animation trigger
    /// </summary>
    public void TriggerInteractableEvent()
    {
        if (animationTriggeredInteractableEvent == null)
            return;

        animationTriggeredInteractableEvent?.Invoke();
        animationTriggeredInteractableEvent = null;
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        PlayerEntity.Instance.OnRespawn += () => ToggleInteraction(true);
    }


    private void Update()
    {
        #region Interaction Refresh
        if (refreshTimer > 0)
            refreshTimer -= Time.deltaTime;
        else
        {
            RefreshInteraction();
            refreshTimer = interactionRefreshTime;
        }
        #endregion
    }

    /// <summary>
    /// Determines with which interactable is targetted
    /// </summary>
    private void RefreshInteraction()
    {
        if (!canInteract)
            return;

        int overlapped = Physics2D.OverlapCircleNonAlloc(transform.position, interactionRange, cols, interactionLayer);

        //Gets interactables from colliders
        List<Interactable> interactables = new List<Interactable>();
        for (int i = 0; i < overlapped; i++)
        {
            if (cols[i].TryGetComponent(out Interactable interactable) && interactable.isInteractable)
                interactables.Add(interactable);
        }

        //Sets interactables
        if (interactables.Count == 0)
            SetCurrentInteractable(null);
        else if (interactables.Count == 1)
            SetCurrentInteractable(interactables[0]);
        else
        {
            int closestIndex = 0;
            float closestDistance = interactionRange + 50;

            //Looks for closest interactable
            for (int i = 0; i < interactables.Count; i++)
            {
                float distance = Vector2.Distance(interactables[i].transform.position, transform.position);
                if(distance < closestDistance)
                {
                    closestIndex = i;
                    closestDistance = distance;
                }
            }

            SetCurrentInteractable(interactables[closestIndex]);
        }
    }

    private void Interact()
    {
        if (currentInteractable == null)
            return;

        if (currentInteractable.IsUsingAnimation())
        {
            //Caches interactable and sets current to null
            Interactable temp = currentInteractable;
            
            //Triggers animation and passes events
            animator.SetTrigger(currentInteractable.GetInteractionAnimationTrigger());
            animationTriggeredInteractableEvent = currentInteractable.GetOnInteractionEvent();
        }
        else
            currentInteractable.GetOnInteractionEvent()?.Invoke();

        RefreshInteraction();
    }

    private void OnEnable()
    {
        InputManager.GameInputs.Game.Interaction.performed += _ => Interact();
    }

    private void OnDisable()
    {
        InputManager.GameInputs.Game.Interaction.performed -= _ => Interact();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}
