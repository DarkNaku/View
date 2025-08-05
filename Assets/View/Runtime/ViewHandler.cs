using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
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
        private List<ViewElement<T>> _viewElements;
        private bool _isInitialized;
        private bool _isReleased;

        public void Initialize() {
            if (_isInitialized) return;

            ViewTransition = GetComponent<IViewTransition>();

            _viewElements = new List<ViewElement<T>>(GetComponents<ViewElement<T>>(true));

            OnInitialize();

            for (int i = 0; i < _viewElements.Count; i++) {
                _viewElements[i].Initialize(this as T);
            }

            gameObject.SetActive(false);
            
            _isInitialized = true;
        }

        public void Release() {
            if (_isReleased) return;

            for (int i = 0; i < _viewElements.Count; i++) {
                if (_viewElements[i] != null && !ReferenceEquals(_viewElements[i], null)) {
                    _viewElements[i].Release();
                }
            }

            _viewElements.Clear();

            OnRelease();

            _isReleased = true;
        }

        public IEnumerator Show() {
            gameObject.SetActive(true);

            yield return CoShow();
        }

        public IEnumerator Hide() {
            yield return CoHide();

            gameObject.SetActive(false);
        }

        public void OnEscape() {
        }

        protected virtual void OnInitialize() {
        }

        protected virtual void OnRelease() {
        }

        protected virtual void OnEnterBefore() {
        }

        protected virtual void OnEnterAfter() {
        }

        protected virtual void OnExitBefore() {
        }

        protected virtual void OnExitAfter() {
        }
        
        private void OnDestroy() {
            Release();
        }

        private IEnumerator CoShow() {
            if (IsInTransition) {
                Debug.LogErrorFormat("[ViewHandler] CoShow : Already in transition.");
                yield break;
            }

            IsInTransition = true;

            EnterBefore();

            if (ViewTransition != null) {
                yield return StartCoroutine(ViewTransition.CoTransitionIn());
            }

            EnterAfter();

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

            ExitBefore();

            if (ViewTransition != null) {
                yield return StartCoroutine(ViewTransition.CoTransitionOut());
            }

            ExitAfter();

            IsInTransition = false;
        }

        private void EnterBefore() {
            OnEnterBefore();

            for (int i = 0; i < _viewElements.Count; i++) {
                _viewElements[i].EnterBefore();
            }
        }

        private void EnterAfter() {
            OnEnterAfter();

            for (int i = 0; i < _viewElements.Count; i++) {
                _viewElements[i].EnterAfter();
            }
        }

        private void ExitBefore() {
            OnExitBefore();

            for (int i = 0; i < _viewElements.Count; i++) {
                _viewElements[i].ExitBefore();
            }
        }

        private void ExitAfter() {
            OnExitAfter();

            for (int i = 0; i < _viewElements.Count; i++) {
                _viewElements[i].ExitAfter();
            }
        }

        private U[] GetComponents<U>(bool recusively = false) {
            var queue = new Queue<Transform>();
            var components = ListPool<U>.Get();

            for (int i = 0; i < transform.childCount; i++) {
                queue.Enqueue(transform.GetChild(i));
            }

            while (queue.Count > 0) {
                var child = queue.Dequeue();
                var childComponents = child.GetComponents<U>();

                if (components != null) {
                    components.AddRange(childComponents);
                }

                if (recusively) {
                    for (int j = 0; j < child.childCount; j++) {
                        queue.Enqueue(child.GetChild(j));
                    }
                }
            }

            var result = components.ToArray();

            ListPool<U>.Release(components);

            return result;
        }
    }
}