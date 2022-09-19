using System.Collections;
using UnityEngine;
using NPS;
using UnityEngine.Networking;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;

public class ApiManager : MonoSingleton<ApiManager>
{
    private const string BaseUrl = "https://zombie-blade.unimob.com.vn/api/";

    private const string UserUrl = BaseUrl + "user";
    private const string ArenaUrl = BaseUrl + "arena";

    private UserSave user;

    public void Init(Transform parent = null)
    {
        AppManager.Api = this;
        if (parent) transform.SetParent(parent);

        user = DataManager.Save.User;
    }

    public void GetArenaTop(Action<Package> result)
    {
        var url = ArenaUrl + $"/top?id={user.id}";
        StartCoroutine(_GetData(url, result));
    }

    public void GetArenaTime(Action<Package> result)
    {
        var url = ArenaUrl + $"/time";
        StartCoroutine(_GetData(url, result));
    }

    public void GetUserData(Action<Package> result)
    {
        var url = UserUrl + $"/get?id={user.id}";
        StartCoroutine(_GetData(url, result));
    }

    public void PostUserData(Action<Package> result)
    {
        if (string.IsNullOrEmpty(user.id))
        {
            result?.Invoke(new Package()
            {
                status = 404,
                message = "NULL"
            });
            return;
        }

        var url = UserUrl + "/set";
        var form = new WWWForm();
        form.AddField("id", user.id);
        form.AddField("name", user.name);
        form.AddField("avatar", user.avatar);
        form.AddField("data", "");
        form.AddField("version", Application.version);

        StartCoroutine(_PostData(url, form, result));
    }

    public void PostUpdateScore(Action<Package> result = null)
    {
        if (string.IsNullOrEmpty(user.id))
        {
            result?.Invoke(new Package()
            {
                status = 404,
                message = "NULL"
            });
            return;
        }

        var url = ArenaUrl + $"/set";
        var form = new WWWForm();
        form.AddField("id", user.id);
        form.AddField("score", (int)user.Currency[CurrencyType.Medal]);

        StartCoroutine(_PostData(url, form, result));
    }

    private IEnumerator _GetData(string url, Action<Package> result = null)
    {
        //Debug.Log("======================Get: " + url);

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.timeout = 15;

            yield return request.SendWebRequest();

            if (request != null)
            {
                switch (request.result)
                {
                    case UnityWebRequest.Result.ConnectionError:
                    case UnityWebRequest.Result.DataProcessingError:
                        Debug.LogWarning(request.error);

                        result?.Invoke(new Package()
                        {
                            status = 404,
                            message = request.error
                        });
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        Debug.LogWarning(request.error);

                        result?.Invoke(new Package()
                        {
                            status = 404,
                            message = request.error
                        });
                        break;
                    case UnityWebRequest.Result.Success:
                        Debug.Log(request.downloadHandler.text);

                        result?.Invoke(JsonConvert.DeserializeObject<Package>(request.downloadHandler.text));
                        break;
                }
            }
        }
    }

    private IEnumerator _PostData(string url, WWWForm form, Action<Package> result = null)
    {
        //Debug.Log("======================Post: " + url);

        using (UnityWebRequest request = UnityWebRequest.Post(url, form))
        {
            request.timeout = 15;

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                result?.Invoke(new Package()
                {
                    status = 404,
                    message = "Error"
                });
            }
            else
            {
                Debug.Log(request.downloadHandler.text);
                result?.Invoke(JsonConvert.DeserializeObject<Package>(request.downloadHandler.text));
            }
        }
    }
}

public class Package
{
    public int status;
    public string message;
    public object data;
}

public class UserPackage
{
    public string id;
    public string name;
    public string avatar;
}

public class TopPackage
{
    public List<RankPackage> top;
}

public class TimePackage
{
    public string time_now;
    public string time_start;
    public string time_end;
}

public class RankPackage
{
    public string id;
    public string name;
    public string avatar;
    public string score;
    public int rank;
    public string imageUrl;
}
