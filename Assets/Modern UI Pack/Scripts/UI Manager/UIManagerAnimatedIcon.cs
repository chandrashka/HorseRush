﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.MUIP
{
    [ExecuteInEditMode]
    public class UIManagerAnimatedIcon : MonoBehaviour
    {
        [Header("Settings")]
        public UIManager UIManagerAsset;

        [Header("Resources")]
        public List<GameObject> images = new();
        public List<GameObject> imagesWithAlpha = new();

        private void Awake()
        {
            if (UIManagerAsset == null) { UIManagerAsset = Resources.Load<UIManager>("MUIP Manager"); }

            enabled = true;

            if (UIManagerAsset.enableDynamicUpdate == false)
            {
                UpdateAnimatedIcon();
                enabled = false;
            }
        }

        private void Update()
        {
            if (UIManagerAsset == null) { return; }
            if (UIManagerAsset.enableDynamicUpdate == true) { UpdateAnimatedIcon(); }
        }

        private void UpdateAnimatedIcon()
        {
            for (int i = 0; i < images.Count; ++i)
            {
                if (images[i] == null)
                    continue;

                Image currentImage = images[i].GetComponent<Image>();
                currentImage.color = UIManagerAsset.animatedIconColor;
            }

            for (int i = 0; i < imagesWithAlpha.Count; ++i)
            {
                if (imagesWithAlpha[i] == null)
                    continue;

                Image currentAlphaImage = imagesWithAlpha[i].GetComponent<Image>();
                currentAlphaImage.color = new Color(UIManagerAsset.animatedIconColor.r, UIManagerAsset.animatedIconColor.g, UIManagerAsset.animatedIconColor.b, currentAlphaImage.color.a);
            }
        }
    }
}