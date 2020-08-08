using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DDF.UI.Bar {
    
    public class HealthBar : BarBase {

        private float maxvalue = 100;

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


        public void TakeDamage(float damage) {
            float normalized = 1/(maxvalue / damage);

            DecreaseOn(normalized);
		    
            if(CurrentCount == 0) {
                beyondMin?.Invoke();
            }

            amount = 0;
        }
        public void RestoreHealth( float heal ) {
            float normalized = 1/(maxvalue / heal);

			if (amount > 0) {
                overMaxLastMax?.Invoke();
            }

            IncreaseOn(normalized);

            if (CurrentCount == 1) {
                overMax?.Invoke();
                amount++;
            }
        }

        public void UpdateBar(float currentValue) {
            float normalized = 1/(maxvalue / currentValue);
            CurrentCount = normalized;
        }
    }
}