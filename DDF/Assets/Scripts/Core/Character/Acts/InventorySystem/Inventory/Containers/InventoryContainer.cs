using DDF.Character.Effects;
using DDF.Events;
using DDF.Help;
using DDF.UI.Inventory.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace DDF.UI.Inventory {
    /// <summary>
    /// TODO: обновлять, сортировать currentItems
    /// TODO: проверять переполнение.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(InventoryGrid))]
    [DisallowMultipleComponent]
    public class InventoryContainer : MonoBehaviour {
    }
}
