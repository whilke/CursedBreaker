using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatDeplay : MonoBehaviour
{
    private float deltaTime = 0f;
    private int characterCount = 0;

    private void Update()
    {
        this.deltaTime += (Time.unscaledDeltaTime - this.deltaTime) * 0.1f;
        this.characterCount = GameObject.FindObjectOfType<CharacterSpawner>().CharacteCount();
    }

    private void OnGUI()
    {
        int w = Screen.width;
        int h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0,0, w, h*2/100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 100;
        style.normal.textColor = new Color(0f,0f,0.5f, 1f);
        float msc = this.deltaTime * 1000f;
        float fps = 1.0f / this.deltaTime;
        string text = $"{msc:0.0} ms ({fps:0.} fps) - characters: {this.characterCount}";
        GUI.Label(rect, text, style);
    }
}
