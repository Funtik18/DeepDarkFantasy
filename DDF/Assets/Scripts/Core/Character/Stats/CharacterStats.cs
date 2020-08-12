﻿using UnityEngine;
using DDF.Atributes;
using DDF.UI.Bar;
using DDF.Character.Stats;
using System;
using System.Collections.Generic;
using UnityEngine.Events;


/// <summary>
/// Note: если onChange руглярка то нужно апдейтить дату по новой, тк current
/// </summary>
public class CharacterStats : Stats {
    [SerializeField]
    private TextsStats textsStats;

    [SerializeField]
    private HealthBar HPBar;
    [SerializeField]
    private ManaBar MPBar;
    [SerializeField]
    private LevelBar LevelBar;


    #region Setup
    protected override void Awake() {
        base.Awake();
        textsStats.Init(stats);
    }
	protected override void Start() {
        base.Start();

        onChangeSkillPoints = () => UpdateTXT();

        
        onChangeHealthPoints = delegate { UpdateData(); UpdateTXT(); } ;
        onChangeManaPoints = delegate { UpdateData(); UpdateTXT(); };
        onChangeStrength = () => UpdateTXT();
        onChangeAgility = () => UpdateTXT();
        onChangeIntelligance = () => UpdateTXT();

        onChangePhysicalArmor = delegate { UpdateData(); UpdateTXT(); };
        onChangeMagicArmor = delegate { UpdateData(); UpdateTXT(); };


        //re init stats
        CurrentLevel = 1;
        
        MaxLevelExperience = 1000;
        CurrentSkillPoints = 3;

        CurrentStrength = 5;
        CurrentAgility = 5;
        CurrentIntelligence = 5;


        base.UpdateStats();
        CurrentHealthPoints = MaxHealthPoints;
        CurrentManaPoints = MaxManaPoints;
        UpdateUI();
    }
    private void UpdateTXT() {
        textsStats.UpdateAllTXT();
    }
    protected override void UpdateStats() {
        base.UpdateStats();
        UpdateUI();
    }
	private void UpdateUI() {
        HPBar.SetMaxValue(MaxHealthPoints);
        HPBar.UpdateBar(CurrentHealthPoints);//надо будет менять на загружаемое значение

        MPBar.SetMaxValue(MaxManaPoints);
        MPBar.UpdateBar(CurrentManaPoints);//надо будет менять на загружаемое значение

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
        HPBar.UpdateBar(CurrentHealthPoints);
    }
    public override void RestoreHealth( float heal ) {
        base.RestoreHealth(heal);
        HPBar.UpdateBar(CurrentHealthPoints);
    }
    #endregion
    #region MP
    public override void SpendMana( float count ) {
        base.SpendMana(count);
        MPBar.UpdateBar(CurrentManaPoints);
    }
    public override void RestoreMana( float mana ) {
        base.RestoreMana(mana);
        MPBar.UpdateBar(CurrentManaPoints);
    }
    #endregion
    #endregion
}
