using UnityEngine;

namespace Plugin.VRTRAKILL.UI
{
    // short for GET THE FUCK OUT OF THE WALL
    // used explicitly for getting the fuck out of the wall
    internal sealed class GTFOTW : MonoSingleton<GTFOTW>
    {
        public Transform DetectorTransform;
        private CanvasGroup CG; private UnityEngine.UI.Text Text;
        public bool ShouldShow, ShouldHide;

        private string[] Texts = new string[]
        {
            "GET OUT OF THE WALL.",
            "GET THE FUCK OUT OF THE WALL.",
            "I WON'T LET YOU PEEK.",
            "DON'T MAKE ME REPEAT MYSELF.",
            "STOP IT."
        };
        private bool ChangeText = false;

        public override void OnEnable()
        {
            base.OnEnable();
            CG = GetComponent<CanvasGroup>();
            Text = GetComponentInChildren<UnityEngine.UI.Text>();
        }

        public void Update()
        {
            if (Util.Misc.DetectCollisions(DetectorTransform.position, 1, (int)Layers.Environment) >= 1)
                ShouldShow = true;
            else ShouldHide = true;

            if (ShouldShow)
            {
                ShouldHide = false;
                if (ChangeText) Text.text = Texts[Random.Range(0, Texts.Length - 1)];
                if (CG.alpha < 1)
                {
                    CG.alpha += Time.deltaTime;
                    if (CG.alpha >= 1) { ShouldShow = false; ChangeText = false; }
                }
            }
            if (ShouldHide)
            {
                ShouldShow = false;
                if (CG.alpha > 0)
                {
                    CG.alpha -= Time.deltaTime;
                    if (CG.alpha <= 0) { ShouldHide = false; ChangeText = true; }
                }
            }
        }
    }
}
