using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace UserTierDefinition.Runtime
{
    public class UserTierDefinition : SingletonBehaviourBase<UserTierDefinition>
    {
        [SerializeField] private int tier;
        [SerializeField] private string country;
        [SerializeField] private string countryCode;

        private const string PlayerPrefsKey = "UserTier";

        public int Tier => tier == 0 ? 3 : tier;
        public string Country => country;
        public string CountryCode => countryCode;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void UserTierDefinitionInitialize()
        {
            Instance.FetchUserTierDefinition();
        }

        private void FetchUserTierDefinition()
        {
            StartCoroutine(_FetchUserTierDefinition());
        }

        private IEnumerator _FetchUserTierDefinition()
        {
            if (PlayerPrefs.HasKey(PlayerPrefsKey))
            {
                var userTierData = JsonUtility.FromJson<UserTierModel>(PlayerPrefs.GetString(PlayerPrefsKey));
                tier = userTierData.tier;
                country = userTierData.country;
                countryCode = userTierData.country_code;
                yield break;
            }

            const string configUrl = "https://configuration.unimob.com.vn/api/user-tier/get";
            var www = UnityWebRequest.Get(configUrl);
            yield return www.SendWebRequest();
            if (www.error != null) yield return null;
            var responseModel = JsonUtility.FromJson<ResponseModel>(www.downloadHandler.text);
            if (responseModel != null && responseModel.status == 200)
            {
                tier = responseModel.data.tier;
                country = responseModel.data.country;
                countryCode = responseModel.data.country_code;
                PlayerPrefs.SetString(PlayerPrefsKey, JsonUtility.ToJson(responseModel.data));
                PlayerPrefs.Save();
            }
        }
    }
}