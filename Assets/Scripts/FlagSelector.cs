using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagSelector : MonoBehaviour
{
    public bool Red;
    public bool Green;
    public bool Yellow;
    public bool Blue;

    private void Awake()
    {
        var animator = this.GetComponentInChildren<Animator>();
        animator.SetBool("Blue", this.Blue);
        animator.SetBool("Red", this.Red);
        animator.SetBool("Yellow", this.Yellow);
        animator.SetBool("Green", this.Green);
    }
}
