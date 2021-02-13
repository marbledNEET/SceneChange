using System;
using UnityEngine;
using UniRx;
using UnityEngine.AddressableAssets;

namespace CustomMan.Common
{
    public static class TransitionManager
    {
        private static TransitionCotroller controller;

        private static TransitionCotroller Controller
        {
            get
            {
                Addressables
                    .LoadAssetAsync<GameObject>("TransitionCotroller")
                    .Completed += op =>
                    {
                        
                        if (controller == null)
                        {
                            var r = GameObject.Instantiate(op.Result) as GameObject;
                            controller = r.GetComponent<TransitionCotroller>();
                        }
                    };

                if(controller != null)
                {
                    Debug.Log("サクセス");
                    return controller;
                }
                return controller;
            }
        }


        public static void StartTransition(string _nextScene)
        {
            Debug.Log("Start!");
            Controller.Transition(_nextScene);
        }
        public static IObservable<Unit> OnTransitionFinished()
        {
            if(Controller == null)
            {
                return Observable.Return(Unit.Default);
            }

            else
            {
                if (!Controller.IsTransition.Value)
                {
                    Debug.Log(Controller.IsTransition.Value);
                    return Observable.Return(Unit.Default);
                }

                else
                {
                    Debug.Log("else");
                    return Controller.OnTransitionFinished;
                }
            }
        }
    }
}
