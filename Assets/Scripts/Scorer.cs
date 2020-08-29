using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Scorer : MonoBehaviour
{
    public TMP_Text text;

    private void Update()
    {

        int totalSeconds = (int) Time.timeSinceLevelLoad;
        this.text.text = $"{totalSeconds:D5}";
    }
}
