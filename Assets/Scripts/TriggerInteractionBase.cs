using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class TriggerInteractionBase : MonoBehaviour, IInteractable
{
    public GameObject Player { get ; set; }
    public bool CanInteract { get ; set; }

    public virtual void Interact()
    {
        
    }
    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (CanInteract)
        {
            if (UserInput.WasInteractPressed)
            {
                Interact();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == Player)
        {
            CanInteract = true;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == Player)
        {
            CanInteract = false;
        }

    }

}
