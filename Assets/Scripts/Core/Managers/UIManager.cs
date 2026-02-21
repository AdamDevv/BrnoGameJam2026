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

        protected override void Initialize()
        {
            base.Initialize();
            _LevelText.transform.localScale = Vector3.zero;
        }

        public async UniTaskVoid ShowLevelText()
        {
            await _LevelText.transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack).ToUniTask(cancellationToken: destroyCancellationToken);
        }

        public async UniTaskVoid HideLevelText()
        {
            await _LevelText.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack).ToUniTask(cancellationToken: destroyCancellationToken);
        }
    }
}
