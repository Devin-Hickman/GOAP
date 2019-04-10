using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float speed;
    Rigidbody2D rb2d;
    bool isMoving = false;

    public Vector2 destination;

    public IEnumerator MoveTo(Vector2 destination)
    {
        if(isMoving) yield break;

        isMoving = true;
        //For when A* is implemented, until then just teleport
        GetComponent<NPC>().transform.position = destination;

        /*float dist = Vector2.Distance(npc.transform.position, destination);
        while (dist > 0.1)
        {
            Vector2 dir = destination - (Vector2)npc.transform.position;
            npc.velocity = (dir.normalized * speed);
            dist = Vector2.Distance(npc.transform.position, destination);
            yield return null;
        }
        npc.velocity = Vector2.zero;*/

        isMoving = false;
    }

}
