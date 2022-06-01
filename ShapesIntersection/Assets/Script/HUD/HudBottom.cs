using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HudBottom : MonoBehaviour
{
    public TextMeshProUGUI _text;

    void Update()
    {
        _text.text = "Result: "+Data.result;
    }
}
