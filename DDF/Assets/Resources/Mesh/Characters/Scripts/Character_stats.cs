using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DDF.Atributes;

public class Character_stats : MonoBehaviour
{
    public float HP;
    public bool dead;
    public float speed;
    public int dmg;

    [InfoBox("Статы 1-10", InfoBoxType.Normal)]
    [InfoBox("СИЛА хитпоинты=10+сила*2 урон в ближнем бою=dmg+(2*strengh)", InfoBoxType.Normal)]
    public int strengh = 1;

    [InfoBox("ЛОВКОСТЬ шанс уворота=рандом 20 до значения ловкости. скорость бега=(2*((float)agility/10))*1", InfoBoxType.Normal)]
    public int agility = 1;

    [InfoBox("ИНТЕЛЛЕКТ дополнительный маг урон=интеллект/2 дополнительное количество скилл поинтов при получение уровня= 15+интеллект*2", InfoBoxType.Normal)]
    public int intelegence = 1;

    [HideInInspector]
    public GameObject Iam;
    // Start is called before the first frame update
    public void getHit(int dmg,GameObject place){
        int uclon = Random.Range(1,20);
        if(uclon>agility)
            {HP -= dmg; 
            Iam = place;
            }else
            {
                Debug.Log("Miss");
            }
    }

    void Start()
    {
        UpDateStat();
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    private void UpDateStat(){
        HP = HP+strengh*2;
        speed = (2*((float)agility/10))*1;
        dmg = dmg+(2*strengh);
    }
}
