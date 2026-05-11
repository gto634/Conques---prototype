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
            currentPosition = 1;
            Debug.Log("up");
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            currentPosition = 0;
            Debug.Log("down");
        }
        this.transform.SetPositionAndRotation(transforms[currentPosition].position, transforms[currentPosition].rotation);
        Debug.Log(transforms[currentPosition].position);
    }
}
