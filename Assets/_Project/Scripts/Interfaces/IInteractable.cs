using UnityEngine;

namespace Game.Interfaces
{
    public interface IInteractable
    {
        void Interact(GameObject source);
        string GetPrompt();
    }
}
