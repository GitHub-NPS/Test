using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

// This component is for use with TextMeshPro. If you're not using TextMeshPro, do delete it.

namespace PlayfulSystems.ProgressBar {
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class BarViewValueTextMeshPro : ProgressBarProView {
        [SerializeField] TextMeshProUGUI text;
        [SerializeField] string prefix = "";
        [SerializeField] float minValue = 0f;
        [SerializeField] public float maxValue = 100f;
        [SerializeField] int numDecimals = 0;
        [SerializeField] bool showMaxValue = false;
        [SerializeField] string numberUnit = "%";
        [SerializeField] string suffix = "";

        private float lastDisplayValue;

        public override bool CanUpdateView(float currentValue, float targetValue) {
            float displayValue = GetRoundedDisplayValue(currentValue);

            if (currentValue >= 0f && Mathf.Approximately(lastDisplayValue, displayValue))
                return false;

            lastDisplayValue = GetRoundedDisplayValue(currentValue);
            return true;
        }

        public override void UpdateView(float currentValue, float targetValue) {
            text.text = prefix + FormatNumber(GetRoundedDisplayValue(currentValue)) + numberUnit +
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
        protected override void Reset() {
            base.Reset();
            text = GetComponent<TextMeshProUGUI>();
        }
#endif
    }
}