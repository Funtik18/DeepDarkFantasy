using DDF.Atributes;
using DDF.UI.Inventory.Items;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

namespace DDF.PCG.WEAPON {
    public class WeaponGenerator : MonoBehaviour {

        List<XmlCategory> weapons = new List<XmlCategory>();
        List<XmlCategory> OneHanded = new List<XmlCategory>();//1
        List<XmlCategory> TwoHanded = new List<XmlCategory>();//2
        List<XmlCategory> mods = new List<XmlCategory>();
        List<XmlCategory> ends = new List<XmlCategory>();

        public Sprite[] OneH;
        public Sprite[] TwoH;

        [SerializeField][ReadOnly]
        private string Patch = @"Resources\XML\WeaponsDescription.xml";

        void Awake() {
            string FullPatch = Application.dataPath + "\\" + Patch;

            if (!File.Exists(FullPatch)) {
                Debug.LogError("File not found");
                return;
                //throw new FileNotFoundException(Patch);
            }
            XmlDocument Doc = new XmlDocument();
            Doc.Load(FullPatch);
            //XmlNodeList nodes = Doc.DocumentElement.SelectNodes("/Weapons/AvaliableWeapons/type");

            Parser(Doc.DocumentElement.SelectNodes("/Weapons/AvaliableWeapons/type"), weapons );
            Parser(Doc.DocumentElement.SelectNodes("/Weapons/WeaponOneHandedItem/one "), OneHanded);
            Parser(Doc.DocumentElement.SelectNodes("/Weapons/WeaponTwoHandedItem/two"), TwoHanded);
            Parser(Doc.DocumentElement.SelectNodes("/Weapons/Modif/mod"), mods);
            Parser(Doc.DocumentElement.SelectNodes("/Weapons/End/end"), ends);
        }

        //передавать инт для выбора, что сгенерировать 1-оружие 2-броня
        public Item Generator(int num){
            XmlCategory currentWeapon = GetRandom(weapons.ToArray());
            switch (num){
                 case  1: {
                    XmlCategory typeWeapon = new XmlCategory();
                    XmlCategory mod = new XmlCategory();
                    XmlCategory end = new XmlCategory();
                    float maxValue = UnityEngine.Random.Range(5, 15);
                    float minValue = UnityEngine.Random.Range(1, 5);
                    int rar = UnityEngine.Random.Range(0, 4);
                    Sprite icon;
                    ItemRarity rarity = (ItemRarity)rar;
                    //1 - одноручка 2 -двуручка
                    switch (currentWeapon.id){
                        case ( "1" ): {
                            typeWeapon = GetRandom(OneHanded.ToArray());
                            mod = GetRandom(mods.ToArray());
                            end = GetRandom(ends.ToArray());
                            icon = GetRandom(OneH);
                            return ItemCreate(ScriptableObject.CreateInstance<OneHandedItem>(), mod, currentWeapon, end, typeWeapon, rarity, maxValue, minValue, icon);
                        }
                        case ( "2" ): {
                            typeWeapon = GetRandom(TwoHanded.ToArray());
                            mod = GetRandom(mods.ToArray());
                            end = GetRandom(ends.ToArray());
                            icon = GetRandom(TwoH);
                            return ItemCreate(ScriptableObject.CreateInstance<TwoHandedItem>(), mod, currentWeapon, end, typeWeapon, rarity, maxValue, minValue, icon);
                        }
                    }
                    return null;
                }
                case (2):
                     return null;
             }
            return null;
        }

        /// <summary>
        /// Создание предмета.
        /// </summary>
        private T ItemCreate<T>(T obj, XmlCategory mod, XmlCategory currentWeapon, XmlCategory end, XmlCategory typeWeapon, ItemRarity rarity, float maxValue, float minValue, Sprite icon) where T : Item {
            obj.itemName = mod.text + " " + currentWeapon.name + " " + end.text;
            obj.itemDescription = typeWeapon.text;
            obj.itemAnotation = mod.name;
            obj.rarity = rarity;
            if (obj is WeaponItem) {
                //( obj as WeaponItem ).damage = new Character.Stats.VarMinMaxFloat("Damage", minValue, maxValue);
                //( obj as WeaponItem ).duration = new Character.Stats.VarMinMaxInt("Duration", (int)minValue, (int)maxValue);
            }
            if (obj is ArmorItem) {
               // ( obj as ArmorItem ).armor = new Character.Stats.VarMinMaxFloat("Armor", minValue, maxValue);
               // ( obj as ArmorItem ).duration = new Character.Stats.VarMinMaxInt("Duration", (int)minValue, (int)maxValue);
            }
            obj.itemIcon = icon;
            obj.itemWidth = 2;
            obj.itemHeight = 2;
            return obj;
        }

        private void Parser(XmlNodeList nodes, List<XmlCategory> temps) {
            foreach (XmlNode item in nodes) {
                XmlCategory t = new XmlCategory();
                t.id = item.Attributes["id"].Value;
                t.text = item.Attributes["text"].Value;
                t.name = item.Attributes["name"].Value;
                temps.Add(t);
            }
        }

        private static T GetRandom<T>(T[] array) {
            //Debug.Log(array + " " + array.Length);
            return array[UnityEngine.Random.Range(0, array.Length)];
        }

        #region Класс для категоризации
        private class XmlCategory {
            public string id;
            public string text;
            public string name;
        }
        #endregion 
    }
}