using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Config/Game", order = 1)]
public class GameConfig : ScriptableObject
{
	[FoldoutGroup("Common")]
	[FoldoutGroup("Common/Demo")]
	[VerticalGroup("Common/Demo/Element")] public float Demo = 60;
}
