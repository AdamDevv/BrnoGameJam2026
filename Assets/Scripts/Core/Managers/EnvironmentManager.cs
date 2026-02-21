using System;
using System.Collections.Generic;
using System.Linq;
using APGame.Enums;
using APGame.Models.DTO;
using APGame.Services.Settings;
using APX.Extra.Misc;
using APX.Managers.GameObjects;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace APGame.Managers
{
    public class EnvironmentManager : ASingleton<EnvironmentManager>
    {
        [SerializeField] [Required] private Transform _EnvironmentLayersRoot;
        private readonly List<GameObject> _layers = new();

        private void Start()
        {
            foreach (Transform child in _EnvironmentLayersRoot)
            {
                Destroy(child.gameObject);
            }

            foreach (EnvironmentData e in GameplaySettings.Instance.Levels.First().EnvironmentData)
            {
                AddEnvironmentLayer(e.EnvironmentPrefab);
            }
        }

        public void AddEnvironmentLayer(GameObject environmentPrefab)
        {
            var envLayer = Instantiate(environmentPrefab, _EnvironmentLayersRoot);
            _layers.Add(envLayer);
            envLayer.transform.localPosition = Vector3.forward * (-_layers.Count * 0.01f);
        }

        public async UniTask AddAndAnimateEnvironmentLayers(EnvironmentData[] environmentDatas)
        {
            var animationTasks = new List<UniTask>();
            foreach (var data in environmentDatas)
            {
                AddEnvironmentLayer(data.EnvironmentPrefab);
                animationTasks.Add(PerformLastLayerAppearAnimation(data.EnvironmentLayerAppearAnimation));
                await UniTask.WaitForSeconds(0.2f);
            }

            await UniTask.WhenAll(animationTasks);
        }


        private async UniTask PerformLastLayerAppearAnimation(EnvironmentLayerAppearAnimation appearAnimation)
        {
            var layer = _layers.Last();

            // Zoom out
            // layer.transform.localScale = Vector3.one * 20;
            // await layer.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutCubic).ToUniTask(cancellationToken: destroyCancellationToken);

            const float slideAnimDuration = 1f;
            const Ease slideAnimEase = Ease.OutCubic;
            const float slideAnimStartDistance = 100;

            switch (appearAnimation)
            {
                case EnvironmentLayerAppearAnimation.SlideFromBottom:
                    layer.transform.localPosition = layer.transform.localPosition.WithY(-slideAnimStartDistance);
                    await layer.transform.DOLocalMove(layer.transform.localPosition.WithY(0), slideAnimDuration).SetEase(slideAnimEase).ToUniTask(cancellationToken: destroyCancellationToken);
                    break;
                case EnvironmentLayerAppearAnimation.SlideFromTop:
                    layer.transform.localPosition = layer.transform.localPosition.WithY(slideAnimStartDistance);
                    await layer.transform.DOLocalMove(layer.transform.localPosition.WithY(0), slideAnimDuration).SetEase(slideAnimEase).ToUniTask(cancellationToken: destroyCancellationToken);
                    break;
                case EnvironmentLayerAppearAnimation.SlideFromLeft:
                    layer.transform.localPosition = layer.transform.localPosition.WithX(-slideAnimStartDistance);
                    await layer.transform.DOLocalMove(layer.transform.localPosition.WithX(0), slideAnimDuration).SetEase(slideAnimEase).ToUniTask(cancellationToken: destroyCancellationToken);
                    break;
                case EnvironmentLayerAppearAnimation.SlideFromRight:
                    layer.transform.localPosition = layer.transform.localPosition.WithX(slideAnimStartDistance);
                    await layer.transform.DOLocalMove(layer.transform.localPosition.WithX(0), slideAnimDuration).SetEase(slideAnimEase).ToUniTask(cancellationToken: destroyCancellationToken);
                    break;
                case EnvironmentLayerAppearAnimation.ZoomOut:
                    layer.transform.localScale = Vector3.one * 20;
                    await layer.transform.DOScale(Vector3.one, slideAnimDuration).SetEase(Ease.OutCubic).ToUniTask(cancellationToken: destroyCancellationToken);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(appearAnimation), appearAnimation, null);
            }
        }
    }
}
