using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class FloatArrayAccessor : ValueAccessor<float[]> {
    public float[] Get(string value) {
        var tmp = value.Split(',');
        var myFloat = new float[tmp.Length];
        for (var i = 0; i < tmp.Length; i++) {
            myFloat[i] = float.Parse(tmp[i].Trim(), CultureInfo.InvariantCulture.NumberFormat);
        }

        return myFloat;
    }
}