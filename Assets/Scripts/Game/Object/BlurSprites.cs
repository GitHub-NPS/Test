using MEC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlurSprites : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] srs;
    private Dictionary<int,Color> colors = new Dictionary<int, Color>();

#if UNITY_EDITOR
    private void OnValidate()
    {
        UpdateList();
    }

    public void UpdateList()
    {
        srs = GetComponentsInChildren<SpriteRenderer>();
    }
#endif

    private void Awake()
    {
        foreach (var sr in srs)
        {
            colors.Add(sr.GetHashCode(), sr.color);
        }
    }

    public void OnBlur()
    {
        foreach (var sr in srs)
        {
            Color color = colors[sr.GetHashCode()];
            sr.color = new Color(color.r, color.g, color.b, color.a / 2);
        }
    }

    public void OffBlur()
    {
        foreach (var sr in srs)
        {
            sr.color = colors[sr.GetHashCode()];
        }
    }
}
