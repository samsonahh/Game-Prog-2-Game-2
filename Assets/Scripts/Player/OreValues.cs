using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "OreValues", menuName = "ScriptableObjects/OreValues", order = 1)]
public class OreValues : ScriptableObject
{
    [Serializable]
    public class OreValue
    {
        public OreType OreType;
        public int Value;
    }

    public Dictionary<OreType, int> Values { get; private set; } = new Dictionary<OreType, int>();
    [SerializeField] private List<OreValue> serializedValues = new List<OreValue>();

    private void OnValidate()
    {
        ConvertSerializedValuesToDict();
    }

    private void ConvertSerializedValuesToDict()
    {
        for(int i = 0; i < serializedValues.Count; i++)
        {
            //Debug.Log($"{serializedValues[i].OreType}: {serializedValues[i].Value}");
            if(Values.ContainsKey(serializedValues[i].OreType)) Values[serializedValues[i].OreType] = serializedValues[i].Value;
            else Values.Add(serializedValues[i].OreType, serializedValues[i].Value);
        }
    }
}
