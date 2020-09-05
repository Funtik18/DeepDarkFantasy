using DDF.Atributes;
using DDF.Environment;
using DDF.UI.Inventory.Items;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DDF.PCG.WEAPON
{
    public class WeaponGenerator : MonoBehaviour
    {

        List<XmlCategory> weapons = new List<XmlCategory>();//1
        List<XmlCategory> OneHanded = new List<XmlCategory>();//1
        List<XmlCategory> TwoHanded = new List<XmlCategory>();//2
        List<XmlCategory> modsW = new List<XmlCategory>();
        List<XmlCategory> endsW = new List<XmlCategory>();

        List<XmlCategory> armors = new List<XmlCategory>();//2
        List<XmlCategory> head = new List<XmlCategory>();//1
        List<XmlCategory> torso = new List<XmlCategory>();//2
        List<XmlCategory> modsA = new List<XmlCategory>();
        List<XmlCategory> endsA = new List<XmlCategory>();

        public Sprite[] OneH;
        public Sprite[] TwoH;


        public Sprite[] Head;
        public Sprite[] Torso;

        [SerializeField]
        [ReadOnly]
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

            //Weapons
            Parser(Doc.DocumentElement.SelectNodes("/Items/Weapons/AvaliableWeapons/type"), weapons);
            Parser(Doc.DocumentElement.SelectNodes("/Items/Weapons/WeaponOneHandedItem/one "), OneHanded);
            Parser(Doc.DocumentElement.SelectNodes("/Items/Weapons/WeaponTwoHandedItem/two"), TwoHanded);
            Parser(Doc.DocumentElement.SelectNodes("/Items/Weapons/Modif/mod"), modsW);
            Parser(Doc.DocumentElement.SelectNodes("/Items/Weapons/End/end"), endsW);
            //Armors
            Parser(Doc.DocumentElement.SelectNodes("/Items/Armor/AvaliableArmor/type"), armors);
            Parser(Doc.DocumentElement.SelectNodes("/Items/Armor/Head/head "), head);
            Parser(Doc.DocumentElement.SelectNodes("/Items/Armor/Torso/torso"), torso);
            Parser(Doc.DocumentElement.SelectNodes("/Items/Armor/Modif/mod"), modsA);
            //Parser(Doc.DocumentElement.SelectNodes("/Items/Armor/End/end"), endsA);
        }

        /// <summary>
        /// передавать инт для выбора, что сгенерировать 1-оружие 2-броня
        /// </summary>
        public Item Generator(int num) {

            XmlItemContent itemContent;
            XmlItemProperties itemProperties;

            XmlCategory itemPrimaryType;

            ItemRarity rarity = (ItemRarity)Random.Range(0, 4);

            switch (num) {
                case 1: {
                    itemPrimaryType = GetRandom(weapons.ToArray());
                    itemProperties = new XmlItemWeapon();
                    (itemProperties as XmlItemWeapon).Set0();
                    //1 - одноручка 2 -двуручка
                    switch (itemPrimaryType.id) {
                        case ( "1" ):
                        itemContent = new XmlItemContent(GetRandom(modsW.ToArray()), itemPrimaryType, GetRandom(OneHanded.ToArray()), GetRandom(endsW.ToArray()), GetRandom(OneH), rarity);
                        return ItemCreate<OneHandedItem>(itemContent, itemProperties);

                        case ( "2" ):
                        itemContent = new XmlItemContent(GetRandom(modsW.ToArray()), itemPrimaryType, GetRandom(TwoHanded.ToArray()), GetRandom(endsW.ToArray()), GetRandom(TwoH), rarity);
                        return ItemCreate<TwoHandedItem>(itemContent, itemProperties);
                    }
                    return null;
                }
                case 2: {
                    itemPrimaryType = GetRandom(armors.ToArray());
                    itemProperties = new XmlItemArmor();
                    (itemProperties as XmlItemArmor).Set0();
                    ///1-шлем 2-грудь
                    switch (itemPrimaryType.id) {
                        case ( "1" ):
                        itemContent = new XmlItemContent(GetRandom(modsW.ToArray()), itemPrimaryType, GetRandom(head.ToArray()), GetRandom(endsW.ToArray()), GetRandom(Head), rarity);
                        return ItemCreate<HeadItem>(itemContent, itemProperties);

                        case ( "2" ):
                        itemContent = new XmlItemContent(GetRandom(modsA.ToArray()), itemPrimaryType, GetRandom(torso.ToArray()), GetRandom(endsW.ToArray()), GetRandom(Torso), rarity);
                        return ItemCreate<TorsoItem>(itemContent, itemProperties);
                    }
                }
                return null;
            }
            return null;
        }

        /// <summary>
        /// Создание предмета.
        /// </summary>
        private T ItemCreate<T>(XmlItemContent content, XmlItemProperties properties) where T : Item
        {
            T obj = ScriptableObject.CreateInstance<T>();
            obj.itemName = content.GetNameContent();
            obj.itemIcon = content.icon;
            obj.rarity = content.rarity;
            obj.itemDescription = content.secondaryType.text;

            obj.weight = new VarFloat("Weight", properties.valueWeight + ( properties.height + properties.width));
            obj.itemWidth = properties.width;
            obj.itemHeight = properties.height;

            if (obj is WeaponItem){
                (obj as WeaponItem).damage = new VarMinMax<float>("Damage", (properties as XmlItemWeapon).minValue, ( properties as XmlItemWeapon ).maxValue);
                (obj as WeaponItem).duration = new VarMinMax<int>("Duration", properties.minValueDuration, properties.maxValueDuration);
            }
            if (obj is ArmorItem){
                (obj as ArmorItem).armor = new VarFloat("Armor", ( properties as XmlItemArmor).value);
                (obj as ArmorItem).duration = new VarMinMax<int>("Duration", properties.maxValueDuration, properties.maxValueDuration);
            }
            
            return obj;
        }

        private void Parser(XmlNodeList nodes, List<XmlCategory> temps)
        {
            foreach (XmlNode item in nodes)
            {
                XmlCategory t = new XmlCategory();
                t.id = item.Attributes["id"].Value;
                t.text = item.Attributes["text"].Value;
                t.name = item.Attributes["name"].Value;
                temps.Add(t);
            }
        }

        private static T GetRandom<T>(T[] array)
        {
            //Debug.Log(array + " " + array.Length);
            return array[UnityEngine.Random.Range(0, array.Length-1)];
        }

        #region Класс для категоризации
        private class XmlCategory{
            public string id;
            public string text;
            public string name;
        }
        private class XmlItemContent {
            public XmlCategory mod;
            public XmlCategory primaryType;
            public XmlCategory secondaryType;
            public XmlCategory end;
            public Sprite icon;
            public ItemRarity rarity;

            public XmlItemContent(XmlCategory mod, XmlCategory primaryType, XmlCategory secondaryType, XmlCategory end, Sprite icon, ItemRarity rarity) {
                this.mod = mod;
                this.primaryType = primaryType;
                this.secondaryType = secondaryType;
                this.end = end;
                this.icon = icon;
                this.rarity = rarity;
            }

            public string GetNameContent() {
                return mod.text + " " + primaryType.name + " " + end.text;
            }
        }
        private class XmlItemProperties {
            public int maxValueDuration, minValueDuration;
            public int width, height;
            public float valueWeight;
        }

        private class XmlItemWeapon : XmlItemProperties {
            public float maxValue, minValue;

            public XmlItemWeapon() {}
            public void Set0() {
                maxValue = float.Parse(Random.Range(5, 15).ToString("F1"));
                minValue = float.Parse(Random.Range(1, 5).ToString("F1"));

                valueWeight = float.Parse(Random.Range(0.6f, 2.5f).ToString("F1"));

                maxValueDuration = Random.Range(1, 100);
                minValueDuration = Random.Range(1, 100);
                width = 1;
                height = Random.Range(2, 4);
            }
        }
        private class XmlItemArmor : XmlItemProperties {
            public float value;

            public XmlItemArmor() {}
            public void Set0() {
                value = float.Parse(Random.Range(5f, 15f).ToString("F1"));

                valueWeight = float.Parse(Random.Range(2f, 6f).ToString("F1"));

                maxValueDuration = Random.Range(1, 100);
                minValueDuration = Random.Range(1, 100);

                width = 2;
                height = 2;
            }
        }
        #endregion
    }
}