using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DDF.UI.GUI {
    public class NavigationBar : MonoBehaviour {
        public NavigationText text;

        public List<NavigationPage> pages;

		public int startPage;
		private int currentPage;
		private void Awake() {

			for (int i = 0; i < pages.Count; i++) {
				pages[i].pageId = i;
				pages[i].onClick = SetCurrentPage;
			}

			SetCurrentPage(startPage);
		}
		public void SetCurrentPage( int currentId ) {
			currentPage = currentId;
			NavigationPage page = pages[currentPage];
			text.SetText(page.pageName, page.navigationButton.curentColor);
			CloseAllPages();
			page.OpenPage();
		}

		public void CloseAllPages() {
			for (int i = 0; i < pages.Count; i++) {
				pages[i].ClosePage();
			}
		}
	}
}