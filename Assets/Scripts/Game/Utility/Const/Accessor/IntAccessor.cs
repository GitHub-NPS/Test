using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntAccessor : ValueAccessor<int> {
    public int Get(string value) {
        return int.Parse(value);
    }
}