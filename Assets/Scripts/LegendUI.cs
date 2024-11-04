using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LegendUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private OreValues values;

    [SerializeField] private TMP_Text copperPriceText;
    [SerializeField] private TMP_Text ironPriceText;
    [SerializeField] private TMP_Text silverPriceText;
    [SerializeField] private TMP_Text goldPriceText;
    [SerializeField] private TMP_Text diamondPriceText;

    private void Start()
    {
        copperPriceText.text = $"- ${values.Values[OreType.COPPER]}";
        ironPriceText.text = $"- ${values.Values[OreType.IRON]}";
        silverPriceText.text = $"- ${values.Values[OreType.SILVER]}";
        goldPriceText.text = $"- ${values.Values[OreType.GOLD]}";
        diamondPriceText.text = $"- ${values.Values[OreType.DIAMOND]}";
    }
}
