using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Scripts
{
    public class ModifierRowPanel : MonoBehaviour
    {
        [field: SerializeField] public Image BgImage { get; private set; }
        [field: SerializeField] public Image MainImage { get; private set; }
        [field: SerializeField] public TMP_Text ModifierText { get; private set; }
    }
}