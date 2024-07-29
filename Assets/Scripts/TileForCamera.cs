using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileForCamera : MonoBehaviour
{
    void Start(){
        if(CameraController.Instance != null)
            CameraController.Instance.AddTile(transform);
    }
}
