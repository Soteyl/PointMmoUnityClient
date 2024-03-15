using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Components.UI
{
    public class HoverColorChanger: SerializedMonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [OdinSerialize]
        private Image _image;

        [OdinSerialize]
        private Color _idleColor;

        [OdinSerialize]
        private Color _hoverColor;

        private void Awake()
        {
            _image.color = _idleColor;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _image.color = _hoverColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _image.color = _idleColor;
        }
    }
}