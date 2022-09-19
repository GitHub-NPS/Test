using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class FloatRandomAccessor : ValueAccessor<float> {
    public float Get(string value) {
        var tmp = value.Split('-');
        var min = float.Parse(tmp[0].Trim(), CultureInfo.InvariantCulture.NumberFormat);
        var max = float.Parse(tmp[0].Trim(), CultureInfo.InvariantCulture.NumberFormat);
        return Random.Range(min, max);
    }
}