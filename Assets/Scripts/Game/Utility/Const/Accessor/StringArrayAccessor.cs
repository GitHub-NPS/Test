using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringArrayAccessor : ValueAccessor<string[]> {
    public string[] Get(string value) {
        var tmp = value.Split(',');
        var myString = new string[tmp.Length];
        for (var i = 0; i < tmp.Length; i++) {
            myString[i] = tmp[i];
        }

        return myString;
    }
}