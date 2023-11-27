using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_Skill : Skill
{
    [SerializeField] private float crystalDuration;
    [SerializeField] private GameObject crystalPrefab;
    private GameObject currentCrsytal;

    public override void UseSkill()
    {
        base.UseSkill();


        if(currentCrsytal == null)
        {
            currentCrsytal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);

            Crystal_Skill_Controller currentCrystalScript = currentCrsytal.GetComponent<Crystal_Skill_Controller>();

            currentCrystalScript.SetupCrystal(crystalDuration);
        }
        else
        {
            player.transform.position = currentCrsytal.transform.position;
            Destroy(currentCrsytal); 
        }
    }

}
