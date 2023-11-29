using UnityEngine;

public class CharacterStats : MonoBehaviour {
  private EntityFX fx;

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

  public bool isIgnited; // make damage overtime
  public bool isChilled; // reduce armor by 20%
  public bool isShocked; // reduce accuracy by 20%

  [SerializeField] private float ailmentsDuaration = 2;
  private float ignitedTimer;
  private float chilledTimer;
  private float shockedTimer;


  private float igniteDamageCooldown = .3f;
  private float igniteDamageTimer;
  private int igniteDamage;

  [SerializeField] private GameObject shockStrikePrefab;
  private int shockDamage;

  public int currentHealth;

  public System.Action onHealthChanged;

  protected virtual void Start() {
    critPower.SetDefaultValue(150);
    currentHealth = GetMaxHealthValue();

    fx = GetComponent<EntityFX>();

    Debug.Log("Characters called");

  }
  protected virtual void Update() {
    ignitedTimer -= Time.deltaTime;
    chilledTimer -= Time.deltaTime;
    shockedTimer -= Time.deltaTime;

    igniteDamageTimer -= Time.deltaTime;


    if (ignitedTimer < 0) {
      isIgnited = false;
    }
    if (chilledTimer < 0) {
      isChilled = false;
    }
    if (shockedTimer < 0) {
      isShocked = false;
    }
    if (igniteDamageTimer < 0 && isIgnited) {
      Debug.Log("Take burn damage " + igniteDamage);

      DecreaseHealthBy(igniteDamage);
      if (currentHealth < 0) {
        Die();
      }
      igniteDamageTimer = igniteDamageCooldown;
    }
  }

  public virtual void DoDamage(CharacterStats _targetStats) {
    if (TargetCanAvoidAttack(_targetStats)) return;

    int totalDamage = damage.GetValue() + strength.GetValue();

    if (CanCrit()) {
      totalDamage = CalculateCriticalDamage(totalDamage);
    }


    totalDamage = CheckTargetArmor(_targetStats, totalDamage);

    // _targetStats.TakeDamage(totalDamage);
    DoMagicalDamage(_targetStats);

  }
  public virtual void DoMagicalDamage(CharacterStats _targetStats) {
    int _fireDamage = fireDamage.GetValue();
    int _iceDamage = iceDamage.GetValue();
    int _lightingDamage = lightingDamage.GetValue();

    int totalMagicalDamage = _fireDamage + _iceDamage + _lightingDamage + intelligence.GetValue();

    totalMagicalDamage = CheckTargetResistance(_targetStats, totalMagicalDamage);
    _targetStats.TakeDamage(totalMagicalDamage);


    if (Mathf.Max(_fireDamage, _iceDamage, _lightingDamage) <= 0) return;

    bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightingDamage;
    bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightingDamage;
    bool canApplyShock = _lightingDamage > _fireDamage && _lightingDamage > _iceDamage;

    while (!canApplyIgnite && !canApplyChill && !canApplyShock) {
      if (Random.value < .3f && _fireDamage > 0) {
        canApplyIgnite = true;
        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
        Debug.Log("Applied fire");
        return;
      }

      if (Random.value < .4f && _iceDamage > 0) {
        canApplyChill = true;
        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
        Debug.Log("Applied ice");
        return;
      }

      if (Random.value < .5f && _lightingDamage > 0) {
        canApplyShock = true;
        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
        Debug.Log("Applied electric");
        return;
      }
    }

    if (canApplyIgnite) {
      _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * .2f));
    }

    if (canApplyShock) {
      _targetStats.SetupShockStrikeDamage(Mathf.RoundToInt(_lightingDamage * .1f));
    }


    _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
  }
  private static int CheckTargetResistance(CharacterStats _targetStats, int totalMagicalDamage) {
    totalMagicalDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3);
    totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);
    return totalMagicalDamage;
  }

  public void ApplyAilments(bool _ignite, bool _chill, bool _shock) {
    bool canApplyIgnite = !isIgnited && !isChilled && !isShocked;
    bool canApplyChill = !isIgnited && !isChilled && !isShocked;
    bool canApplyShock = !isIgnited && !isChilled;

    if (_ignite && canApplyIgnite) {
      isIgnited = _ignite;
      ignitedTimer = ailmentsDuaration;

      fx.IgniteFxFor(ailmentsDuaration);

    }
    if (_chill && canApplyChill) {
      isChilled = _chill;
      chilledTimer = ailmentsDuaration;

      float slowPercentage = .25f;
      GetComponent<Entity>().SlowEntityBy(slowPercentage, ailmentsDuaration);

      fx.ChillFxFor(ailmentsDuaration);
    }
    if (_shock && canApplyShock) {
      if (!isShocked) {
        ApplyShock(_shock);
      }
      else {
        if (GetComponent<Player>() != null) return;


        HitNearestTargetWithShockStrike();


      }

    }

  }



  public void ApplyShock(bool _shock) {
    if (isShocked) return;

    shockedTimer = ailmentsDuaration;
    isShocked = _shock;
    fx.ShockFxFor(ailmentsDuaration);
  }
  private void HitNearestTargetWithShockStrike() {
    //find closest target enemy
    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);

    float closestDistance = Mathf.Infinity;
    Transform closestEnemy = null;
    foreach (var hit in colliders) {
      if (hit.GetComponent<Enemy>() != null && Vector2.Distance(transform.position, hit.transform.position) > 1) {
        float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);
        if (distanceToEnemy < closestDistance) {
          closestDistance = distanceToEnemy;
          closestEnemy = hit.transform;
        }
      }
      if (closestEnemy == null) {
        closestEnemy = transform;
      }
    }


    if (closestEnemy != null) {
      GameObject newShockStrike = Instantiate(shockStrikePrefab, transform.position, Quaternion.identity);

      newShockStrike.GetComponent<ShockStrike_Controller>().Setup(shockDamage, closestEnemy.GetComponent<CharacterStats>());
    }
  }

  public void SetupIgniteDamage(int _damage) => igniteDamage = _damage;
  public void SetupShockStrikeDamage(int _damage) => shockDamage = _damage;
  public virtual void TakeDamage(int _damage) {
    DecreaseHealthBy(_damage);

    Debug.Log(_damage);

    if (currentHealth < 0) {
      Die();
    }
  }

  protected virtual void DecreaseHealthBy(int _damage) {
    currentHealth -= _damage;
    if (onHealthChanged != null) {
      onHealthChanged();
    }
  }

  protected virtual void Die() {
    //throw new NotImplementedException();
  }

  private int CheckTargetArmor(CharacterStats _targetStats, int totalDamage) {
    if (_targetStats.isChilled) {
      totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * .8f);
    }
    else {
      totalDamage -= _targetStats.armor.GetValue();
    }

    totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
    return totalDamage;
  }

  private bool TargetCanAvoidAttack(CharacterStats _targetStats) {
    int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

    if (isShocked) {
      totalEvasion += 20;
    }

    if (Random.Range(0, 100) < totalEvasion) {
      Debug.Log("ATTACK AVOIDED");
      return true;
    }
    return false;
  }

  private bool CanCrit() {
    int totalCriticalChance = critChance.GetValue() + agility.GetValue();
    if (Random.Range(0, 100) <= totalCriticalChance) {
      return true;
    }
    return false;
  }
  private int CalculateCriticalDamage(int _damage) {
    float totalCritPower = (critPower.GetValue() + strength.GetValue()) * .01f;
    float critDamage = _damage * totalCritPower;
    return Mathf.RoundToInt(critDamage);
  }

  public int GetMaxHealthValue() {
    return maxHealth.GetValue() + vitality.GetValue() * 5;
  }
}