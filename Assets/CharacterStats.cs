using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Major stats")]
    public Stat strength; // 1 point increase damage by 1 and crit.power  1%
    public Stat agility;  // 1 increase evasion by 1% and crit.chance  1%
    public Stat intelligence; // 1 increase magic damage 1 and magic resistance 3
    public Stat vitality; // 1 increase health by 3-5 points

    [Header("Offensive stats")]
    public Stat damage;
    public Stat critChance;
    public Stat critPower; // default value 150%


    [Header("Defensive stats")]
    public Stat maxHealth;
    public Stat armor;
    public Stat evasion;
    public Stat magicResistance;

    [Header("Magic stats")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightingDamage;

    public bool isIgnited; // make damge overtime
    public bool isChilled; // reduce armor by 20%
    public bool isShocked; // reduce accuracy by 20%

    [SerializeField] private int currentHealth;

    protected virtual void Start()
    {
        currentHealth = maxHealth.GetValue();

        damage.AddModifier(4);
    }

    public virtual void DoDamage(CharacterStats _targetStats)
    {
        if (TargetCanAvoidAttack(_targetStats)) return;

        int totalDamage = damage.GetValue() + strength.GetValue();


        totalDamage = CheckTargetArmor(_targetStats, totalDamage);

        _targetStats.TakeDamge(totalDamage);

    }

    public virtual void TakeDamge(int _damage)
    {
        currentHealth -= _damage;

        Debug.Log(_damage);

        if (currentHealth < 0)
        {

            Die();
        }
    }

    protected virtual void Die()
    {
        //throw new NotImplementedException();
    }

    private int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
    {
        totalDamage -= _targetStats.armor.GetValue();

        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }

    private bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (Random.Range(0, 100) < totalEvasion)
        {
            Debug.Log("ATTACK AVOIDED");
            return true;
        }
        return false;
    }
}
