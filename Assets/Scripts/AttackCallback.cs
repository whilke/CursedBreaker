using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackCallback : MonoBehaviour
{
    public UnityEvent Callback;

    public void OnHit()
    {
        this.Callback?.Invoke();
    }
}
