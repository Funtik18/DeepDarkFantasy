using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.UI.Bar {
    public class LevelBar : BarBase {
        private RectTransform rootRect;
        public override void SetMaxValue( float maximum ) {
            base.SetMaxValue(maximum);
            //rootRect.sizeDelta = new Vector2(10 * maxvalue, rootRect.sizeDelta.y);
        }

        protected override void Awake() {
            base.Awake();
            rootRect = GetComponent<RectTransform>();
        }
    }
}