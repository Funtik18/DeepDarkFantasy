using UnityEngine;
using DDF.Atributes;
using DDF.UI.Bar;
using DDF.Character.Stats;
using System.Collections.Generic;
using System.Linq;

public class CharacterStats : MonoBehaviour {

    [Header("Статы 1-10")]
    [SerializeField]
    private ValueStructure statsStructure;

    public Stats stats;

    #region Здоровье
    private Value valueMaxHP;
    private float maxHP;
    public float MaxHP {
        get {
            return maxHP;
        }
        set {
            maxHP = value;
            if (maxHP <= 0) maxHP = 0;
            if (maxHP < CurrentHP) CurrentHP = maxHP;
        }
    }// максимально возможное количесвто хп
    [SerializeField]
    [ReadOnly]
    private float currentHP;
    public float CurrentHP {
        get {
            return currentHP;
        }
        set {
            currentHP = value;
            if (currentHP >= MaxHP) currentHP = MaxHP;
            if (currentHP <= 0) currentHP = 0;
        }
    }//теукщее
    #endregion
    #region Мана
    private Value valueMaxMP;
    private float maxMP;
    public float MaxMP {
        get {
            return maxMP;
        }
        set {
            maxMP = value;
            if (maxMP <= 0) maxMP = 0;
            if (maxMP < CurrentMP) CurrentMP = maxHP;
        }
    }
    [SerializeField]
    [ReadOnly]
    private float currentMP;
    public float CurrentMP {
        get {
            return currentMP;
        }
        set {
            currentMP = value;
            if (currentMP >= maxMP) currentMP = maxMP;
            if (currentMP <= 0) currentMP = 0;
        }
    }
    #endregion

    #region Сила
    private Value valueStrength;
    [SerializeField]
    [ReadOnly]
    private float currentStrength;
    public float CurrentStrength {
        get {
            return currentStrength;
        }
        set {
            currentStrength = value;
        }
    }//теукщее
    #endregion

    #region Сила
    private Value valueIntelligence;
    /*[SerializeField]
    [ReadOnly]
    private int currentStrength;
    public int CurrentStrength {
        get {
            return currentStrength;
        }
        set {
            currentStrength = value;
        }
    }*/
    #endregion

    public bool dead = false;
    public bool castrat = false;//не может владеть магией если true

    [SerializeField]
    private HealthBar HPBar;
    [SerializeField]
    private ManaBar MPBar;


    #region Setup
    private void Awake() {
        InitValues();
        InitFormulas();
    }
    void Start() {
        AssignValues();

        int temp = 0;
        stats.Get(valueMaxHP, out temp);
        CurrentHP = MaxHP = temp;
        HPBar.SetMaxValue(MaxHP);
        HPBar.UpdateBar(MaxHP);

        //currentHP = maxHP;
        temp = 0;
        stats.Get(valueMaxMP, out temp);
        CurrentMP = MaxMP = temp;
        MPBar.SetMaxValue(MaxMP);
        MPBar.UpdateBar(MaxMP);
    }


    private void InitValues() {
        stats = new Stats();

        List<ValueStat> valuestats = statsStructure.stats;

        for (int i = 0; i < valuestats.Count; i++) {

            Value value = valuestats[i].value;
            float points = valuestats[i].count;

            if (value is ValueFloat) {
                stats.valueList.Add(new ValueFloatReference(value, points));

            }
            if (value is ValueInt) {
                stats.valueList.Add(new ValueIntReference(value, Mathf.FloorToInt(points)));
            }
        }
    }
    private void InitFormulas() {
        List<ValueReference> references = stats.valueList;
        for (int i = 0; i < references.Count; i++) {

            ValueReference reference = references[i];

            Formula formula = reference.valueBase.formula;
            Value value = reference.valueBase;

            if (formula) {//если в стате есть формула
                reference.Null();
                if (formula is FormulaInt) {
                    FormulaInt formulaInt = (FormulaInt)formula;
                    stats.Sum(value, formulaInt.Calculate(stats));
                }
                if (formula is FormulaFloat) {
                    FormulaFloat formulaInt = (FormulaFloat)formula;
                    stats.Sum(value, formulaInt.Calculate(stats));
                }

                List<Value> valuesref = formula.GetRefernces();

                for (int j = 0; j < valuesref.Count; j++) {
                    stats.SubscribeOnRecalculate(ValueRecalculate, value, valuesref[j]);
                }
            }
        }
    }

    private void ValueRecalculate( Value value ) {
        ValueReference valueNull = stats.GetValueReference(value);
        valueNull.Null();

        Formula formula = value.formula;

        if (formula) {
            if (formula is FormulaInt) {
                FormulaInt formulaInt = (FormulaInt)formula;

                stats.Sum(value, formulaInt.Calculate(stats));
            }
            if (formula is FormulaFloat) {
                FormulaFloat formulaInt = (FormulaFloat)formula;

                stats.Sum(value, formulaInt.Calculate(stats));
            }
        }
    }

    #endregion


    /*//Дополнительные пойнты для здоровья
    private float baseAHP = 0;
    private float maxAHP;
    public float MaxAHP {
        get {
            return maxAHP;
        }
        set {
            maxAHP = value;
            if (maxAHP <= 0) maxAHP = 0;
        }
    }
    [SerializeField]
    [ReadOnly]
    private float currentAHP;
    public float CurrentAHP {
        get {
            return currentAHP;
        }
        set {
            currentAHP = value;
            if (currentAHP >= MaxAHP) currentAHP = MaxAHP;
            if (currentAHP <= 0) currentAHP = 0;
        }
    }*/

    public float speed;
    public float dmg;

    [HideInInspector]
    public GameObject Iam;

    
    /// <summary>
    /// Delete
    /// </summary>
    public void TakeDamage( float dmg, GameObject place ) {
        /*int uclon = Random.Range(1, 20);
        if (uclon > agility) {
            currentHP -= dmg;
            Iam = place;
        } else {
            Debug.Log("Miss");
        }*/
    }

	#region HP
	public void TakeDamage( float dmg ) {
        int uclon = Random.Range(1, 20);
        //if (uclon > agility) {
            CurrentHP -= dmg;
            HPBar.UpdateBar(CurrentHP);
        //} else {
        //    Debug.Log("Miss");
       // }
    }
    public void RestoreHealth( float heal ) {
        CurrentHP += heal;
        HPBar.UpdateBar(CurrentHP);
    }

    public void IncreaseHPLimitOn(float buf) {
        MaxHP += buf;
        HPBar.SetMaxValue(MaxHP);
        HPBar.UpdateBar(CurrentHP);
    }
    public void DecreaseHPLimitOn( float buf ) {
        MaxHP -= buf;
        HPBar.SetMaxValue(MaxHP);
        HPBar.UpdateBar(CurrentHP);
    }
    #endregion

    #region MP
    public void SpendMana( float count ) {
        CurrentMP -= count;
        MPBar.UpdateBar(CurrentMP);
    }
    public void RestoreMana( float mana ) {
        CurrentMP += mana;
        MPBar.UpdateBar(CurrentMP);
    }

    public void IncreaseMPLimitOn( float buf ) {
        if (castrat) return;
        maxMP += buf;
        MPBar.SetMaxValue(maxMP);
        MPBar.UpdateBar(CurrentMP);
    }
    public void DecreaseMPLimitOn( float buf ) {
        if (castrat) return;
        maxMP -= buf;
        MPBar.SetMaxValue(maxMP);
        MPBar.UpdateBar(CurrentMP);
    }
    #endregion

    private void AssignValues() {
        List<ValueReference> values = stats.valueList;

        valueMaxHP = values[0].valueBase;
        valueMaxMP = values[1].valueBase;

        valueStrength = values[2].valueBase;

        valueIntelligence = values[4].valueBase;
    }

    public void IncreaceStrength() {
        stats.Sum(valueStrength, 1);
        UpdateStats();

        int temp = 0;
        stats.Get(valueMaxHP, out temp);
        MaxHP = temp;
        HPBar.SetMaxValue(MaxHP);
        HPBar.UpdateBar(CurrentHP);//надо будет менять на загружаемое значение
    }
    public void DecreaceStrength() {
        stats.Sum(valueStrength, -1);
        UpdateStats();

        int temp = 0;
        stats.Get(valueMaxHP, out temp);
        MaxHP = temp;
        HPBar.SetMaxValue(MaxHP);
        HPBar.UpdateBar(CurrentHP);//надо будет менять на загружаемое значение
    }

    public void IncreaceIntelligence() {
        stats.Sum(valueIntelligence, 1);
        UpdateStats();
        int temp = 0;

        stats.Get(valueMaxMP, out temp);
        MaxMP = temp;
        MPBar.SetMaxValue(MaxMP);
        MPBar.UpdateBar(CurrentMP);//надо будет менять на загружаемое значение
    }
    public void DecreaceIntelligence() {
        stats.Sum(valueIntelligence, -1);
        UpdateStats();
        int temp = 0;

        stats.Get(valueMaxMP, out temp);
        MaxMP = temp;
        MPBar.SetMaxValue(MaxMP);
        MPBar.UpdateBar(CurrentMP);//надо будет менять на загружаемое значение
    }

    private void UpdateStats() {
        

        

        /*
                if (castrat) maxMP = 0;
                else maxMP = baseMP + intelegence * 2;

                speed = ( 2 * ( (float)agility / 10 ) ) * 1;
                dmg = dmg + ( 2 * strengh );*/

    }

    public void PrintStats() {
        Debug.Log("\nCharacterStats" + "\n"
            + MaxHP + "/" + CurrentHP + "\n"
            + maxMP + "/" + CurrentMP + "\n");
    }
}
