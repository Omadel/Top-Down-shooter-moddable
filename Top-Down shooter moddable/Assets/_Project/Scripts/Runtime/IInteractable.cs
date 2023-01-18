public interface IInteractable
{
    bool IsInteractable => true;
    void ShowInteraction();
    void HideInteraction();
    void Interact();
    void OnInteractionEnded();
}