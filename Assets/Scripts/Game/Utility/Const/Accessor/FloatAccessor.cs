using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class FloatAccessor : ValueAccessor<float> {
    public float Get(string value) {
        return float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
    }
}