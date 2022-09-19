using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntArrayAccessor : ValueAccessor<int[]> {
    public int[] Get(string value) {
        var tmp = value.Split(',');
        var myInt = new int[tmp.Length];
        for (var i = 0; i < tmp.Length; i++) {
            myInt[i] = int.Parse(tmp[i].Trim());
        }

        return myInt;
    }
}