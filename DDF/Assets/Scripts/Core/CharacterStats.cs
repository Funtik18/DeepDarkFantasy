using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DDF.Atributes;
using DDF.UI.Bar;

public class CharacterStats : MonoBehaviour {
    public float maxHP;// максимально возможное количесвто хп
    private float baseHP = 10;//базовое хп
    public float currentHP;//теукщее

    public bool dead;
    public float speed;
    public int dmg;

    [SerializeField]
    private HealthBar HPBar;
    //private HealthBar HPBar;

    [Header("Статы 1-10")]
    [InfoBox("СИЛА хитпоинты=10+сила*2 урон в ближнем бою=dmg+(2*strengh)", InfoBoxType.Normal)]
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

        HPBar.SetMaxValue(maxHP);
        HPBar.UpdateBar(maxHP);//надо будет менять на загружаемое значение

        PrintStats();
    }
    /// <summary>
    /// Delete
    /// </summary>
    public void TakeDamage( float dmg, GameObject place ) {
        int uclon = Random.Range(1, 20);
        if (uclon > agility) {
            currentHP -= dmg;
            HPBar.TakeDamage(dmg);

            Iam = place;
        } else {
            Debug.Log("Miss");
        }
    }
    public void TakeDamage( float dmg ) {
        int uclon = Random.Range(1, 20);
        if (uclon > agility) {
            currentHP -= dmg;
            HPBar.UpdateBar(currentHP);


            PrintStats();
        } else {
            Debug.Log("Miss");
        }
    }
    private void UpdateStats() {
        maxHP = baseHP + strengh * 2;

        speed = ( 2 * ( (float)agility / 10 ) ) * 1;
        dmg = dmg + ( 2 * strengh );
    }

    public void PrintStats() {
        print(maxHP + "/" + currentHP);
	}
}
