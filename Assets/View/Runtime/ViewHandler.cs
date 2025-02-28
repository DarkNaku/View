using System.Collections;
using UnityEngine;

namespace DarkNaku.View {
    public abstract class ViewHandler<T> : MonoBehaviour, IViewHandler where T : ViewHandler<T> {
        [SerializeField] private Canvas _viewCanvas = null;

        public string Name => gameObject.name;
        public Canvas ViewCanvas => _viewCanvas;
        public CanvasGroup ViewCanvasGroup => _viewCanvasGroup;
        public bool IsInTransition { get; protected set; }

        private CanvasGroup _viewCanvasGroup = null;
        private IViewTransition ViewTransition { get; set; }

        public void Initialize() {
            if (ViewCanvas != null) {
                _viewCanvasGroup = ViewCanvas.GetComponent<CanvasGroup>();

                if (_viewCanvasGroup == null) {
                    _viewCanvasGroup = ViewCanvas.gameObject.AddComponent<CanvasGroup>();
                }
            }

            ViewTransition = GetComponent<IViewTransition>();

            OnInitialize();

            gameObject.SetActive(false);
        }

        public IEnumerator Show() {
            gameObject.SetActive(true);

            yield return CoShow();
        }

        public IEnumerator Hide() {
            yield return CoHide();

            gameObject.SetActive(false);
        }

        protected virtual void OnInitialize() {
        }

        protected virtual void OnEnterBefore() {
        }

        protected virtual void OnEnterAfter() {
        }

        protected virtual void OnExitBefore() {
        }

        protected virtual void OnExitAfter() {
        }

        private IEnumerator CoShow() {
            if (IsInTransition) {
                Debug.LogErrorFormat("[ViewHandler] CoShow : Already in transition.");
                yield break;
            }

            IsInTransition = true;

            OnEnterBefore();

            if (ViewTransition != null) {
                yield return StartCoroutine(ViewTransition.CoTransitionIn());
            }

            OnEnterAfter();

            if (ViewCanvasGroup != null) {
                ViewCanvasGroup.interactable = true;
            }

            IsInTransition = false;
        }

        private IEnumerator CoHide() {
            if (IsInTransition) {
                Debug.LogErrorFormat("[ViewHandler] CoShow : Already in transition.");
                yield break;
            }

            IsInTransition = true;

            if (ViewCanvasGroup != null) {
                ViewCanvasGroup.interactable = false;
            }

            OnExitBefore();

            if (ViewTransition != null) {
                yield return StartCoroutine(ViewTransition.CoTransitionOut());
            }

            OnExitAfter();

            IsInTransition = false;
        }
    }
}