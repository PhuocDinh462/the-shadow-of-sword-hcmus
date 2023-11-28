using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_Skill : Skill
{
    [SerializeField] private float crystalDuration;
    [SerializeField] private GameObject crystalPrefab;
    private GameObject currentCrsytal;


    [Header("Explosive Crystal")]
    [SerializeField] private bool canExplode;

    [Header("Moving Crystal")]
    [SerializeField] private bool canMoveToEnemy;
    [SerializeField] private float moveSpeed;

    [Header("Muti Stacking crystal")]
    [SerializeField] private bool canUseMultiStacks;
    [SerializeField] private int amountOfStacks;
    [SerializeField] private float mutiStackCooldown;
    [SerializeField] private float useTimeWondow;
    [SerializeField] private List<GameObject> crystalLeft = new List<GameObject>();

    public override void UseSkill()
    {
        base.UseSkill();

        if (CanUseMultiCrystal())
            return;


        if (currentCrsytal == null)
        {
            currentCrsytal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);

            Crystal_Skill_Controller currentCrystalScript = currentCrsytal.GetComponent<Crystal_Skill_Controller>();

            currentCrystalScript.SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(currentCrsytal.transform));
        }
        else
        {
            if (canMoveToEnemy)
                return;


            Vector2 playerPos = player.transform.position; 

            player.transform.position = currentCrsytal.transform.position;
            
            currentCrsytal.transform.position = playerPos;

            currentCrsytal.GetComponent<Crystal_Skill_Controller>()?.FinishCrystal();

        }
    }


    private bool CanUseMultiCrystal()
    {
        if (canUseMultiStacks)
        {
            if(crystalLeft.Count > 0)
            {
                if (crystalLeft.Count == amountOfStacks)
                    Invoke("ResetAbility", useTimeWondow);


                cooldown = 0;
                GameObject crystalToSpawn = crystalLeft[crystalLeft.Count - 1];
                GameObject newCrystal = Instantiate(crystalToSpawn, player.transform.position, Quaternion.identity);

                crystalLeft.Remove(crystalToSpawn);

                newCrystal.GetComponent<Crystal_Skill_Controller>().
                    SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(newCrystal.transform));   

                if(crystalLeft.Count <= 0)
                {
                    cooldown = mutiStackCooldown;
                    RefilCrystal();

                }
            
            return true;
            
            }

            
        }
        return false;
    }

    private void RefilCrystal()
    {
        int amountToAdd = amountOfStacks - crystalLeft.Count;

        for(int i =0; i < amountToAdd; i++)
        {
            crystalLeft.Add(crystalPrefab);
        }
    }


    private void ResetAbility()
    {

        if (cooldownTimer > 0)
            return;

        cooldownTimer = mutiStackCooldown;
        RefilCrystal();
    }
}
