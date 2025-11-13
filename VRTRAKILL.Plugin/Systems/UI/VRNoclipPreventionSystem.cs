using VRTRAKILL.Data;
using ULTRAKILL.Cheats;
using UnityEngine;
using UnityEngine.UI;

namespace VRTRAKILL.Systems.UI;

public class VRNoclipPreventionSystem : MonoBehaviour
{
    public Transform Pivot;

    private CanvasGroup _cg;
    private Text _text;
    private bool _shouldShow;

    private readonly string[] _flavorText = new string[]
    {
        "Get out of the wall.",
        "Get the fuck out of the wall!",
        "Hey, the level is THIS way!",
        "Not gonna let you noclip out of this one!",
        "Please stop trespassing.",
        "Go back!",
        "Nothing to see here!"
    };
    private bool _shouldChangeText = false;

    public void OnEnable()
    {
        _cg = GetComponent<CanvasGroup>();
        _cg.alpha = 0;
        _text = GetComponentInChildren<Text>();
    }

    public void Update()
    {
        // check if noclip is enabled
        if (CheatsManager.Instance.GetCheatState(new Noclip().Identifier))
            return;

        if (CollisionCheck(Pivot.position, .1f, (int)Layers.Environment))
            _shouldShow = true;
        else _shouldShow = false;

        if (_shouldShow)
        {
            if (_shouldChangeText) _text.text = _flavorText[Random.Range(0, _flavorText.Length - 1)];
            _shouldChangeText = false;
            if (_cg.alpha < 1)
            {
                _cg.alpha += Time.deltaTime;
                if (_cg.alpha >= 1) _shouldShow = false;
            }
        }
        else
        {
            if (_cg.alpha > 0)
            {
                _cg.alpha -= Time.deltaTime;
                if (_cg.alpha <= 0) _shouldChangeText = true;
            }
        }
    }

    private bool CollisionCheck(Vector3 pos, float radius, int layer)
    {
        var sphere = Physics.OverlapSphere(pos, radius, 1 << layer, QueryTriggerInteraction.Ignore);
        return sphere.Length > 0;
    }
}
