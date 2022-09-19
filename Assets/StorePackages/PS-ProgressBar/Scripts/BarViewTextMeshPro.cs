using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class BarViewTextMeshPro : MonoBehaviour {
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] string prefix = "";
    [SerializeField] float minValue = 0f;
    [SerializeField] public float maxValue = 100f;
    [SerializeField] int numDecimals = 0;
    [SerializeField] bool showMaxValue = false;
    [SerializeField] string numberUnit = "%";
    [SerializeField] string suffix = "";

    private float lastDisplayValue;

    public TextMeshProUGUI Text => text;
    
    public bool CanUpdateView(float currentValue, float targetValue) {
        float displayValue = GetRoundedDisplayValue(currentValue);

        if (currentValue >= 0f && Mathf.Approximately(lastDisplayValue, displayValue))
            return false;

        lastDisplayValue = GetRoundedDisplayValue(currentValue);
        return true;
    }

    public void UpdateView(float currentValue, float targetValue) {
        maxValue = targetValue;
        text.text = prefix + FormatNumber(currentValue) + numberUnit +
                    (showMaxValue ? "/" + FormatNumber(maxValue) + numberUnit : "") + suffix;
    }

    float GetDisplayValue(float num) {
        return Mathf.Lerp(minValue, maxValue, num);
    }

    float GetRoundedDisplayValue(float num) {
        float value = GetDisplayValue(num);

        if (numDecimals == 0)
            return Mathf.Round(value);

        float multiplier = Mathf.Pow(10, numDecimals);
        value = Mathf.Round(value * multiplier) / multiplier;

        return value;
    }

    string FormatNumber(float num) {
        return num.ToString("N" + numDecimals);
    }

#if UNITY_EDITOR
    protected void Reset() {
        text = GetComponent<TextMeshProUGUI>();
    }
#endif
}