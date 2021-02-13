using UnityEngine;
using UnityEngine.UI;
using UniRx;
using CustomMan.Common;

public class TestTransition : MonoBehaviour
{
    [SerializeField]
    private string _nextScene;
    [SerializeField]
    private Button _button;

    private async void Start()
    {
        await TransitionManager.OnTransitionFinished();

        _button.OnClickAsObservable()
            .Subscribe(_ => { TransitionManager.LoadScene(_nextScene); }).AddTo(this);

    }
}
