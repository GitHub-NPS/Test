using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoolAccessor : ValueAccessor<bool> {
    public bool Get(string value) {
        return bool.Parse(value);
    }
}