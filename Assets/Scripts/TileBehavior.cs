using TMPro;
using UnityEngine;

public class TileBehavior : MonoBehaviour
{
    public TileNode node;
    public TextMeshPro valueDisplayer;
    private Camera mainCamera;
    private bool valueAssigned = false;

    void Awake()
    {
        valueDisplayer = GetComponentInChildren<TextMeshPro>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (node == null)
            return;
        if (valueAssigned || node.tileValue == -1)
            return;

        valueDisplayer.text = GetStyledValue(node.tileValue);
        valueAssigned = true;
    }

    void LateUpdate()
    {
        if (valueDisplayer != null && mainCamera != null)
        {
            Vector3 direction = valueDisplayer.transform.position - mainCamera.transform.position;
            valueDisplayer.transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    private string GetStyledValue(int tileValue)
    {
        if (tileValue == 6 || tileValue == 9)
            return $"<u>{tileValue}</u>"; 
        else
            return tileValue.ToString();
    }
}
