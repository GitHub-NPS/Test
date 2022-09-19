using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ValueAccessor {
}

public interface ValueAccessor<T> : ValueAccessor {
    T Get(string value);
}