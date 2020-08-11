using UnityEngine;
using DDF.Atributes;
using DDF.UI.Bar;
using DDF.Character.Stats;
using System;

public class CharacterStats : MonoBehaviour {
    [Header("Статы")]

    #region Здоровье
    [SerializeField] private Stat HP;
    private float baseHP = 10;
    private float maxHP;
    public float MaxHP {
        get {
            return maxHP;
        }
        set {
            maxHP = value;
            if (maxHP <= 0) maxHP = 0;
            if (maxHP < CurrentHP) CurrentHP = maxHP;
            onChangeHP?.Invoke();
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
            onChangeHP?.Invoke();
        }
    }//теукщее

    public Action onChangeHP;
    #endregion

    #region Мана
    [SerializeField] private Stat MP;
    private float baseMP = 5;
    private float maxMP;
    public float MaxMP {
        get {
            return maxMP;
        }
        set {
            maxMP = value;
            if (maxMP <= 0) maxMP = 0;
            if (maxMP < CurrentMP) CurrentMP = maxHP;
            onChangeMP?.Invoke();
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
            onChangeMP?.Invoke();
        }
    }

    public Action onChangeMP;
    #endregion

    private int statMin = 1;

    #region Сила
    [SerializeField] private Stat strength;
    [ReadOnly] public int currentStrength;
    public int Strength {
		get {
            return currentStrength;
		}
		set {
            currentStrength = value;
            if (currentStrength < statMin) currentStrength = statMin;
            onChangeStrength?.Invoke();
        }
	}

    public Action onChangeStrength;
    #endregion
    #region Ловкость
    [SerializeField] private Stat agility;
    [ReadOnly] public int currentAgility;
    public int Agility {
        get {
            return currentAgility;
        }
        set {
            currentAgility = value;
            if (currentAgility < statMin) currentAgility = statMin;
            onChangeAgility?.Invoke();
        }
    }

    public Action onChangeAgility;
    #endregion
    #region Интелект
    [SerializeField] private Stat intelligence;
    [ReadOnly] public int currentIntelligence;
    public int Intelligence {
        get {
            return currentIntelligence;
        }
        set {
            currentIntelligence = value;
            if (currentIntelligence < statMin) currentIntelligence = statMin;
            onChangeIntelligance?.Invoke();
        }
    }

    public Action onChangeIntelligance;
    #endregion

    private float basemeleeDamage = 5;
    public float meleeDamage;

    private float baseavoid = 5;
    private float avoid;

    private float basespeed = 5;
    public float speed;

    public bool dead = false;
    public bool castrat = false;//не может владеть магией если true

    [SerializeField]
    private HealthBar HPBar;
    [SerializeField]
    private ManaBar MPBar;
    [SerializeField]
    private TextsStats textsStats;


    #region Setup

    void Start() {
        //коопии статов
        //GetCopyStats();
        //взяли дату
        GetData();

        onChangeHP = () => UpdateTXT();
        onChangeMP = () => UpdateTXT();
        onChangeStrength = () => UpdateTXT();
        onChangeAgility = () => UpdateTXT();
        onChangeIntelligance = () => UpdateTXT();

        UpdateStats();

        UpdateTXT();
    }
    public void UpdateTXT() {
        textsStats.UpdateAllTxt();
    }

    private void GetData() {
        currentHP = MaxHP = HP.amount;
        currentMP = MaxMP = MP.amount;
        currentStrength = (int)strength.amount;
        currentAgility = (int)agility.amount;
        currentIntelligence = (int)intelligence.amount;
    }
    private void SetData() {
        HP.amount = maxHP;
        MP.amount = maxMP;
        strength.amount = currentStrength;
        agility.amount = currentAgility;
        intelligence.amount = currentIntelligence;
    }
    private void SaveData() {

	}
    private void LoadData() {

    }
    private void GetCopyStats() {
        HP = HP.GetStatCopy();
        MP = MP.GetStatCopy();
        strength = strength.GetStatCopy();
        agility = agility.GetStatCopy();
        intelligence = intelligence.GetStatCopy();
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
        CurrentHP -= dmg;
        HPBar.UpdateBar(CurrentHP);
    }
    public void RestoreHealth( float heal ) {
        CurrentHP += heal;
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

    /*public void IncreaseMPLimitOn( float buf ) {
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
    }*/
    #endregion


    #region Stats
    private void UpdateStats() {
        //подсчёт
        MakeFormules();
        
        if (castrat) maxMP = 0;


        HPBar.SetMaxValue(MaxHP);
        HPBar.UpdateBar(CurrentHP);//надо будет менять на загружаемое значение

        MPBar.SetMaxValue(MaxMP);
        MPBar.UpdateBar(CurrentMP);//надо будет менять на загружаемое значение

        SetData();
        UpdateTXT();
    }
    private void MakeFormules() {
        MaxHP = baseHP + Strength * 2;
        MaxMP = baseMP + Intelligence * 2;
        meleeDamage = basemeleeDamage + Strength * 2;
        avoid = baseavoid + Agility / 2;
        speed = basespeed + Agility / 2;
    }


    public void IncreaceStrength() {
        Strength++;
        UpdateStats();
    }
    public void DecreaceStrength() {
        Strength--;
        UpdateStats();
    }

    public void IncreaceAgility() {
        Agility++;
        UpdateStats();
    }
    public void DecreaceAgility() {
        Agility--;
        UpdateStats();
    }


    public void IncreaceIntelligence() {
        Intelligence++;
        UpdateStats();
    }
    public void DecreaceIntelligence() {
        Intelligence--;
        UpdateStats();
    }
    #endregion
}
