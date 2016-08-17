using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ChangeValue : MonoBehaviour
{

    public Slider slider;
    public Text text;
    public string unit;
    public byte decimals = 2;
    void Start()
    {
        slider.maxValue = 45;
    }


    void OnEnable()
    {
        slider.onValueChanged.AddListener(UpdateValue);
        UpdateValue(slider.value);
    }
    void OnDisable()
    {
        slider.onValueChanged.RemoveAllListeners();
    }

    void UpdateValue(float value)
    {
        text.text = value.ToString("n" + decimals) + " " + unit;
    }


}
