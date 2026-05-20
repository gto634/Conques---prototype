using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderTextValue : MonoBehaviour
{

    private Slider slider;
    private TextMeshProUGUI textComp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        slider = GetComponentInParent<Slider>();
        textComp = GetComponent<TextMeshProUGUI>();
        Debug.Log(slider);
        Debug.Log(textComp);
    }

    void Start()
    {
        UpdateText(slider.value);
        slider.onValueChanged.AddListener(UpdateText);
    }

    // Update is called once per frame
    void UpdateText(float val)
    {
        textComp.text = slider.value.ToString();
    }
}
