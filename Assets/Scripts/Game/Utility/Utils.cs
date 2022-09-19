using MEC;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public static class Utils
{
    public static bool FindLayer(this int layer, params LayerInt[] layers)
    {
        for (int i = 0; i < layers.Length; i++)
        {
            if (layer == (int)layers[i]) return true;
        }
        return false;
    }

    public static int FindLayer(params LayerBit[] layers)
    {
        int result = 0;
        for (int i = 0; i < layers.Length; i++)
        {
            result += (int)layers[i];
        }
        return result;
    }

    public static List<ITag> GetObject<T>(this RaycastHit2D[] rs, List<ObjectTag> tags, int limit = -1)
    {
        int count = 0;
        List<ITag> objs = new List<ITag>();

        for (int i = 0; i < rs.Length; i++)
        {
            T t = GetObject<T>(rs[i].collider, tags);
            if (t != null)
            {
                count++;
                if (limit != -1 && count > limit)
                {
                    break;
                }

                objs.Add(rs[i].collider.GetComponent<ITag>());
            }
        }

        return objs;
    }

    public static List<ITag> GetObject<T>(this Collider2D[] cl, List<ObjectTag> tags, int limit = -1)
    {
        int count = 0;
        List<ITag> objs = new List<ITag>();

        for (int i = 0; i < cl.Length; i++)
        {
            T t = GetObject<T>(cl[i], tags);
            if (t != null)
            {
                count++;
                if (limit != -1 && count > limit)
                {
                    break;
                }

                objs.Add(cl[i].GetComponent<ITag>());
            }
        }

        return objs;
    }

    public static T GetObject<T>(this Collider2D cl, List<ObjectTag> tags)
    {
        ITag iTag = cl.GetComponent<ITag>();
        if (iTag != null && tags.Contains(iTag.GetTag()))
        {
            T t = cl.GetComponent<T>();
            if (t != null)
            {
                return t;
            }
        }

        return default;
    }

    public static Vector3 GetRandomPosition(Transform from, float radius)
    {
        return GetRandomPosition(from, from.position, radius);
    }

    public static Vector3 GetRandomPosition(Transform trans, Vector3 start, float radius)
    {
        NavMeshHit hit;
        float x = 0;
        float y = 0;

        int count = 0;
        int max = 1000;
        while (trans)
        {
            float angle = Random.Range(0, 2 * Mathf.PI);
            x = radius * Mathf.Cos(angle) + start.x;
            y = radius * Mathf.Sin(angle) + start.y;

            if (NavMesh.SamplePosition(new Vector3(x, y), out hit, 0.5f, NavMesh.AllAreas))
            {
                return hit.position;
            }

            count++;
            if (count > max) {
                Debug.LogWarning("Not Get Random Position Agent");
                radius -= 1;
                count = 0;
                if (radius == 0) max = 1;
                if (radius < 0)
                {
                    x = start.x; y = start.y; break;
                }
            }
        }
        return new Vector3(x, y, 0);
    }

    public static Vector2 GetCamPosition(int x, int y)
    {
        return ProCameraManager.S.Camera.ScreenToWorldPoint(new Vector3(x, y, ProCameraManager.S.Camera.nearClipPlane));
    }

    public static void MergeData(this List<LootData> rewards, LootData reward)
    {
        if (reward.Type == LootType.Currency)
        {
            int idx = rewards.FindIndex(x => x.Type == reward.Type && ((CurrencyData)x.Data).Type == ((CurrencyData)reward.Data).Type);
            if (idx != -1)
            {
                CurrencyData data = (CurrencyData)rewards[idx].Data;
                data.Value += ((CurrencyData)reward.Data).Value;
            }
            else rewards.Add(reward);
        }
        else rewards.Add(reward);
    }

    public static Color HtmlToColor(string html)
    {
        ColorUtility.TryParseHtmlString(html, out Color color);
        return color;
    }

    public static int KindOfRankEndGamePvp(int rank)
    {
        return rank >= 3 ? 3 : rank;
    }

    public static string Serialize(this object data)
    {
        return JsonConvert.SerializeObject(data);
    }

    public static T Deserialize<T>(this string data)
    {
        return JsonConvert.DeserializeObject<T>(data);
    }

    public static void Dump(this object data)
    {
        Debug.Log(data.GetType());
        Debug.Log(Serialize(data));
    }

    public static T Parse<T>(this JObject obj)
    {
        return obj.ToObject<T>();
    }

    public static List<T> Parse<T>(this JArray arr)
    {
        List<T> result = new List<T>();
        foreach (JObject obj in arr)
        {
            result.Add(obj.ToObject<T>());
        }

        return result;
    }

    public static void Loot(List<LootData> rewards, string source = "")
    {
        foreach (var item in rewards)
        {
            Loot(item, source);
        }

        DataManager.Save.User.Save();
    }

    public static void Loot(LootData reward, string source = "")
    {
        switch (reward.Type)
        {
            case LootType.Currency:
                var currency = reward.Data as CurrencyData;
                DataManager.Save.User.IncreaseCurrency(currency);
                break;
        }
    }
}
