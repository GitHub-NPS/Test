using BayatGames.SaveGameFree;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using I2.Loc;
using TMPro;

#if UNITY_EDITOR
public static class EditorTools
{
    #region I2
    [MenuItem("Tools/List Unique Char")]
    private static void DistinctAllCharacterInSheet()
    {
        var path = "Assets/Localization/Characters/CharacterSheet_{0}.txt";
        var pathCharId = "Assets/Localization/Characters/CharacterId_{0}.txt";
        var languageSourceAsset = AssetDatabaseUtils.GetAssetOfType<LanguageSourceAsset>("I2Languages");
        var sourceData = languageSourceAsset.SourceData;
        var languages = sourceData.mLanguages;
        var terms = sourceData.mTerms;

        var charLanguages = new List<string>();

        var languageBuilderArray = new StringBuilder[languages.Count];
        for (var i = 0; i < languageBuilderArray.Length; i++)
        {
            languageBuilderArray[i] = new StringBuilder();
        }

        var charListArray = new HashSet<char>[languages.Count];
        for (var i = 0; i < charListArray.Length; i++)
        {
            charListArray[i] = new HashSet<char>();
        }

        foreach (var termData in terms)
        {
            for (var i = 0; i < languages.Count; ++i)
            {
                AppendToCharSet(ref charListArray[i], termData.Languages[i], LocalizationManager.IsRTL(languages[i].Code));
            }
        }

        var allCharBuilder = new StringBuilder();

        // remove all character that already exist.
        for (var i = 0; i < languages.Count; i++)
        {
            var charArray = charListArray[i].ToArray().OrderBy(c => c).ToList();
            for (var j = 0; j < i; j++)
            {
                var count = charArray.Count;
                for (var t = 0; t < count; t++)
                {
                    if (charListArray[j].Contains(charArray[t]))
                    {
                        charArray.RemoveAt(t);
                        count--;
                        t--;
                    }
                }
            }

            if (charArray.Count > 0)
            {
                var charBuilder = new StringBuilder();
                var missingCharIdBuilder = new StringBuilder();

                foreach (var character in charArray)
                {
                    if (!HasCharacter(TMP_Settings.defaultFontAsset, character))
                    {
                        missingCharIdBuilder.Append(character);
                    }

                    charBuilder.Append(character);
                    allCharBuilder.Append(character);
                }

                var fileName = string.Format(path, languages[i].Name);
                using (var writer = File.CreateText(fileName))
                {
                    writer.Write(charBuilder.ToString());
                }

                var charIdBuilder = new StringBuilder();
                foreach (var character in charArray)
                {
                    charIdBuilder.Append((int)character);
                    charIdBuilder.Append(',');
                }

                if (charIdBuilder.Length > 0)
                {
                    charIdBuilder.Remove(charIdBuilder.Length - 1, 1);
                }

                var idFileName = string.Format(pathCharId, languages[i].Name);
                using (var writer = File.CreateText(idFileName))
                {
                    writer.Write(charIdBuilder);
                }

                AddMissingCharacters(languages[i].Name, missingCharIdBuilder.ToString());
            }
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("List Unique Char");
    }

    private static void AddMissingCharacters(string language, string characters)
    {
        if (characters.Length <= 0) return;

        var font = TMP_Settings.defaultFontAsset;
        if (font.atlasPopulationMode == AtlasPopulationMode.Dynamic)
        {
            return;
        }

        if (font.fallbackFontAssetTable != null && font.fallbackFontAssetTable.Count > 0)
        {
            for (var i = 0; i < font.fallbackFontAssetTable.Count && font.fallbackFontAssetTable[i] != null; i++)
            {
                var fallbackFont = font.fallbackFontAssetTable[i];
                if (fallbackFont.name.Equals(language))
                {
                    fallbackFont.atlasPopulationMode = AtlasPopulationMode.Dynamic;
                    fallbackFont.TryAddCharacters(characters);
                    fallbackFont.atlasPopulationMode = AtlasPopulationMode.Static;
                }
            }
        }
    }

    private static bool HasCharacter(TMP_FontAsset font, char character)
    {
        if (font.atlasPopulationMode == AtlasPopulationMode.Dynamic)
        {
            return false;
        }

        if (font.characterLookupTable.ContainsKey(character))
        {
            return true;
        }

        if (font.fallbackFontAssetTable != null && font.fallbackFontAssetTable.Count > 0)
        {
            for (var i = 0; i < font.fallbackFontAssetTable.Count && font.fallbackFontAssetTable[i] != null; i++)
            {
                if (HasCharacter(font.fallbackFontAssetTable[i], character))
                {
                    return true;
                }
            }
        }

        return false;
    }

    static void AppendToCharSet(ref HashSet<char> sb, string text, bool isRTL)
    {
        if (string.IsNullOrEmpty(text)) return;

        text = RemoveTagsPrefix(text, "[i2p_");
        text = RemoveTagsPrefix(text, "[i2s_");
        text = RemoveTagsPrefix(text, "{");

        if (isRTL) text = RTLFixer.Fix(text);

        foreach (char c in text)
        {
            sb.Add(c);
        }
    }

    static string RemoveTagsPrefix(string text, string tagPrefix)
    {
        var idx = 0;
        while (idx < text.Length)
        {
            idx = text.IndexOf(tagPrefix);
            if (idx < 0) break;

            int idx2 = text.IndexOf(']', idx);
            if (idx2 < 0) break;

            text = text.Remove(idx, idx2 - idx + 1);
        }

        idx = 0;
        while (idx < text.Length)
        {
            idx = text.IndexOf(tagPrefix);
            if (idx < 0) break;

            int idx2 = text.IndexOf('}', idx);
            if (idx2 < 0) break;

            text = text.Remove(idx, idx2 - idx + 1);
        }

        return text;
    }
}

public class AssetDatabaseUtils
{

    public static string GetSelectionObjectPath()
    {
        var path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (path == "")
        {
            path = "Assets";
        }
        else if (Path.GetExtension(path) != "")
        {
            path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
        }
        return path;
    }

    public static T GetAssetOfType<T>(string name, System.Type mainType = null) where T : class
    {
        if (mainType == null)
        {
            mainType = typeof(T);
        }
        var guids = AssetDatabase.FindAssets(name + " t:" + mainType.Name);
        if (guids.Length == 0)
            return null;
        string guid = guids[0];
        string path = AssetDatabase.GUIDToAssetPath(guid);
        foreach (var o in AssetDatabase.LoadAllAssetsAtPath(path))
        {
            var res = o as T;
            if (res != null)
            {
                return res;
            }
        }
        return default(T);
    }

    public static string GetAssetPathOfType<T>(string name, System.Type mainType = null) where T : class
    {
        if (mainType == null)
        {
            mainType = typeof(T);
        }
        var guids = AssetDatabase.FindAssets(name + " t:" + mainType.Name);
        if (guids.Length == 0)
            return null;
        string guid = guids[0];
        string path = AssetDatabase.GUIDToAssetPath(guid);
        return path;
    }

    public static T GetAssetOfType<T>(bool unique = false) where T : class
    {
        var guids = AssetDatabase.FindAssets("t:" + typeof(T).FullName);
        if (guids.Length == 0)
            return null;
        if (guids.Length > 1 && unique)
        {
            var pathes = "";
            foreach (var g in guids)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(g);
                pathes += assetPath + "\n";
            }
            throw new System.ArgumentException("Has multiple objects with this type: \n" + pathes);
        }
        var guid = guids[0];
        var path = AssetDatabase.GUIDToAssetPath(guid);
        return AssetDatabase.LoadAssetAtPath(path, typeof(T)) as T;
    }

    public static T[] GetAssetsOfType<T>() where T : class
    {
        if (typeof(UnityEngine.Component).IsAssignableFrom(typeof(T)))
        {
            var guidsGO = AssetDatabase.FindAssets("t:Prefab");
            var l = new List<T>();
            foreach (var g in guidsGO)
            {
                var path = AssetDatabase.GUIDToAssetPath(g);
                var t = (AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject).GetComponent<T>();
                if (t != null)
                {
                    l.Add(t);
                }
            }
            return l.ToArray();
        }

        var guids = AssetDatabase.FindAssets("t:" + typeof(T).FullName);
        if (guids.Length == 0)
            return null;

        var i = 0;
        var res = new T[guids.Length];
        foreach (var g in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(g);
            var t = AssetDatabase.LoadAssetAtPath(path, typeof(T)) as T;
            res[i] = t;
            i++;
        }
        return res;
    }
    #endregion

    [MenuItem("Tools/Clear Save Data")]
    private static void ClearSaveData()
    {
        SaveGame.Clear();
        PlayerPrefs.DeleteAll();
        Debug.Log("Clear Save Data");
    }

    [MenuItem("Tools/Clear Live Data")]
    private static void ClearLiveData()
    {
        //Resources.Load<DemoLive>("Live/DemoLive").Clear();
        Debug.Log("Clear Live Data");
    }
}
#endif