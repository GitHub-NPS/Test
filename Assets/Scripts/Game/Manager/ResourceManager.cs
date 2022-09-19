using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager S;

    private Dictionary<string, Sprite> dicSP = new Dictionary<string, Sprite>();
    private Dictionary<string, AudioClip> dicSound = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> dicMusic = new Dictionary<string, AudioClip>();

    private Dictionary<string, SkeletonAnimation> dicAnimation = new Dictionary<string, SkeletonAnimation>();
    private Dictionary<string, SkeletonGraphic> dicGraphic = new Dictionary<string, SkeletonGraphic>();
    private Dictionary<string, SkeletonDataAsset> dicSkeleton = new Dictionary<string, SkeletonDataAsset>();

    [SerializeField] private List<SpriteAtlas> Atlas = new List<SpriteAtlas>();
    private Dictionary<string, SpriteAtlas> dicAtlas = new Dictionary<string, SpriteAtlas>();

#if UNITY_EDITOR
    private static List<SpriteAtlas> sAtlas = new List<SpriteAtlas>();
    private void OnValidate()
    {
        sAtlas.Clear();
        foreach (var item in Atlas)
        {
            sAtlas.Add(item);
        }
    }
#endif

    private void Awake()
    {
        if (!S) S = this;

        foreach (var item in Atlas)
        {
            dicAtlas.Add(item.name, item);
        }
    }

    private void Start()
    {

    }

    public AudioClip LoadSound(string name, bool save = true)
    {
        return Load("Audios/Sound/" + name, dicSound, save);
    }

    public void ClearSound()
    {
        dicSound.Clear();
    }

    public AudioClip LoadMusic(string name, bool save = true)
    {
        return Load("Audios/Music/" + name, dicMusic, save);
    }

    public Sprite LoadSprite(string path, bool save = true)
    {
        return Load("Textures/" + path, dicSP, save);
    }

    public Sprite LoadSprite(string atlas, string name, bool save = true, bool check = true)
    {
        if (save)
        {
            string path = atlas + "/" + name;

            if (!dicSP.ContainsKey(path))
            {
                if (check && !dicAtlas.ContainsKey(atlas))
                    Debug.LogError("Resource Null: " + path);
                Sprite t = dicAtlas[atlas].GetSprite(name);

                if (t) dicSP.Add(path, t);
                else if (check) Debug.LogError("Resource Null: " + path);
            }

            if (dicSP.ContainsKey(path)) return dicSP[path];
            return default;
        }
        else
        {
            return dicAtlas[atlas].GetSprite(name);
        }
    }

    public SkeletonAnimation LoadAnimation(string path, bool save = true)
    {
        return Load("Animations/" + path, dicAnimation, save);
    }

    public SkeletonGraphic LoadGraphic(string path, bool save = true)
    {
        return Load("Graphics/" + path, dicGraphic, save);
    }

    public SkeletonDataAsset LoadSkeleton(string path, bool save = true)
    {
        return Load("Skeletons/" + path, dicSkeleton, save);
    }

    private T Load<T>(string path, Dictionary<string, T> dic, bool save = true, bool check = true) where T : Object
    {
        if (save)
        {
            if (!dic.ContainsKey(path))
            {
                T t = Resources.Load<T>(path);

                if (t) dic.Add(path, t);
                else if (check) Debug.LogError("Resource Null: " + path);
            }

            if (dic.ContainsKey(path)) return dic[path];
            return default;
        }
        else
        {
            return Resources.Load<T>(path);
        }
    }

    // Custom
}
