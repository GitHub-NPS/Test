using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UserTierDefinition.Runtime
{
    [System.Serializable]
    public class ResponseModel
    {
        public int status;
        public string message;
        public UserTierModel data;
    }
}