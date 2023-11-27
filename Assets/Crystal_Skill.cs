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

    public override void UseSkill()
    {
        base.UseSkill();


        if(currentCrsytal == null)
        {
            currentCrsytal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);

            Crystal_Skill_Controller currentCrystalScript = currentCrsytal.GetComponent<Crystal_Skill_Controller>();

            currentCrystalScript.SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed);
        }
        else
        {
            Vector2 playerPos = player.transform.position; 
            player.transform.position = currentCrsytal.transform.position;
            
            currentCrsytal.transform.position = playerPos;
            currentCrsytal.GetComponent<Crystal_Skill_Controller>()?.FinishCrystal();

        }
    }

}
