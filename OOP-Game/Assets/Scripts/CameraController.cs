using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Player player;
    public Vector3 cameraOffset = new Vector3(1, 2, -3);
    private void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
    }
    private void LateUpdate()
    {
        transform.position = player.transform.position + cameraOffset;
    }
}
