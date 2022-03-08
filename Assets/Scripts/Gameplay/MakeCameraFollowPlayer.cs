using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeCameraFollowPlayer : MonoBehaviour
{
    public float lerpValue = 0.1f;
    // Update is called once per frame
    protected void Update()
    {
        var player = Game.Instance.Player;
        if (player == null) return;

        Vector3 playerPos = new Vector3(player.position.x, player.position.y);
        playerPos = Vector3.Lerp(transform.position, playerPos, lerpValue);
        playerPos.z = -10;
        transform.position = playerPos;

    }
}
