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

#if UNITY_EDITOR
        [InitializeOnLoadMethod]
        private static void PackageImportHandler() {
            var define = $"DARKNAKU_{MethodBase.GetCurrentMethod().DeclaringType.Assembly.GetName().Name.ToUpper()}";

            System.Array buildTargets = System.Enum.GetValues(typeof(BuildTarget));

            foreach (BuildTarget target in buildTargets) {
                var buildTargetGroup = BuildPipeline.GetBuildTargetGroup(target);

                if (BuildPipeline.IsBuildTargetSupported(buildTargetGroup, target) == false) continue;

#if UNITY_2023_1_OR_NEWER
                var namedBuildTarget = UnityEditor.Build.NamedBuildTarget.FromBuildTargetGroup(buildTargetGroup);
                var defines = PlayerSettings.GetScriptingDefineSymbols(namedBuildTarget);
#else
                var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
#endif

                if (defines.IndexOf(define) > 0) continue;

#if UNITY_2023_1_OR_NEWER
				PlayerSettings.SetScriptingDefineSymbols(namedBuildTarget, $"{defines};{define}".Replace(";;", ";"));
#else
                PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, $"{defines};{define}".Replace(";;", ";"));
#endif
            }
        }
#endif

        public static IViewHandler Change(string viewName) {
            return Instance._Change<IViewHandler>(viewName);
        }

        public static T Change<T>(string viewName) where T : class, IViewHandler {
            return Instance._Change<T>(viewName);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnBeforeSceneLoad() {
            _instance = null;
            Debug.Log("[View] OnBeforeSceneLoad");
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void OnAfterSceneLoad() {
            Debug.Log("[View] OnAfterSceneLoad");
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

        private void OnDestroy() {
            if (_instance != this) return;

            _instance = null;

            Debug.Log($"[View] Destroyed.");
        }

        private void Initialize() {
            _viewTable = new Dictionary<string, IViewHandler>();

            for (int i = 0; i < transform.childCount; i++) {
                var view = transform.GetChild(i).GetComponent<IViewHandler>();

                if (view != null) {
                    view.Initialize();
                    _viewTable.Add(view.Name, view);
                }
            }

            name = "[Singleton] View";
        }

        private T _Change<T>(string viewName) where T : class, IViewHandler {
            if (_viewTable.ContainsKey(viewName) == false) {
                Debug.LogErrorFormat("[View] Change : Not on view table - {0}", viewName);
                return null;
            }

            IViewHandler handler = _viewTable[viewName];

            StartCoroutine(CoChange(handler));

            return handler as T;
        }

        private IEnumerator CoChange(IViewHandler handler) {
            yield return null;

            if (CurrentView != null) {
                yield return CurrentView.Hide();
            }

            CurrentView = handler;
            yield return CurrentView.Show();
        }
    }
}