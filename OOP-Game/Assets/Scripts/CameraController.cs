using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Player player;
    [SerializeField] public Vector3 cameraOffset = new Vector3(1, 1.3f, -4);
    private void LateUpdate()
    {
        try
        {
            player = GameObject.FindObjectOfType<Player>();
        }
        catch
        {

        }
        transform.position = player.transform.position + cameraOffset;
    }
}
