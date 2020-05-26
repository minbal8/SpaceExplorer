using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnStateEnter_PlaySound : StateMachineBehaviour
{
    public AudioClip audioClip;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Pressed");

        AudioSource audioSource = animator.GetComponent<AudioSource>();

        if(!audioSource.isPlaying)
            audioSource.PlayOneShot(audioClip);
    }
}
