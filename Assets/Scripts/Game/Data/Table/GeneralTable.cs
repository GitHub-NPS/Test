using Sirenix.OdinInspector;
using System.Collections.Generic;

[System.Serializable]
public class GeneralTable: IDataTable
{
    [ShowInInspector] public Dictionary<string, GeneralEntity> generalTable = new Dictionary<string, GeneralEntity>();

    private StringAccessor stringAccessor;
    private IntAccessor intAccessor;
    private FloatAccessor floatAccessor;
    private BoolAccessor boolAccessor;
    private IntArrayAccessor intArrayAccessor;
    private FloatArrayAccessor floatArrayAccessor;
    private StringArrayAccessor stringArrayAccessor;
    private BoolArrayAccessor boolArrayAccessor;
    private IntRandomAccessor intRandomAccessor;
    private FloatRandomAccessor floatRandomAccessor;

    #region accessor

    public string String(string cname, string defaultValue = "")
    {
        var data = generalTable.ContainsKey(cname) ? generalTable[cname] : null;
        return data != null ? (stringAccessor ??= new StringAccessor()).Get(data.Value) : defaultValue;
    }

    public int Int(string cname, int defaultValue = 0)
    {
        var data = generalTable.ContainsKey(cname) ? generalTable[cname] : null;
        return data != null ? (intAccessor ??= new IntAccessor()).Get(data.Value) : defaultValue;
    }

    public float Float(string cname, float defaultValue = 0f)
    {
        var data = generalTable.ContainsKey(cname) ? generalTable[cname] : null;
        return data != null ? (floatAccessor ??= new FloatAccessor()).Get(data.Value) : defaultValue;
    }

    public bool Bool(string cname, bool defaultValue = false)
    {
        var data = generalTable.ContainsKey(cname) ? generalTable[cname] : null;
        return data != null ? (boolAccessor ??= new BoolAccessor()).Get(data.Value) : defaultValue;
    }

    public int[] IntArray(string cname, int[] defaultValue = null)
    {
        var data = generalTable.ContainsKey(cname) ? generalTable[cname] : null;
        return data != null ? (intArrayAccessor ??= new IntArrayAccessor()).Get(data.Value) : defaultValue;
    }

    public string[] StringArray(string cname, string[] defaultValue = null)
    {
        var data = generalTable.ContainsKey(cname) ? generalTable[cname] : null;
        return data != null ? (stringArrayAccessor ??= new StringArrayAccessor()).Get(data.Value) : defaultValue;
    }

    public float[] FloatArray(string cname, float[] defaultValue = null)
    {
        var data = generalTable.ContainsKey(cname) ? generalTable[cname] : null;
        return data != null ? (floatArrayAccessor ??= new FloatArrayAccessor()).Get(data.Value) : defaultValue;
    }

    public bool[] BoolArray(string cname, bool[] defaultValue = null)
    {
        var data = generalTable.ContainsKey(cname) ? generalTable[cname] : null;
        return data != null ? (boolArrayAccessor ??= new BoolArrayAccessor()).Get(data.Value) : defaultValue;
    }

    public int IntRandom(string cname, int defaultValue = 0)
    {
        var data = generalTable.ContainsKey(cname) ? generalTable[cname] : null;
        return data != null ? (intRandomAccessor ??= new IntRandomAccessor()).Get(data.Value) : defaultValue;
    }

    public float? FloatRandom(string cname, float defaultValue = 0f)
    {
        var data = generalTable.ContainsKey(cname) ? generalTable[cname] : null;
        return data != null ? (floatRandomAccessor ??= new FloatRandomAccessor()).Get(data.Value) : defaultValue;
    }

    #endregion

    public void GetDatabase()
    {
        generalTable.Clear();
        DB_General.ForEachEntity(entity => {
            if (entity != null)
            {
                var data = new GeneralEntity();
                data.Name = entity.Get<string>("Name");
                data.Value = entity.Get<string>("Value");
                generalTable.Add(data.Name, data);
            }
        });
    }
}
