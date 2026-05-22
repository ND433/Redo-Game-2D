using UnityEngine;
using System.Collections;
using TMPro;

public class SecondsCounter : MonoBehaviour
{
    [Header("UI Setup")]
    [Tooltip("Drag your TextMeshPro UI component here from the scene Hierarchy")]
    public TextMeshProUGUI counterText;

    [Header("Settings")]
    [Tooltip("Optional text to show before the number (e.g. 'Time: ')")]
    public string textPrefix = "";

    private int totalSeconds = 0;

    void Start()
    {
        UpdateTextDisplay();

        StartCoroutine(CountSecondsRoutine());
    }

    private IEnumerator CountSecondsRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            totalSeconds++;

            UpdateTextDisplay();
        }
    }

    private void UpdateTextDisplay()
    {
        if (counterText != null)
        {
            counterText.text = textPrefix + totalSeconds.ToString();
        }
    }
}