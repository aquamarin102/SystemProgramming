using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class TextField : MonoBehaviour
{
    private const int MAX_LIST_COUNT = 7;

    [SerializeField]
    private TextMeshProUGUI textObject;
    [SerializeField]
    private Scrollbar scrollbar;
    private List<string> messages = new List<string>();
    private void Start()
    {
        scrollbar.onValueChanged.AddListener((float value) => UpdateText());
    }

    private void ClampList(int maxCount)
    {
        while (messages.Count > maxCount)
        {
            messages.RemoveAt(0);
        }
    }

    public void ReceiveMessage(object message)
    {
        messages.Add(message.ToString());
        ClampList(MAX_LIST_COUNT);
        //float value = (messages.Count - 1) * scrollbar.value;
        //scrollbar.value = Mathf.Clamp(value, 0, 1);
        UpdateText();
    }
    private void UpdateText()
    {
        string text = "";
        //int index = (int)(messages.Count * scrollbar.value);
        for (int i = 0; i < messages.Count; i++)
        {
            text += messages[i] + "\n";
        }
        textObject.text = text;
    }
}