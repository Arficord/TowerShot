using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum ActionState{
    DEFAULT,
    WALK,
    ATTACK,
    STAN,
    DEATH,
    USING_ABILITY,
}   
public abstract class EnemyController : MonoBehaviour, IDestroyable {
    #region Variables
    private const float SPEED_RANDOM_FACTOR = 0.05f;
    public static List<EnemyController> liveEnemies = new List<EnemyController>();
    public static int enemiesOnField=0;

    public static MainWallController mainWall;
    public string name;
    public MonsterStats baseStats;
    [HideInInspector]
    public MonsterStats currentStats;

    protected EnemyStatusUIController statusUI;
    protected Rigidbody rigidbody;
    protected Animator animator;
    protected EnemyVisualEffectsController visualEffectsController;

    public ActionState action = ActionState.DEFAULT;

    public AreaEffectController areaEffect;

    protected StatusEffects statusEffect;

    public static event Action onEnemiesEnded;
    public event Action onEnemyDeath;

    private IEnumerator burningCoroutine;
    #endregion
    #region Initialize
    protected void Start()
    {
        initialize();
    }
    private void initialize(){
        liveEnemies.Add(this);
        enemiesOnField++;
        statusEffect = new StatusEffects();
        animator = GetComponent<Animator>();
        currentStats = new MonsterStats(baseStats);
        rigidbody = GetComponent<Rigidbody>();
        statusUI = transform.GetChild(0).GetComponent<EnemyStatusUIController>();
        visualEffectsController = transform.GetChild(1).GetComponent<EnemyVisualEffectsController>();
        animator.SetFloat("Speed", currentStats.speed);

        burningCoroutine = burnCircle();


        specificInitialize();
        randomiseStats();
    }
    public static void reset()
    {
        liveEnemies = new List<EnemyController>();
        enemiesOnField = 0;
        onEnemiesEnded = null;
    }

    protected virtual void randomiseStats()
    {
        currentStats.speed *= 1 + Random.Range(-SPEED_RANDOM_FACTOR, SPEED_RANDOM_FACTOR);
    }
    protected virtual void specificInitialize() { }
    #endregion
    protected void Update()
    {
        if (action == ActionState.DEATH)
        {
            return;
        }
        actAI();
    }
    protected virtual void actAI(){
        Debug.Log("AI is CLEAR");
    }

    protected void startMoving() {
        action = ActionState.WALK;
        animator.SetInteger("CurrentAction", (int)ActionState.WALK);
        rigidbody.velocity = transform.forward * -currentStats.speed;
    }
    protected void stopMoving() {
        action = ActionState.DEFAULT;
        animator.SetInteger("CurrentAction", (int)ActionState.DEFAULT);
        rigidbody.velocity = Vector3.zero;
    }


    public void takeDamage(float damage) {
        if (currentStats.health > 0)
        {
            currentStats.health -= damage;
            statusUI.showDamage(damage);
            statusUI.resizeHealth(currentStats.health / baseStats.health, EnemyStatusUIController.HEALTHBAR_ANIMATION.SHAKE);
        }
        if (currentStats.health <= 0)
        {   
            kill();
        }
    }
    public void kill() {
        liveEnemies.Remove(this);
        if (action == ActionState.DEATH || rigidbody == null)
        {
            return;
        }
        action = ActionState.DEATH;
        rigidbody.velocity = Vector3.zero;
        animator.SetInteger("CurrentAction", (int)ActionState.DEATH);
        if (onEnemyDeath != null)
        {
            onEnemyDeath();
        }
        Destroy(GetComponent<BoxCollider>());
    }

    public virtual void downAnimationEvent(){
        enemiesOnField--;
        if (enemiesOnField <= 0)
        {
            onEnemiesEnded();
        }
        if (GameData.upgrades.deathExplosive_lvl>0)
        {
            AreaEffectController area = Instantiate(areaEffect);
            area.transform.position = transform.position;
            area.play(AreaEffectController.Effect.EXPLOSIVE);
        }
        Destroy(gameObject);
    }
    public void setPower(float power){
        currentStats.power = power;
    }
    public void setSpeed(float speed){
        currentStats.speed = speed;
        animator.SetFloat("Speed", currentStats.speed);
        
        if(action == ActionState.WALK)
        {
            rigidbody.velocity = transform.forward * -currentStats.speed;
        }
    }
    public void addHealth(float health)
    {
        if(currentStats.health + health > baseStats.health)
        {
            currentStats.health = baseStats.health;
        }
        else
        {
            currentStats.health += health;
        }
        visualEffectsController.doHealEffect();
        statusUI.resizeHealth(currentStats.health / baseStats.health, EnemyStatusUIController.HEALTHBAR_ANIMATION.NONE);
    }
    #region Effects
    public void startBurning()
    {
        if (statusEffect.burnTime <= 0)
        {
            statusEffect.burnTime = 5;
            StartCoroutine(burningCoroutine);
        }
        else
        {
            statusEffect.burnTime = 5;
        }
    }
    protected IEnumerator burnCircle(){
        while(statusEffect.burnTime >0){
            visualEffectsController.doBurnEffect();
            yield return new WaitForSeconds(Upgrades.burn_tickDelay);
            takeDamage(GameData.shellBaseStats.damage * (Upgrades.burn_startDamagePerTickMultiplier+ Upgrades.burn_plusDamagePerTickMultiplier * GameData.upgrades.burn_lvl));
            statusEffect.burnTime -= Upgrades.burn_tickDelay;
        }
    }
    #endregion
    protected void OnTriggerEnter(Collider other)
    {
        MainWallController wall = other.transform.GetComponent<MainWallController>();
        if(wall!=null){
            wall.takeDamage(this);
            kill();
        }
    }
    public void shellHit(ShellController shell)
    {
        float damage = shell.stats.damage;
        if (currentStats.health / baseStats.health > Upgrades.fullHpDamage_startHpMin - GameData.upgrades.fullHpDamage_lvl * Upgrades.fullHpDamage_HpMinDecimal)
        {
            damage *= Upgrades.fullHpDamage_damageMultiplier * GameData.upgrades.fullHpDamage_lvl;
        }
        if (GameData.upgrades.burn_lvl > 0)
        {
            if (Random.Range(0, 100) < Upgrades.burn_startChance + Upgrades.burn_plusChance * GameData.upgrades.burn_lvl)
            {
                startBurning();
            }
        }
        if (GameData.upgrades.slow_lvl > 0)
        {
            if (Random.Range(0, 100) < GameData.upgrades.slow_getSlowChance())
            {
                setSpeed(currentStats.speed * GameData.upgrades.slow_getSlowAmount());
                Debug.Log("SLOW");
            }
        }

        takeDamage(damage);
    }
    #region Search
    public static EnemyController findClosestEnemy(float x, float z){
        EnemyController result = null;
        float closestDistance = float.PositiveInfinity;
        for(int i = 0; i<liveEnemies.Count; i++){
            if (liveEnemies[i].action == ActionState.DEATH)
            {
                continue;
            }
            float xAbstractDistance = (liveEnemies[i].transform.localPosition.x - x) * (liveEnemies[i].transform.localPosition.x - x);
            float zAbstractDistance = (liveEnemies[i].transform.localPosition.z - z) * (liveEnemies[i].transform.localPosition.z - z);
            float abstractDistance = xAbstractDistance + zAbstractDistance;
            if (abstractDistance < closestDistance)
            {
                closestDistance = abstractDistance;
                result = liveEnemies[i];
            }
        }

        return result;
    }
    public static EnemyController findClosestEnemy(Transform target){
        EnemyController result = null;
        float closestDistance = float.PositiveInfinity;
        for (int i = 0; i < liveEnemies.Count; i++)
        {
            if (liveEnemies[i].transform == target || liveEnemies[i].action==ActionState.DEATH)
            {
                continue;
            }
            float xAbstractDistance = (liveEnemies[i].transform.localPosition.x - target.localPosition.x) * (liveEnemies[i].transform.localPosition.x - target.localPosition.x);
            float zAbstractDistance = (liveEnemies[i].transform.localPosition.z - target.localPosition.z) * (liveEnemies[i].transform.localPosition.z - target.localPosition.z);
            float abstractDistance = xAbstractDistance + zAbstractDistance;
            if (abstractDistance < closestDistance)
            {
                closestDistance = abstractDistance;
                result = liveEnemies[i];
            }
        }
        return result;
    }
    public static EnemyController findClosestEnemy(EnemyController target){
        return findClosestEnemy(target.transform);
    }
    #endregion
    public abstract Vector3 getCenterPoint();
    public abstract Vector3 getHeadPoint();
}
