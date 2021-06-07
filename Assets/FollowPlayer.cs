using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public MyTransform player;       
    private Vector3 offset;   

    // Use this for initialization
    void Start()
    {
        offset = transform.position - player.Position;
    }

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        transform.position = (player.Position + offset) * 2;
    }
}
