using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    private static string description;
    public void SetDescription(string s) { description = s; }

    private static Text text;

    private void Start()
    {
        text = GetComponentInChildren<Text>();
        HideTooltip();
    }

    public void DisplayTooltip()
    {
        text.text = description;
        text.color = Color.white;
        text.enabled = true;
    }

    public static void DisplayTooltip(string s)
    {
        description = s;
        text.color = Color.white;
        text.text = description;
        text.enabled = true;
    }

    public static void DisplayTooltip(string s, Color color)
    {
        description = s;
        text.text = description;
        text.color = color;
        text.enabled = true;
    }

    public static void HideTooltip()
    {
        if (text != null)
        {
            text.enabled = false;
        }
    }

    public Text GetText()
    {
        return text;
    }
}
