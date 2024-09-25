using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform AlicePos;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            transform.position = new Vector3(AlicePos.position.x, AlicePos.position.y, transform.position.z) + new Vector3(-1.5f, 1.2f, 0);
            transform.GetComponent<Camera>().orthographicSize = 2.5f;
        }
    }
}
