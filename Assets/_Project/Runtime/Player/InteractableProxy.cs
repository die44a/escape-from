using UnityEngine;
using _Project.Runtime.Core.Main.Interfaces;

public class InteractableProxy : MonoBehaviour
{
    public MonoBehaviour target;

    public IInteractable GetInteractable()
    {
        return target as IInteractable;
    }
} 