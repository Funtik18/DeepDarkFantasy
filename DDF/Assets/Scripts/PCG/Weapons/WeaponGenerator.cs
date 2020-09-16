using DDF.Atributes;
using DDF.Environment;
using DDF.UI.Inventory.Items;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace DDF.PCG.WEAPON
{
    public class WeaponGenerator : MonoBehaviour
    {

        //private static WeaponGenerator _instance;

        [HideInInspector] public static WeaponGenerator _instance;

        List<XmlCategory> weapons = new List<XmlCategory>();//1
        List<XmlCategory> OneHanded = new List<XmlCategory>();//1
        List<XmlCategory> TwoHanded = new List<XmlCategory>();//2
        List<XmlCategory> modsW = new List<XmlCategory>();
        List<XmlCategory> endsW = new List<XmlCategory>();

        List<XmlCategory> armors = new List<XmlCategory>();//2
        List<XmlCategory> head = new List<XmlCategory>();//1
        List<XmlCategory> torso = new List<XmlCategory>();//2
        List<XmlCategory> belt = new List<XmlCategory>();//3
        List<XmlCategory> legs = new List<XmlCategory>();//4
        List<XmlCategory> feets = new List<XmlCategory>();//5
        List<XmlCategory> shields = new List<XmlCategory>();//6
        List<XmlCategory> modsA = new List<XmlCategory>();
        List<XmlCategory> endsA = new List<XmlCategory>();

        List<XmlCategory> jewerlys = new List<XmlCategory>();//3
        List<XmlCategory> ring = new List<XmlCategory>();//1
        List<XmlCategory> wrist = new List<XmlCategory>();//2

        public Sprite[] OneH;
        public Sprite[] TwoH;

        

        public Sprite[] Head;
        public Sprite[] Torso;
        public Sprite[] Belt;
        public Sprite[] Legs;
        public Sprite[] Feets;
        public Sprite[] Shields;

        public Sprite[] Ring;
        public Sprite[] Wrist;

        [SerializeField]
        [ReadOnly]
        private string Patch = @"Resources\XML\WeaponsDescription.xml";

        /*public static WeaponGenerator GetInstance() {
            if(_instance == null) {
                _instance = FindObjectOfType<WeaponGenerator>();
                DontDestroyOnLoad(_instance);
                _instance.Init();
            }
            return _instance;
        }*/

        void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(this.gameObject);
                Init();
            }
            else if (_instance != null)
            {
                Destroy(this.gameObject);
            }
        }

        void Init() {
			string FullPatch = Application.dataPath + "\\" + Patch;

			if (!File.Exists(FullPatch)) {
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
			Parser(Doc.DocumentElement.SelectNodes("/Items/Armor/Belt/belt"), belt);
			Parser(Doc.DocumentElement.SelectNodes("/Items/Armor/Legs/leg"), legs);
			Parser(Doc.DocumentElement.SelectNodes("/Items/Armor/Feets/feet"), feets);
			Parser(Doc.DocumentElement.SelectNodes("/Items/Armor/Shields/shield"), shields);
			//Parser(Doc.DocumentElement.SelectNodes("/Items/Armor/End/end"), endsA);

			Parser(Doc.DocumentElement.SelectNodes("/Items/Armor/AvaliableJewerly/type"), jewerlys);
			Parser(Doc.DocumentElement.SelectNodes("/Items/Armor/Rings/ring "), ring);
			Parser(Doc.DocumentElement.SelectNodes("/Items/Armor/Wrists/wr "), wrist);
		}

		/// <summary>
		/// передавать инт для выбора, что сгенерировать 1-оружие 2-броня 3 - украшения
		/// </summary>
		public Item Generator(int num)
        {
            XmlCategory mod;
            XmlCategory end;
            float maxValue, minValue, valueW;
            int maxValueD, minValueD, width, rar, height;

            Sprite icon;
            ItemRarity rarity;
            //1 - оружка 2 - броня 3 - украшения
            switch (num)
            {
                case 1:
                    {
                        XmlCategory currentWeapon = GetRandom(weapons.ToArray());
                        XmlCategory typeWeapon;


                        maxValue = float.Parse(Random.Range(5f, 15f).ToString("F1"));
                        minValue = float.Parse(Random.Range(1f, 5f).ToString("F1"));

                        valueW = float.Parse(Random.Range(0.6f, 2.5f).ToString("F1"));

                        maxValueD = Random.Range(1, 100);
                        minValueD = Random.Range(1, 100);
                        width = 1;
                        rar = Random.Range(0, 4);
                        height = Random.Range(2, 4);
                        rarity = (ItemRarity)rar;
                        //1 - одноручка 2 -двуручка
                        switch (currentWeapon.id)
                        {
                            case ("1"):
                                {
                                    typeWeapon = GetRandom(OneHanded.ToArray());
                                    //Debug.Log(currentWeapon.name + " " + currentWeapon.gender);
                                    mod = GetRandomWithGender(modsW, currentWeapon.gender);
                                    //Debug.Log(mod.text + " " + mod.gender);
                                    end = GetRandom(endsW.ToArray());
                                    icon = GetRandom(OneH);
                                    return ItemCreate<OneHandedItem>(mod, currentWeapon, end, typeWeapon, rarity, maxValue, minValue, valueW, maxValueD, minValueD, width, height, icon);
                                }
                            case ("2"):
                                {
                                    typeWeapon = GetRandom(TwoHanded.ToArray());
                                    //Debug.Log(currentWeapon.name + " " + currentWeapon.gender);
                                    mod = GetRandomWithGender(modsW, currentWeapon.gender);
                                    //Debug.Log(mod.text + " " + mod.gender);
                                    end = GetRandom(endsW.ToArray());
                                    icon = GetRandom(TwoH);
                                    return ItemCreate<TwoHandedItem>(mod, currentWeapon, end, typeWeapon, rarity, maxValue * 2, minValue * 2, valueW * 2, maxValueD, minValueD, width * 2, height, icon);
                                }
                        }
                        return null;
                    }
                case (2):
                    XmlCategory currentArmor = GetRandom(armors.ToArray());
                    XmlCategory typeArmor;
                    maxValue = float.Parse(Random.Range(5f, 15f).ToString("F1"));

                    valueW = float.Parse(Random.Range(2f, 6f).ToString("F1"));

                    maxValueD = Random.Range(1, 100);
                    minValueD = Random.Range(1, 100);
                    width = 2;
                    rar = Random.Range(0, 4);
                    height = 2;
                    rarity = (ItemRarity)rar;
                    ///1-шлем 2-грудь 3- пояс 4 - поножи 5 - боты 6 - щит
                    switch (currentArmor.id)
                    {
                        case ("1"):
                            typeArmor = GetRandom(head.ToArray());
                            //Debug.Log(currentArmor.name+" "+currentArmor.gender);
                            mod = GetRandomWithGender(modsA, currentArmor.gender);
                            //Debug.Log(mod.text+" "+mod.gender);
                            end = GetRandom(endsW.ToArray());
                            icon = GetRandom(Head);
                            return ItemCreate<HeadItem>(mod, currentArmor, end, typeArmor, rarity, maxValue, valueW + (height + width), maxValueD, minValueD, width, height, icon);
                        case ("2"):
                            height += Random.Range(0, 2);
                            valueW *= 5f;
                            maxValue += (int)(height * width * 5f);
                            typeArmor = GetRandom(torso.ToArray());
                            //Debug.Log(currentArmor.name + " " + currentArmor.gender);
                            mod = GetRandomWithGender(modsA, currentArmor.gender);
                            //Debug.Log(mod.text + " " + mod.gender);
                            end = GetRandom(endsW.ToArray());
                            icon = GetRandom(Torso);
                            return ItemCreate<TorsoItem>(mod, currentArmor, end, typeArmor, rarity, maxValue, valueW + (height + width), maxValueD, minValueD, width, height, icon);
                        case ("3"):
                            height = 1;
                            valueW = float.Parse(Random.Range(0.5f, 2f).ToString("F1"));
                            maxValue = float.Parse(Random.Range(1f, 5f).ToString("F1"));
                            typeArmor = GetRandom(belt.ToArray());
                            //Debug.Log(currentArmor.name + " " + currentArmor.gender);
                            mod = GetRandomWithGender(modsA, currentArmor.gender);
                            //Debug.Log(mod.text + " " + mod.gender);
                            end = GetRandom(endsW.ToArray());
                            icon = GetRandom(Belt);
                            return ItemCreate<WaistItem>(mod, currentArmor, end, typeArmor, rarity, maxValue, valueW + (height + width), maxValueD, minValueD, width, height, icon);
                        case ("4"):
                            height = Random.Range(2, 4);
                            valueW = float.Parse(Random.Range(1f, 5f).ToString("F1"));
                            maxValue = float.Parse(Random.Range(1f, 5f).ToString("F1"));
                            typeArmor = GetRandom(legs.ToArray());
                            //Debug.Log(currentArmor.name + " " + currentArmor.gender);
                            mod = GetRandomWithGender(modsA, currentArmor.gender);
                            //Debug.Log(mod.text + " " + mod.gender);
                            end = GetRandom(endsW.ToArray());
                            icon = GetRandom(Legs);
                            return ItemCreate<LegsItem>(mod, currentArmor, end, typeArmor, rarity, maxValue, valueW + (height + width), maxValueD, minValueD, width, height, icon);
                        case ("5"):
                            valueW = float.Parse(Random.Range(1f, 5f).ToString("F1"));
                            maxValue = float.Parse(Random.Range(1f, 5f).ToString("F1"));
                            typeArmor = GetRandom(feets.ToArray());
                            //Debug.Log(currentArmor.name + " " + currentArmor.gender);
                            mod = GetRandomWithGender(modsA, currentArmor.gender);
                            //Debug.Log(mod.text + " " + mod.gender);
                            end = GetRandom(endsW.ToArray());
                            icon = GetRandom(Feets);
                            return ItemCreate<FeetItem>(mod, currentArmor, end, typeArmor, rarity, maxValue, valueW + (height + width), maxValueD, minValueD, width, height, icon);
                        case ("6"):
                            height += Random.Range(0, 2);
                            valueW *= 2.5f;
                            maxValue += (int)(height * width * 2.5f);
                            typeArmor = GetRandom(shields.ToArray());
                            //Debug.Log(currentArmor.name + " " + currentArmor.gender);
                            mod = GetRandomWithGender(modsA, currentArmor.gender);
                            //Debug.Log(mod.text + " " + mod.gender);
                            end = GetRandom(endsW.ToArray());
                            icon = GetRandom(Shields);
                            return ItemCreate<OffHandItem>(mod, currentArmor, end, typeArmor, rarity, maxValue, valueW + (height + width), maxValueD, minValueD, width, height, icon);
                    }
                    return null;
                case (3):
                    XmlCategory currentJewelry = GetRandom(jewerlys.ToArray());
                    XmlCategory typeJewelry;
                    maxValue = float.Parse(Random.Range(0.5f, 1.5f).ToString("F1"));

                    valueW = float.Parse(Random.Range(0.01f, 0.5f).ToString("F1"));

                    maxValueD = Random.Range(1, 100);
                    minValueD = Random.Range(1, 100);
                    width = 1;
                    rar = Random.Range(0, 4);
                    height = 1;
                    rarity = (ItemRarity)rar;
                    //1 - кольцо
                    switch (currentJewelry.id)
                    {
                        case ("1"):
                            typeJewelry = GetRandom(ring.ToArray());
                            //Debug.Log(currentJewelry.name+" "+ currentJewelry.gender);
                            mod = GetRandomWithGender(modsA, currentJewelry.gender);
                            //Debug.Log(mod.text+" "+mod.gender);
                            end = GetRandom(endsW.ToArray());
                            icon = GetRandom(Ring);
                            return ItemCreate<JewerlyItem>(mod, currentJewelry, end, typeJewelry, rarity, maxValue, valueW, maxValueD, minValueD, width, height, icon);
                        case ("2"):
                            width++;
                            height++;
                            maxValue = float.Parse(Random.Range(1.5f, 3f).ToString("F1"));
                            valueW = float.Parse(Random.Range(0.1f, 1.5f).ToString("F1"));
                            typeJewelry = GetRandom(wrist.ToArray());
                            //Debug.Log(currentJewelry.name+" "+ currentJewelry.gender);
                            mod = GetRandomWithGender(modsA, currentJewelry.gender);
                            //Debug.Log(mod.text+" "+mod.gender);
                            end = GetRandom(endsW.ToArray());
                            icon = GetRandom(Wrist);
                            return ItemCreate<WristItem>(mod, currentJewelry, end, typeJewelry, rarity, maxValue, valueW, maxValueD, minValueD, width, height, icon);

                    }
                    return null;
            }
            return null;
        }

        /// <summary>
        /// Создание предмета.
        /// </summary>
        private T ItemCreate<T>(XmlCategory mod, XmlCategory currentWeapon, XmlCategory end, XmlCategory typeWeapon, ItemRarity rarity, float value, float valueW, int maxValueD, int minValueD, int width, int height, Sprite icon) where T : ArmorItem
        {
            T obj = ScriptableObject.CreateInstance<T>();
            obj.itemName = mod.text + " " + currentWeapon.name + " " + end.text;
            obj.itemDescription = typeWeapon.text;
            obj.rarity = rarity;
            obj.weight = new VarFloat("Weight", valueW );
            obj.armor = new VarFloat("Armor", value);
            obj.duration = new VarMinMax<int>("Duration", minValueD, maxValueD);
            obj.itemIcon = icon;
            obj.itemWidth = width;
            obj.itemHeight = height;
            return obj;
        }
        private T ItemCreate<T>(XmlCategory mod, XmlCategory currentWeapon, XmlCategory end, XmlCategory typeWeapon, ItemRarity rarity, float maxValue, float minValue, float valueW, int maxValueD, int minValueD, int width, int height, Sprite icon) where T : WeaponItem
        {
            T obj = ScriptableObject.CreateInstance<T>();
            obj.itemName = mod.text + " " + currentWeapon.name + " " + end.text;
            obj.itemDescription = typeWeapon.text;
            obj.rarity = rarity;
            obj.weight = new VarFloat("Weight", valueW + (height + width));
            obj.damage = new VarMinMax<float>("Damage", minValue + (height * 5f), maxValue + (height * 5f));
            obj.duration = new VarMinMax<int>("Duration", minValueD, maxValueD);
            obj.itemIcon = icon;
            obj.itemWidth = width;
            obj.itemHeight = height;
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
                t.gender = item.Attributes["gender"].Value;
                //Debug.Log(item.Attributes["id"].Value + " " + item.Attributes["text"].Value + " " + item.Attributes["name"].Value + " " + item.Attributes["gender"].Value);
                temps.Add(t);
            }
        }

        private static XmlCategory GetRandomWithGender(List<XmlCategory> arr, string gender)
        {

            var matchingModules = arr.Where(m => m.gender.Equals(gender)).ToArray();
            //Debug.Log(gender);

            return GetRandom(matchingModules);
        }

        private static T GetRandom<T>(T[] array)////ПАЧИМУ ЛОМАЕТСЯ НА КОЛЬЦЕ Я НЕ ПАНИМАЮ
        {
            //Debug.Log(array + " " + array.Length);
            try
            {
                return array[UnityEngine.Random.Range(0, array.Length)];
            }
            catch
            {
                return default;
            }
        }

        #region Класс для категоризации
        private class XmlCategory
        {
            public string id;
            public string text;
            public string name;
            public string gender;
        }
        #endregion 
    }
}