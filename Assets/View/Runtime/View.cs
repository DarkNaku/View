using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using System.Reflection;
#endif

namespace DarkNaku.View {
    public sealed class View : MonoBehaviour {
        public static View Instance {
            get {
                lock (_lock) {
                    if (_instance == null) {
                        var instances = FindObjectsByType<View>(FindObjectsInactive.Include, FindObjectsSortMode.None);

                        if (instances.Length > 0) {
                            _instance = instances[0];

                            for (int i = 1; i < instances.Length; i++) {
                                Debug.LogWarningFormat("[View] Instance Duplicated - {0}", instances[i].name);
                                Destroy(instances[i]);
                            }

                        } else {
                            _instance = new GameObject($"[Singleton] View").AddComponent<View>();
                        }

                        _instance.Initialize();
                    }

                    return _instance;
                }
            }
        }

        public IViewHandler CurrentView { get; private set; }

        private Dictionary<string, IViewHandler> _viewTable = null;
        private static readonly object _lock = new();
        private static View _instance;

        private bool _isInitialized;
        private bool _isReleased;
        private float _escapePressedTime;

        public static IViewHandler Change(string viewName) {
            return Instance._Change<IViewHandler>(viewName);
        }

        public static T Change<T>(string viewName) where T : class, IViewHandler {
            return Instance._Change<T>(viewName);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnBeforeSceneLoad() {
            _instance = null;
            // Debug.Log("[View] OnBeforeSceneLoad");
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void OnAfterSceneLoad() {
            // Debug.Log("[View] OnAfterSceneLoad");
        }

        private void Awake() {
            if (_instance == null) {
                _instance = this;
                Initialize();
            } else if (_instance != this) {
                Debug.LogWarning($"[View] Duplicated - {name}");
                Destroy(gameObject);
            }
        }
        
        private void Update() {
            if (Input.GetKeyUp(KeyCode.Escape)) Escape();
        }

        private void Escape() {
            if (CurrentView == null) return;
            if (CurrentView.IsInTransition) return;
            if (Time.realtimeSinceStartup - _escapePressedTime < 1f) return;
#if DARKNAKU_POPUP
            if (Popup.Popup.IsAnyPopupShow) return;
#endif

            CurrentView.OnEscape();

            _escapePressedTime = Time.realtimeSinceStartup;
        }

        private void OnDestroy() {
            if (_instance != this) return;

            Release();

            _instance = null;

            Debug.Log("[View] Destroyed.");
        }

        private void Initialize() {
            if (_isInitialized) return;

            _viewTable = new Dictionary<string, IViewHandler>();

            for (int i = 0; i < transform.childCount; i++) {
                var view = transform.GetChild(i).GetComponent<IViewHandler>();

                if (view != null) {
                    view.Initialize();
                    _viewTable.Add(view.Name, view);
                }
            }

            _isInitialized = true;

            Debug.Log("[View] Initialized.");
        }

        private void Release() {
            if (_isReleased) return;

            foreach (var view in _viewTable.Values) {
                if (view != null && !ReferenceEquals(view, null)) {
                    view.Release();
                }
            }

            _isReleased = true;
        }

        private T _Change<T>(string viewName) where T : class, IViewHandler {
            if (_viewTable.ContainsKey(viewName) == false) {
                Debug.LogErrorFormat("[View] Change : Not on view table - {0}", viewName);
                return null;
            }

            var handler = _viewTable[viewName];

            StartCoroutine(CoChange(handler));

            return handler as T;
        }

        private IEnumerator CoChange(IViewHandler handler) {
            yield return null;

            if (CurrentView != null) {
                yield return CurrentView.Hide();
            }

            CurrentView = handler;

#if DARKNAKU_POPUP
            Popup.Popup.MainCanvas = CurrentView.ViewCanvas;
#endif

            yield return CurrentView.Show();
        }
    }
}