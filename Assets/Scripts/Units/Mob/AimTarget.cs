using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimTarget : MonoBehaviour
{
    public PlayerMoveController player;
    
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, player.MoveDir + player.transform.position,2*Time.deltaTime);
    }
}
