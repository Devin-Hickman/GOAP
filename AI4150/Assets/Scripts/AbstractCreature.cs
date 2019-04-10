using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractCreature : MonoBehaviour
{
    int health;

    public void UnderAttack(int damage)
    {
        health -= damage;
        if(health < 0)
        {
            OnDeath();
        }
    }
    private void OnDeath()
    {
        //TODO:
    }
}
