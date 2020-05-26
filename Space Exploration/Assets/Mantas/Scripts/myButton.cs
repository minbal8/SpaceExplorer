using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class myButton : MonoBehaviour
{
    public Animator interactionAnimator;

    public void Interact()
    {
        if (interactionAnimator.enabled == false)
            interactionAnimator.enabled = true;

        interactionAnimator.SetBool("isOpened", !interactionAnimator.GetBool("isOpened"));
        interactionAnimator.SetTrigger("Pressed");
    }
}
