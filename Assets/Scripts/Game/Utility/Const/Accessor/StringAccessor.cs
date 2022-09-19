using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringAccessor : ValueAccessor<string> {
    public string Get(string value) {
        return value;
    }
}