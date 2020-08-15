using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.UI.GUI {
    public class NavigationBar : MonoBehaviour {
        public NavigationText text;

        public List<NavigationButton> buttons;

		private List<string> pages;
		public int startPage;
		private int currentPage;
		private void Awake() {
			pages = new List<string>();
			pages.Add("Инвентарь");
			pages.Add("Местоположение");
			pages.Add("Задания");
			pages.Add("Алхимя");
			pages.Add("Журнал");
			pages.Add("Глюсари");
			for (int i = 0; i < buttons.Count; i++) {
				buttons[i].id = i;
				buttons[i].onGetThis = SetCurrentPage;
				buttons[i].onClick = OnClick;
			}

			currentPage = startPage;
		}
		public void SetCurrentPage( NavigationButton navigationButton ) {
			NavigationButton navigation = navigationButton;

			currentPage = navigation.id;
			text.SetText(pages[currentPage], navigation.curentColor);
		}
		public void OnClick() {
		}
	}
}