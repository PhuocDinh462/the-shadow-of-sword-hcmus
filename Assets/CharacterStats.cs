using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{

    public Stat damage;
    public Stat maxHealth;
    public Stat strength;

    [SerializeField] private int currentHealth;

    protected virtual void Start()
    {
        currentHealth = maxHealth.GetValue();

        damage.AddModifier(4);
    }

    public virtual void DoDamage(CharacterStats _targetStats)
    {

        int totalDamage = damage.GetValue() + strength.GetValue();

        _targetStats.TakeDamge(totalDamage);

    }

    public virtual void TakeDamge(int _damage)
    {
        currentHealth -= _damage;

        if (currentHealth < 0)
        {

            Die();
        }
    }

    protected virtual void Die()
    {
        //throw new NotImplementedException();
    }
}
