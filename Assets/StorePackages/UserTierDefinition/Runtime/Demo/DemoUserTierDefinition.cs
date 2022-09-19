using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UserTierDefinition.Runtime {
    public class DemoUserTierDefinition : MonoBehaviour {
        private void Start() {
            Debug.Log(UserTierDefinition.Instance.Tier);
        }
    }
}