﻿using UnityEngine;

namespace VRBasePlugin.ULTRAKILL.UI
{
    // short for GET THE FUCK OUT OF THE WALL
    // used explicitly for getting the fuck out of the wall
    internal sealed class GTFOTW : MonoBehaviour
    {
        public Transform DetectorTransform;
        private CanvasGroup CG; private UnityEngine.UI.Text Text;
        private bool ShouldShow;

        private readonly string[] Texts = new string[]
        {
            "GET OUT OF THE WALL.",
            "GET THE FUCK OUT OF THE WALL.",
            "DON'T MAKE ME REPEAT MYSELF."
        };
        private bool ChangeText = false;

        public void OnEnable()
        {
            CG = GetComponent<CanvasGroup>();
            CG.alpha = 0;
            Text = GetComponentInChildren<UnityEngine.UI.Text>();
        }

        public void Update()
        {
            if (VRTRAKILL.Utilities.Unity.DetectCollisions(DetectorTransform.position, .1f, (int)Layers.Environment) > 0)
                ShouldShow = true;
            else ShouldShow = false;

            if (ShouldShow)
            {
                if (ChangeText) Text.text = Texts[Random.Range(0, Texts.Length - 1)];
                ChangeText = false;
                if (CG.alpha < 1)
                {
                    CG.alpha += Time.deltaTime;
                    if (CG.alpha >= 1) ShouldShow = false;
                }
            }
            else
            {
                if (CG.alpha > 0)
                {
                    CG.alpha -= Time.deltaTime;
                    if (CG.alpha <= 0) ChangeText = true;
                }
            }
        }
    }
}
