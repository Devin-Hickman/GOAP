using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : GOAPAction
{
    public float speed;
    private float xVelocity = 0;
    private float yVelocity = 0;
    Rigidbody2D rb2d;
    bool isMoving = false;

    Vector2 destination;
    private NPC npc;


    private new void Awake()
    {
        base.Awake();        
        rb2d = GetComponent<Rigidbody2D>();
        npc = GetComponent<NPC>();
    }

    public override void DoAction()
    {
        StartCoroutine(MoveTo(destination));
    }

    protected IEnumerator MoveTo(Vector2 destination)
    {
        if(isMoving) yield break;

        isMoving = true;
        float dist = Vector2.Distance(npc.transform.position, destination);
        while (dist > 0.1)
        {
            Vector2 dir = destination - (Vector2)npc.transform.position;
            npc.velocity = (dir.normalized * speed);
            dist = Vector2.Distance(npc.transform.position, destination);
            yield return null;
        }
        UpdatePostConditions();
        npc.velocity = Vector2.zero;
        isMoving = false;
    }

    //After moving check to see if we are in range of any potential objects we could interact with for GOAP
    private void UpdatePostConditions()
    {
        ContactFilter2D cf2d = new ContactFilter2D();
        cf2d.layerMask = LayerMask.GetMask("Interactable");
        cf2d.useLayerMask = true;

        CircleCollider2D reachedRadius = gameObject.AddComponent<CircleCollider2D>();
        reachedRadius.radius = this.GetComponent<NPC>().ReachRadius;
        reachedRadius.isTrigger = true;

        Collider2D[] interactableColliders = new Collider2D[100];
        int numInteractables = reachedRadius.OverlapCollider(cf2d, interactableColliders);

        if (numInteractables != 0)
        {
            for(int i = 0; i < numInteractables; i++)
            {
                float dist = Mathf.Abs(Vector3.Distance(transform.position, interactableColliders[i].transform.position));
                interactableColliders[i].GetComponent<GOAPInteractable>().ApplyPostConditions(npc.GetNPCState(), dist);
            }
        }
        Destroy(reachedRadius);
    }
}
