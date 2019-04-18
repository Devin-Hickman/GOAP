using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestWood : GOAPAction
{
    private float closeness = 5f;
    private new void Awake()
    {
        base.Awake();
        cost = 20;
    }

    public override IEnumerator DoAction(Dictionary<Condition, object> npcState)
    {
        base.DoAction(npcState);
        ContactFilter2D cf2d = new ContactFilter2D();
        cf2d.layerMask = LayerMask.GetMask("Items");
        cf2d.useLayerMask = true;

        CircleCollider2D nearbyItemCollider = gameObject.AddComponent<CircleCollider2D>();
        nearbyItemCollider.radius = closeness;
        nearbyItemCollider.isTrigger = true;

        Collider2D[] interactableColliders = new Collider2D[100];
        int numInteractables = nearbyItemCollider.OverlapCollider(cf2d, interactableColliders);

        if (numInteractables != 0)
        {
            for (int i = 0; i < numInteractables; i++)
            {
                if (interactableColliders[i].GetComponent<Wood>() != null)
                {
                    Destroy(interactableColliders[i]);
                    break;
                }
            }
        }

        Destroy(nearbyItemCollider);
        yield return null;
    }

    public override bool CheckPreConditions(Dictionary<Condition, object> currentState)
    {
        return (currentState.ContainsKey(Condition.nearIron) && (float)currentState[Condition.nearWood] <= (float)preConditions[Condition.nearWood]);
    }

    protected override void AddPreConditions()
    {
        preConditions.Add(Condition.nearWood, closeness);
    }

    protected override void AddPostConditions()
    {
        postConditions.Add(Condition.hasWood, true);
    }
}
