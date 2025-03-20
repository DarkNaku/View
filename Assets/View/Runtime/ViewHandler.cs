using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DarkNaku.View {
    public abstract class ViewHandler<T> : MonoBehaviour, IViewHandler where T : ViewHandler<T> {
        [SerializeField] private Canvas _viewCanvas = null;

        public string Name => gameObject.name;
        public Canvas ViewCanvas => _viewCanvas;
        
        public bool Interactable {
            get => ViewGraphicRaycaster != null && ViewGraphicRaycaster.enabled;
            set {
                if (ViewGraphicRaycaster != null) {
                    ViewGraphicRaycaster.enabled = value;
                }
            }
        }

        public bool IsInTransition { get; protected set; }

        private IViewTransition ViewTransition { get; set; }

        private GraphicRaycaster ViewGraphicRaycaster => _viewGraphicRaycaster ??= ViewCanvas?.GetComponent<GraphicRaycaster>();

        private GraphicRaycaster _viewGraphicRaycaster;

        public void Initialize() {
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

            Interactable = true;
            IsInTransition = false;
        }

        private IEnumerator CoHide() {
            if (IsInTransition) {
                Debug.LogErrorFormat("[ViewHandler] CoShow : Already in transition.");
                yield break;
            }

            IsInTransition = true;
            Interactable = false;

            OnExitBefore();

            if (ViewTransition != null) {
                yield return StartCoroutine(ViewTransition.CoTransitionOut());
            }

            OnExitAfter();

            IsInTransition = false;
        }
    }
}