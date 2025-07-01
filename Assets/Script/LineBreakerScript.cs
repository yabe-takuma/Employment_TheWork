using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LineBreakerScript : MonoBehaviour
{
    [SerializeField] private TMP_Text textMeshPro;
    [SerializeField] private string originalText;
    [SerializeField] private int breakInterval = 8;

    void Start()
    {
        textMeshPro.text = InsertLineBreaks(originalText, breakInterval);
    }

    string InsertLineBreaks(string input, int interval)
    {
        if (string.IsNullOrEmpty(input) || interval <= 0) return input;

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        for (int i = 0; i < input.Length; i++)
        {
            sb.Append(input[i]);
            if ((i + 1) % interval == 0 && i != input.Length - 1)
            {
                sb.Append('\n');
            }
        }
        return sb.ToString();
    }


}
