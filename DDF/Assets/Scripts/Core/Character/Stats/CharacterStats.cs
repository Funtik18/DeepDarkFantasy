using UnityEngine;
using DDF.Atributes;
using DDF.UI.Bar;
using DDF.Character.Stats;
using System;

public class CharacterStats : Stats {
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
    protected override void UpdateStats() {
        base.UpdateStats();
        if (castrat) MaxMP = 0;


        HPBar.SetMaxValue(MaxHP);
        HPBar.UpdateBar(CurrentHP);//надо будет менять на загружаемое значение

        MPBar.SetMaxValue(MaxMP);
        MPBar.UpdateBar(CurrentMP);//надо будет менять на загружаемое значение

        UpdateTXT();
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




    #region Stats
    #region HP
    public override void TakeDamage( float dmg ) {
        base.TakeDamage(dmg);
        HPBar.UpdateBar(CurrentHP);
    }
    public override void RestoreHealth( float heal ) {
        base.RestoreHealth(heal);
        HPBar.UpdateBar(CurrentHP);
    }
    #endregion
    #region MP
    public override void SpendMana( float count ) {
        base.SpendMana(count);
        MPBar.UpdateBar(CurrentMP);
    }
    public override void RestoreMana( float mana ) {
        base.RestoreMana(mana);
        MPBar.UpdateBar(CurrentMP);
    }
    #endregion
    #endregion
}
