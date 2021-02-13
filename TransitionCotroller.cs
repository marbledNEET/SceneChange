using System;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UniRx;
using Cysharp.Threading.Tasks;


namespace CustomMan.Common
{
    public class TransitionCotroller : MonoBehaviour
    {
        [SerializeField]
        private Image _coverImage;
        [SerializeField]
        private float _transitionSeconds = 1f;

        private BoolReactiveProperty _isTransition = new BoolReactiveProperty(false);
        public IReadOnlyReactiveProperty<bool> IsTransition => _isTransition;

        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
        
        public IObservable<Unit> OnTransitionFinished
        {
            get
            {
                if (!_isTransition.Value)
                {
                    return Observable.Return(Unit.Default);
                }
                return _isTransition.FirstOrDefault(x => !x).AsUnitObservable();
            }
        }

        public void Transition(string _nextScene)
        {
            if (_isTransition.Value)
            {
                return;
            }

            _isTransition.Value = true ;
            Transition(_nextScene, this.GetCancellationTokenOnDestroy()).Forget();
        }

        private async UniTaskVoid Transition(string _nextScene, CancellationToken token)
        {
            var time = _transitionSeconds;

            _coverImage.raycastTarget = true;
            Debug.Log("White");
            //画面を徐々に白くする
            while (time > 0)
            {
                time -= Time.deltaTime;
                _coverImage.color = OverrideColorAlpha(_coverImage.color,1.0f - time / _transitionSeconds);
                Debug.Log(_coverImage.color);
                await UniTask.Yield();
            }

            _coverImage.color = OverrideColorAlpha(_coverImage.color, 1.0f);
            await SceneManager.LoadSceneAsync(_nextScene);

            time = _transitionSeconds;
            Debug.Log("Return");
            //画面を
            while(time >0)
            {
                time -= Time.deltaTime;
                _coverImage.color = OverrideColorAlpha(_coverImage.color,time / _transitionSeconds);
                Debug.Log(_coverImage.color);
                await UniTask.Yield();
            }

            _coverImage.raycastTarget = false;
            _coverImage.color = OverrideColorAlpha(_coverImage.color, 0.0f);

            _isTransition.Value = false;
        }

        private Color OverrideColorAlpha(Color c, float a)
        {
            return new Color(c.r, c.g, c.b, a);
        }
    }
}