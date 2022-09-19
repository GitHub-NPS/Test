using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoolArrayAccessor : ValueAccessor<bool[]> {
    public bool[] Get(string value) {
        var tmp = value.Split(',');
        var myBool = new bool[tmp.Length];
        for (var i = 0; i < tmp.Length; i++) {
            myBool[i] = bool.Parse(tmp[i].Trim());
        }

        return myBool;
    }
}