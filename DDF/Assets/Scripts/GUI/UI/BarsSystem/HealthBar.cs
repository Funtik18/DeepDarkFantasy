using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DDF.UI.Bar {
    
    public class HealthBar : BarBase {

        private float maxvalue = 100;

        private float lastvalue = 0;

        /// <summary>
        /// События, если вышел за минимум.
        /// </summary>
        [HideInInspector] public UnityAction beyondMin;

        /// <summary>
        /// Событие, если преодалел максимум.
        /// </summary>
        [HideInInspector] public UnityAction overMax;

        /// <summary>
        /// Событие, если преодалел максимум повторно.
        /// </summary>
        [HideInInspector] public UnityAction overMaxLastMax;
        private int amount = 0;

        public void SetMaxValue(float maximum) {
            maxvalue = maximum;
        }
        
        public void UpdateBar(float currentValue) {
            float normalized = 1/(maxvalue / currentValue);//в проеделах от 0 до 1
            CurrentCount = normalized;
            SeeForActions();
        }

        private void SeeForActions() {
            //...
            lastvalue = CurrentCount;
        }
    }
}