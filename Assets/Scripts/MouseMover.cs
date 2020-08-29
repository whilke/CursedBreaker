using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMover : MonoBehaviour
{
    public SpriteRenderer Sprite;

    // Update is called once per frame
    void Update()
    {
        this.SetOverMouse();
    }

    public void SetOverMouse()
    {
        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0f;
        this.transform.position = pos;

    }
}
