﻿using DDF.Atributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DDF.UI.Customization {
    public class Pages : MonoBehaviour {
        public List<Page> pages;
		private Page currentPage;
		[SerializeField][ReadOnly]
		private int currentIndex;
		public int CurrentIndex {
			get {
				return currentIndex;
			}
			set {
				currentIndex = value;

				if (currentPage != null) currentPage.DeleteActions();

				if (currentIndex < 0) currentIndex = 0;
				if (currentIndex >= pages.Count) currentIndex = pages.Count - 1;

				currentPage = pages[currentIndex];

				if (currentIndex == pages.Count - 1) {
					currentPage.SetActions(Back, AcceptChanges);
				} else {
					currentPage.SetActions(Back, Next);
				}
			}
		}
		private void Awake() {
			if(pages == null) {
				pages = new List<Page>();
				pages = GetComponentsInChildren<Page>().ToList();
			}
		}
		private void Start() {
			CurrentIndex = 0;
			DisablePages();
			UpdatePage();
		}

		public void UpdatePage() {
			DisablePages();
			currentPage.EnablePage();
		}

		public void DisablePages() {
			for (int i = 0; i < pages.Count; i++) {
				pages[i].DisablePage();
			}
		}

		private void AcceptChanges() {
			print("+");
		}
		private void Next() {
			CurrentIndex++;
			DisablePages();
			UpdatePage();
		}
		private void Back() {
			CurrentIndex--;
			DisablePages();
			UpdatePage();
		}
	}
}