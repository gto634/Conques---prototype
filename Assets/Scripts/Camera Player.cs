using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class CameraPlayer : MonoBehaviour
{
    public List<Transform> transforms;

    public int currentPosition = 0;

    public GameObject map;

    void Start()
    {
        this.transform.SetPositionAndRotation(transforms[currentPosition].position, transforms[currentPosition].rotation);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (currentPosition < transforms.Count - 1) currentPosition += 1;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (currentPosition > 0 ) currentPosition -= 1;
        }
        this.transform.SetPositionAndRotation(transforms[currentPosition].position, transforms[currentPosition].rotation);
    }
}
