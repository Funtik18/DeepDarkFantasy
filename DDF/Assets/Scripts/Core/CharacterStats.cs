using UnityEngine;
using DDF.Atributes;
using DDF.UI.Bar;

public class CharacterStats : MonoBehaviour {

    private float maxHP;// максимально возможное количесвто хп
    private float baseHP = 10;//базовое хп
    [SerializeField]
    [ReadOnly]
    private float currentHP;
    public float CurrentHP { 
        get {
            return currentHP;
        }
        set {
            currentHP = value;
			if (currentHP >= maxHP) {
                currentHP = maxHP;
            }
            if (currentHP <=0) {
                currentHP = 0;
            }
        }
    }//теукщее

    private float maxMP;
    private float baseMP = 5;
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
    }//теукщее

    [SerializeField]
    private HealthBar HPBar;
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
        currentHP = maxHP;
        currentMP = maxMP;

        HPBar.SetMaxValue(maxHP);
        HPBar.UpdateBar(maxHP);//надо будет менять на загружаемое значение

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

            PrintStats();
        } else {
            Debug.Log("Miss");
        }
    }

    public void RestoreHealth( float heal ) {
        CurrentHP += heal;
        HPBar.UpdateBar(CurrentHP);
    }

    public void IncreaseHPLimitOn(float buf) {
        maxHP += buf;
        HPBar.SetMaxValue(maxHP);
        HPBar.UpdateBar(CurrentHP);
    }
    public void DecreaseHPLimitOn( float buf ) {
        maxHP -= buf;
        HPBar.SetMaxValue(maxHP);
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
        maxHP = baseHP + strengh * 2;

        if (castrat) maxMP = 0;
        else maxMP = baseMP + intelegence * 2;

        speed = ( 2 * ( (float)agility / 10 ) ) * 1;
        dmg = dmg + ( 2 * strengh );
    }

    public void PrintStats() {
        Debug.Log("\nCharacterStats" + "\n"
            + maxHP + "/" + CurrentHP + "\n"
            + maxMP + "/" + CurrentMP + "\n");
    }
}
