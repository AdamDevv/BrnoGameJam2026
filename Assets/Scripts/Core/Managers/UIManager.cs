using System;
using APX.Managers.GameObjects;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace APGame.Managers
{
    public class UIManager : ASingleton<UIManager>
    {
        [SerializeField] [Required] private TMP_Text _LevelText;

        public TMP_Text LevelText => _LevelText;

        private Vector3 _levelTextShowPosition;
        private Vector3 _levelTextHidePosition;

        protected override void Initialize()
        {
            base.Initialize();
            _levelTextShowPosition = _LevelText.rectTransform.anchoredPosition;
            _levelTextHidePosition = _levelTextShowPosition - Vector3.right * (_LevelText.rectTransform.sizeDelta.x + _levelTextShowPosition.x);
            _LevelText.rectTransform.anchoredPosition = _levelTextHidePosition;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                ShowLevelText();
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                HideLevelText();
            }
        }

        public void ShowLevelText() => UniTask.Void(async cancellationToken => {
            _LevelText.rectTransform.anchoredPosition = _levelTextHidePosition;
            // await UniTask.WaitForSeconds(0.1f, cancellationToken: cancellationToken);
            await _LevelText.rectTransform.DOAnchorPos(_levelTextShowPosition, .45f).SetEase(Ease.OutBack, 0.9f).ToUniTask(cancellationToken: cancellationToken);
        }, destroyCancellationToken);

        public void HideLevelText() => UniTask.Void(async cancellationToken => {
            _LevelText.rectTransform.anchoredPosition = _levelTextShowPosition;
            await _LevelText.rectTransform.DOAnchorPos(_levelTextHidePosition, 0.3f).SetEase(Ease.OutBack).ToUniTask(cancellationToken: cancellationToken);
        }, destroyCancellationToken);
    }
}
