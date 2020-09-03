using DDF.UI.Inventory.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

namespace DDF.PCG.WEAPON
{
    public class WeaponGenerator : MonoBehaviour
    {

        List<AvaliableWeapons> weapons = new List<AvaliableWeapons>();
        List<Weapon> OneHanded = new List<Weapon>();//1
        List<Weapon> TwoHanded = new List<Weapon>();//2
        List<Modif> mods = new List<Modif>();
        List<End> ends = new List<End>();

        public Sprite[] OneH;
        public Sprite[] TwoH;

        [SerializeField]
        private string Patch = @"Resources\XML\WeaponsDescription.xml";

        void Awake()
        {
            string FullPatch = Application.dataPath + "\\" + Patch;

            if (!File.Exists(FullPatch))
            {
                Debug.LogError("File not found");
                return;
                //throw new FileNotFoundException(Patch);
            }
            XmlDocument Doc = new XmlDocument();
            Doc.Load(FullPatch);
            //XmlNodeList nodes = Doc.DocumentElement.SelectNodes("/Weapons/AvaliableWeapons/type");
            AvaliableWeapons weapon = new AvaliableWeapons();
            Parser(Doc.DocumentElement.SelectNodes("/Weapons/AvaliableWeapons/type"), weapons, weapon);
            Weapon onew = new Weapon();
            Parser(Doc.DocumentElement.SelectNodes("/Weapons/WeaponOneHandedItem/one "), OneHanded, onew);
            Weapon twow = new Weapon();
            Parser(Doc.DocumentElement.SelectNodes("/Weapons/WeaponTwoHandedItem/two"), TwoHanded, twow);
            Modif mod = new Modif();
            Parser(Doc.DocumentElement.SelectNodes("/Weapons/Modif/mod"), mods, mod);
            End end = new End();
            Parser(Doc.DocumentElement.SelectNodes("/Weapons/End/end"), ends, end);
            Generator(1);

        }

        private static TItem GetRandom<TItem>(TItem[] array)
        {
            //Debug.Log(array + " " + array.Length);
            return array[UnityEngine.Random.Range(0, array.Length)];
        }

        //передавать инт для выбора, что сгенерировать 1-оружие 2-броня
        public Item Generator(int num)
        {
            switch (num)
            {
                case (1):

                    AvaliableWeapons currentWeapon = GetRandom(weapons.ToArray());
                    Weapon typeWeapon = new Weapon();
                    Modif mod = new Modif();
                    End end = new End();
                    float maxDamage=0;
                    float minDamage=0;
                    System.Random random = new System.Random();
                    int rar = random.Next(0, 4);
                    Sprite icon;
                    ItemRarity rarity = new ItemRarity();
                    switch (currentWeapon.id)//1 - одноручка 2 -двуручка
                    {
                        case ("1"):
                            typeWeapon = GetRandom(OneHanded.ToArray());
                            mod = GetRandom(mods.ToArray());
                            end= GetRandom(ends.ToArray());
                            rarity = (ItemRarity)rar;
                            maxDamage = UnityEngine.Random.Range(5, 15);
                            minDamage= UnityEngine.Random.Range(1, 5);
                            
                            icon = GetRandom(OneH);

                            //OneHandedItem obj = ScriptableObject.CreateInstance<OneHandedItem>();
                            return ItemCreate(ScriptableObject.CreateInstance<OneHandedItem>(), mod, currentWeapon, end, typeWeapon, rarity, maxDamage, minDamage,icon);
                        case ("2"):
                            typeWeapon = GetRandom(TwoHanded.ToArray());
                            mod = GetRandom(mods.ToArray());
                            end = GetRandom(ends.ToArray());
                            rarity = (ItemRarity)rar;
                            maxDamage = UnityEngine.Random.Range(10, 30);
                            
                            minDamage = UnityEngine.Random.Range(2, 10);
                            icon = GetRandom(TwoH);
                            //TwoHandedItem obj = ScriptableObject.CreateInstance<TwoHandedItem>();
                            return ItemCreate(ScriptableObject.CreateInstance<TwoHandedItem>(), mod, currentWeapon, end, typeWeapon, rarity, maxDamage, minDamage,icon);
                    }

                    return null;
                case (2):
                    return null;
            }
            return null;
        }

        #region Создание предмета
        TwoHandedItem ItemCreate(TwoHandedItem obj, Modif mod, AvaliableWeapons currentWeapon, End end, Weapon typeWeapon, ItemRarity rarity, float maxDamage, float minDamage, Sprite icon)
        {
            //Debug.Log(mod.text + " " + currentWeapon.name + " " + end.text);
            //Debug.Log(typeWeapon.text + "\n" + mod.name);

            obj.itemName = mod.text + " " + currentWeapon.name + " " + end.text;
            obj.itemDescription = typeWeapon.text;
            obj.itemAnotation = mod.name;
            obj.rarity = rarity;
            obj.damage.max = maxDamage;
            obj.damage.min = minDamage;
            obj.itemIcon = icon;
            obj.itemWidth = 2;
            obj.itemHeight = 2;
            Debug.Log(maxDamage);
            return obj;
        }

        OneHandedItem ItemCreate(OneHandedItem obj,Modif mod, AvaliableWeapons currentWeapon, End end, Weapon typeWeapon, ItemRarity rarity, float maxDamage, float minDamage,Sprite icon)
        {
            //Debug.Log(mod.text + " " + currentWeapon.name + " " + end.text);
            //Debug.Log(typeWeapon.text + "\n" + mod.name);
            
            obj.itemName = mod.text + " " + currentWeapon.name + " " + end.text;
            obj.itemDescription = typeWeapon.text;
            obj.itemAnotation = mod.name;
            obj.rarity = rarity;
            obj.damage.max  = maxDamage;
            obj.damage.min = minDamage;
            obj.itemIcon = icon;
            obj.itemWidth = 1;
            obj.itemHeight = 2;
            Debug.Log(obj.damage.max);
            return obj;
        }
        #endregion

        #region Перегрузки парсеров
        private void Parser(XmlNodeList nodes, List<End> temps, End temp)
        {
            foreach (XmlNode item in nodes)
            {
                End t = new End();
                t.id = item.Attributes["id"].Value;
                t.text = item.Attributes["text"].Value;
                t.name = item.Attributes["name"].Value;
                temps.Add(t);
            }
        }

        private void Parser(XmlNodeList nodes, List<Modif> temps, Modif temp)
        {
            foreach (XmlNode item in nodes)
            {
                Modif t = new Modif();
                t.id = item.Attributes["id"].Value;
                t.text = item.Attributes["text"].Value;
                t.name = item.Attributes["name"].Value;
                temps.Add(t);
            }
        }


        private void Parser(XmlNodeList nodes, List<Weapon> temps, Weapon temp)
        {
            foreach (XmlNode item in nodes)
            {
                Weapon t = new Weapon();
                t.id = item.Attributes["id"].Value;
                t.text = item.Attributes["text"].Value;
                t.name = item.Attributes["name"].Value;
                temps.Add(t);
            }
        }

        void Parser(XmlNodeList nodes, List<AvaliableWeapons> temps, AvaliableWeapons temp)
        {
            foreach (XmlNode item in nodes)
            {
                AvaliableWeapons t = new AvaliableWeapons();
                t.id = item.Attributes["id"].Value;
                t.text = item.Attributes["text"].Value;
                t.name = item.Attributes["name"].Value;
                temps.Add(t);
            }
        }
        #endregion

        #region Классы для категоризации
        class AvaliableWeapons
        {
            public string id;
            public string text;
            public string name;
        }

        class Weapon
        {
            public string id;
            public string text;
            public string name;
        }

        class Modif
        {
            public string id;
            public string text;
            public string name;
        }
        class End
        {
            public string id;
            public string text;
            public string name;
        }
        #endregion 

    }
}