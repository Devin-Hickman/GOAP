using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredictMove : GOAPAction
{
    public string target;
    public float speed = -5f;

    public Vector2 predictedPosition;

    new void Awake()
    {
        base.Awake();
    }

    public override IEnumerator DoAction()
    {
        yield return GetComponent<PathMaker>().MoveTowards(predictedPosition);
    }

    protected override void AddPreConditions()
    {
        //No preConditions
    }

    protected override void AddPostConditions()
    {
        //Created elsewhere
    }


    public static void AddComponent(GameObject agent, float cost, Vector2 dest, float speed, string targetName, int i)
    {
        agent.AddComponent<PredictMove>();
        agent.GetComponents<PredictMove>()[i].cost = cost;
        agent.GetComponents<PredictMove>()[i].predictedPosition = dest;
        agent.GetComponents<PredictMove>()[i].speed = speed;
        agent.GetComponents<PredictMove>()[i].target = targetName;
        agent.GetComponent<NPC>().AddGOAPAction(agent.GetComponents<PredictMove>()[i]);
    }

    //After moving check to see if we are in range of any potential objects we could interact with for GOAP
    public void CreatePostConditions(Dictionary<Condition, object> state)
    {
        GameObject npcPredictor = new GameObject();
        npcPredictor.transform.position = predictedPosition;

        ContactFilter2D cf2d = new ContactFilter2D();
        cf2d.layerMask = LayerMask.GetMask("Items");
        cf2d.useLayerMask = true;

        CircleCollider2D reachedRadius = npcPredictor.AddComponent<CircleCollider2D>();
        reachedRadius.radius = this.GetComponent<NPC>().reachRadius;
        reachedRadius.isTrigger = true;

        Collider2D[] interactableColliders = new Collider2D[100];
        int numInteractables = reachedRadius.OverlapCollider(cf2d, interactableColliders);

        //TODO:

        if (numInteractables != 0)
        {
            for (int i = 0; i < numInteractables; i++)
            {
                float dist = Mathf.Abs(Vector3.Distance(predictedPosition, interactableColliders[i].transform.position));
                postConditions = interactableColliders[i].GetComponent<GOAPInteractable>().ApplyPostConditions(new Dictionary<Condition, object>(), dist);
            }
        }
        //Destroy(npcPredictor);
    }
}
