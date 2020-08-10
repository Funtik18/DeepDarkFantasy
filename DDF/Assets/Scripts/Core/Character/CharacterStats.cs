using UnityEngine;
using DDF.Atributes;
using DDF.UI.Bar;
using DDF.Character.Stats;
using System.Collections.Generic;


public class CharacterStats : MonoBehaviour {

    public ValueStructure statsStructure;

    public Stats stats;

    #region Setup

    private void Awake() {
        InitValues();
        InitFormulas();
    }

    private void InitValues() {
        stats = new Stats();

        List<Value> values = statsStructure.values;

        for (int i = 0; i < values.Count; i++) {

            Value value = values[i];

            if (value is ValueFloat) {
                stats.valueList.Add(new ValueFloatReference(value, 0f));

            }
            if (value is ValueInt) {
                stats.valueList.Add(new ValueIntReference(value, 0));
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
                    stats.SubscribeOnRecalculate(ValueRecalculate, value, valuesref[i]);
                }
            }
        }
    }


    private void ValueRecalculate( Value value ) {
        ValueReference valueNull = stats.GetValueReference(value);
        valueNull.Null();

        List<ValueReference> references = stats.valueList;

        for (int i = 0; i < references.Count; i++) {
            ValueReference reference = references[i];

            Formula formula = reference.valueBase.formula;

            if (formula) {
                if (formula is FormulaInt) {
                    FormulaInt formulaInt = (FormulaInt)formula;

                    stats.Sum(reference.valueBase, formulaInt.Calculate(stats));
                }
                if (formula is FormulaFloat) {
                    FormulaFloat formulaInt = (FormulaFloat)formula;

                    stats.Sum(reference.valueBase, formulaInt.Calculate(stats));
                }
            }
        }
    }

    #endregion


    //Здоровье
    private float baseHP = 10;//базовое хп
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
            if (currentHP <=0) currentHP = 0;
        }
    }//теукщее

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

    //Мана
    private float baseMP = 5;
    private float maxMP;
    [SerializeField]
    [ReadOnly]
    private float currentMP;
    public float CurrentMP {
        get {
            return currentMP;
        }
        set {
            currentMP = value;
            if (currentMP >= maxMP) {
                currentMP = maxMP;
            }
            if (currentMP <= 0) {
                currentMP = 0;
            }
        }
    }

    [SerializeField]
    private HealthBar HPBar;
    //[SerializeField]
    //private HealthBar APBar;
    [SerializeField]
    private ManaBar MPBar;


    public bool dead = false;
    public bool castrat = false;//не может владеть магией если true

    public float speed;
    public float dmg;

    [InfoBox("СИЛА хитпоинты=10+сила*2 урон в ближнем бою=dmg+(2*strengh)", InfoBoxType.Normal)]
    [Header("Статы 1-10")]
    [Range(1, 10)]
    public int strengh = 1;

    [InfoBox("ЛОВКОСТЬ шанс уворота=рандом 20 до значения ловкости. скорость бега=(2*((float)agility/10))*1", InfoBoxType.Normal)]
    [Range(1, 10)]
    public int agility = 1;

    [InfoBox("ИНТЕЛЛЕКТ дополнительный маг урон=интеллект/2 дополнительное количество скилл поинтов при получение уровня= 15+интеллект*2", InfoBoxType.Normal)]
    [Range(1, 10)]
    public int intelegence = 1;

    [HideInInspector]
    public GameObject Iam;

    void Start() {
        UpdateStats();
        currentHP = MaxHP;
        currentMP = maxMP;

        HPBar.SetMaxValue(MaxHP);
        HPBar.UpdateBar(MaxHP);//надо будет менять на загружаемое значение

        MPBar.SetMaxValue(maxMP);
        MPBar.UpdateBar(maxMP);//надо будет менять на загружаемое значение
    }
    /// <summary>
    /// Delete
    /// </summary>
    public void TakeDamage( float dmg, GameObject place ) {
        int uclon = Random.Range(1, 20);
        if (uclon > agility) {
            currentHP -= dmg;
            Iam = place;
        } else {
            Debug.Log("Miss");
        }
    }

	#region HP
	public void TakeDamage( float dmg ) {
        int uclon = Random.Range(1, 20);
        if (uclon > agility) {
            CurrentHP -= dmg;
            HPBar.UpdateBar(CurrentHP);
        } else {
            Debug.Log("Miss");
        }
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

    private void UpdateStats() {
        MaxHP = baseHP + strengh * 2;

        if (castrat) maxMP = 0;
        else maxMP = baseMP + intelegence * 2;

        speed = ( 2 * ( (float)agility / 10 ) ) * 1;
        dmg = dmg + ( 2 * strengh );
    }

    public void PrintStats() {
        Debug.Log("\nCharacterStats" + "\n"
            + MaxHP + "/" + CurrentHP + "\n"
            + maxMP + "/" + CurrentMP + "\n");
    }
}
