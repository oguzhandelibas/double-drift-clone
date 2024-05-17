using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DoubleDrift.UIModule;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

namespace ODProjects.UICreator.Scripts.Editor
{
    public class UICreatorWindow : EditorWindow
    {
        private List<UiDrawer> _contents = new List<UiDrawer>() { };
        private List<bool> _selections = new List<bool>();

        private List<string> _contentNames = new List<string>()
        {
            "Home UI",
            "Game UI",
            "Progress Slide",
            "Progress Stage",
            "Level Failed UI",
            "Level Completed UI",
            "Chest Fill",
            "Popup Bonus",
            "Unlock UI",
            "Chest UI",
            "Shop UI"
        };

        private List<string> _contentPaths = new List<string>()
        {
            "Previews/home_prev",
            "Previews/game_prev",
            "Previews/progress_slide_prev",
            "Previews/progress_stage_prev",
            "Previews/level_failed_prev",
            "Previews/level_completed_prev",
            "Previews/chest_fill_prev",
            "Previews/popup_bonus_prev",
            "Previews/unlock_prev",
            "Previews/chest_prev",
            "Previews/shop_prev"
        };

        private List<string> _contentComponents = new List<string>()
        {
            "HomeUI",
            "GameUI",
            "ProgressSlide",
            "ProgressStage",
            "LevelFailedUI",
            "LevelCompletedUI",
            "ChestFill",
            "PopupBonus",
            "UnlockUI",
            "ChestUI",
            "ShopUI"
        };

        private GameObject UIManagerObject;
        private List<GameObject> _prefabs = new List<GameObject>();
        
        private GUIStyle titleStyle;

        private int selectedContentIndex;

        private void OnEnable()
        {
            titleStyle = new GUIStyle()
            {
                fontSize = 30,
                alignment = TextAnchor.UpperCenter,
                fontStyle = FontStyle.Bold,
            };
            titleStyle.normal.textColor = Color.green;

            // Init contents
            InitContents();
        }

        /// <summary>
        /// Initializes selectable contents of UI Creator
        /// </summary>
        private void InitContents()
        {
            for (int i = 0; i < _contentNames.Count; i++)
            {
                UiDrawer content = new UiDrawer();
                content.Name = _contentNames[i];
                content.Preview = Resources.Load<Texture2D>(_contentPaths[i]);
                _contents.Add(content);

                _selections.Add(false);
            }

            UIManagerObject = Resources.Load<GameObject>("Prefabs/UIManager");

            _prefabs = new List<GameObject>()
            {
                Resources.Load<GameObject>("Prefabs/HomeUI"),
                Resources.Load<GameObject>("Prefabs/GameUI"),
                Resources.Load<GameObject>("Prefabs/ProgressBarSliderPanel"),
                Resources.Load<GameObject>("Prefabs/ProgressBarStagePanel"),
                Resources.Load<GameObject>("Prefabs/LevelFailedUI"),
                Resources.Load<GameObject>("Prefabs/LevelCompletedUI"),
                Resources.Load<GameObject>("Prefabs/ChestFillPanel"),
                Resources.Load<GameObject>("Prefabs/PopupBonusPanel"),
                Resources.Load<GameObject>("Prefabs/UnlockUI"),
                Resources.Load<GameObject>("Prefabs/ChestUI"),
                Resources.Load<GameObject>("Prefabs/ShopUI")
            };
        }

        /// <summary>
        /// Draws selectable contents of UI Creator
        /// </summary>
        /// <param name="index"></param>
        private void DrawContent(int index)
        {
            EditorGUILayout.BeginHorizontal("box");

            if ((index == 2 || index == 3) && _selections[1] == false
               ) // Disable GameUI sub panels if GameUI not selected
            {
                EditorGUI.BeginDisabledGroup(_selections[1] == false);
                _selections[index] = false;
                _selections[index] = EditorGUILayout.ToggleLeft(_contentNames[index], _selections[index]);
                EditorGUI.EndDisabledGroup();
            }
            else if ((index == 6 || index == 7) && _selections[5] == false
                    ) // Disable LevelCompletedUI sub panels if LevelCompletedUI not selected
            {
                EditorGUI.BeginDisabledGroup(_selections[5] == false);
                _selections[index] = false;
                _selections[index] = EditorGUILayout.ToggleLeft(_contentNames[index], _selections[index]);
                EditorGUI.EndDisabledGroup();
            }
            else
            {
                _selections[index] = EditorGUILayout.ToggleLeft(_contentNames[index], _selections[index]);
            }

            if (GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
            {
                selectedContentIndex = index;
            }

            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// Instantiates all selected UI prefabs from UI Creator window
        /// </summary>
        private void GenerateUI()
        {
            InitContents();
            // Check EventSystem in scene if there is not, instantiate an EventSystem
            if (FindObjectOfType<EventSystem>() == null)
            {
                GameObject eventSystem = new GameObject("Event System");
                eventSystem.AddComponent<EventSystem>();
                eventSystem.AddComponent<StandaloneInputModule>();
            }

            UIManager go = FindObjectOfType<UIManager>();

            if (!go)
            {
                go = Instantiate(UIManagerObject).GetComponent<UIManager>();
            }

            go.name = "UIManager";
            go.transform.SetParent(GameObject.Find("---> MANAGER").transform);

            // Instantiate UI prefabs
            for (int i = 0; i < _contents.Count; i++)
            {
                if (_selections[i])
                {
                    if ((i == 2 || i == 3) && _selections[1] == true
                    ) // Instantiate GameUI sub panels if GameUI is selected
                    {
                        _prefabs[i] = Instantiate(_prefabs[i]);
                        _prefabs[i].transform.SetParent(_prefabs[1].transform);
                    }
                    else if ((i == 6 || i == 7) && _selections[5] == true
                    ) // Instantiate LevelCompletedUI sub panels if LevelCompletedUI is selected
                    {
                        _prefabs[i] = Instantiate(_prefabs[i]);
                        _prefabs[i].transform.SetParent(_prefabs[5].transform);
                    }
                    else
                    {
                        _prefabs[i] = Instantiate(_prefabs[i]);
                        _prefabs[i].transform.SetParent(go.transform);
                    }

                    _prefabs[i].name = _contents[i].Name;
                }
            }
        }


        void GenerateVariables(FileStream fs)
        {
            Byte[] boolText;

            for (int i = 0; i < _contents.Count; i++)
            {
                if (_selections[i])
                {
                    // Variable
                    boolText = new UTF8Encoding(true)
                        .GetBytes("[SerializeField] private " + _contentComponents[i] + " _" + _contentComponents[i] +
                                  ";\n");
                    fs.Write(boolText, 0, boolText.Length);
                }
            }

            boolText = new UTF8Encoding(true)
                .GetBytes("\n");
            fs.Write(boolText, 0, boolText.Length);

            for (int i = 0; i < _contents.Count; i++)
            {
                if (_selections[i])
                {
                    // Property
                    boolText = new UTF8Encoding(true)
                        .GetBytes("public " + _contentComponents[i] + " " + _contentComponents[i].ToUpper() + " => " +
                                  "_" + _contentComponents[i] + ";\n");
                    fs.Write(boolText, 0, boolText.Length);
                }
            }
        }

        /// <summary>
        /// Creates an instance of this editor window and show
        /// </summary>
        [MenuItem("OD Projects/Mobile/UI Creator")]
        public static void ShowCreatorWindow()
        {
            EditorWindow window = GetWindow(typeof(UICreatorWindow));
            window.titleContent = new GUIContent("UI Creator");
            window.minSize = new Vector2(720, 640);
            window.maxSize = new Vector2(720, 640);
            window.Focus();
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();

            ////////////////////////////////////// // Left Panel

            #region Left Panel for selection

            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("———————————", titleStyle);
            EditorGUILayout.LabelField("UI CREATOR", titleStyle);
            EditorGUILayout.LabelField("———————————", titleStyle);
            EditorGUILayout.Space(50);

            #endregion

            #region Draw list of selectable UI Contents to left panel

            for (int i = 0; i < 11; i++)
            {
                DrawContent(i);
                EditorGUILayout.Space(3);
            }

            #endregion

            #region Draw Generate UI Button

            EditorGUILayout.Space(50);
            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button)
            {
                fontSize = 20,
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold,
            };
            buttonStyle.normal.textColor = Color.green;
            if (GUILayout.Button("Generate UI", buttonStyle, GUILayout.Height(50)))
            {
                Debug.Log("UI Creator: UI Created!");
                GenerateUI();
            }

            // Draw credits label
            EditorGUILayout.Separator();
            GUILayout.Label("© 2023 Oguzhan Delibas", EditorStyles.centeredGreyMiniLabel);
            GUILayout.Label("OD Projects", EditorStyles.centeredGreyMiniLabel);
            EditorGUILayout.EndVertical();

            #endregion

            ////////////////////////////////////// // Right Panel

            #region Draw right panel for preview

            EditorGUILayout.BeginVertical();
            //EditorGUILayout.LabelField("Preview", EditorStyles.whiteLargeLabel);
            GUILayout.Label(_contents[selectedContentIndex].Preview,
                titleStyle,
                GUILayout.Width(360),
                GUILayout.Height(720));
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();

            #endregion

            //////////////////////////////////////
        }
    }
}

public struct UiDrawer
{
    public string Name { get; set; }
    public Texture2D Preview { get; set; }
}