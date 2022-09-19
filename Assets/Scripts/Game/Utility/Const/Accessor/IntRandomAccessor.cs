using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntRandomAccessor : ValueAccessor<int> {
    public int Get(string value) {
        var tmp = value.Split('-');
        var min = int.Parse(tmp[0].Trim());
        var max = int.Parse(tmp[0].Trim());
        return Random.Range(min, max + 1);
    }
}