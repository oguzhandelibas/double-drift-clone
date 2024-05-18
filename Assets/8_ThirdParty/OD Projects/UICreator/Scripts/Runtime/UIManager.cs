using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace DoubleDrift.UIModule
{
    public class UIManager : AbstractSingleton<UIManager>
    {
        [SerializeField]
        Canvas m_Canvas;
        [SerializeField]
        RectTransform m_Root;
        [SerializeField]
        RectTransform m_ViewLayer;
        List<View> m_Views = new List<View>();
        View m_CurrentView;
        readonly Stack<View> m_History = new();

        [SerializeField] private TextMeshProUGUI speedText;
        public void SetCarSpeed(int carSpeed)
        {
            speedText.text = (carSpeed * 3).ToString() + " mph";
        }
        
        void Start()
        {
            m_Views = m_Root.GetComponentsInChildren<View>(true).ToList();
            Init();
            //m_ViewLayer.ResizeToSafeArea(m_Canvas);
        }

        void Init()
        {
            foreach (var view in m_Views)
                view.Hide();
            m_History.Clear();
            Show<HomeUI>();
        }

        /// <summary>
        /// Finds the first registered UI View of the specified type
        /// </summary>
        /// <typeparam name="T">The View class to search for</typeparam>
        /// <returns>The instance of the View of the specified type. null if not found </returns>
        public T GetView<T>() where T : View
        {
            foreach (var view in m_Views)
            {
                if (view is T tView)
                {
                    return tView;
                }
            }
            return null;
        }

        /// <summary>
        /// Finds the View of the specified type and makes it visible
        /// </summary>
        /// MenuScreen,
        /// SettingsScreen,
        /// UnitSelectionScreen,
        /// GameSelectionScreen,
        /// StatusScreen,
        /// ProfileScreen
        /// <param name="keepInHistory">Pushes the current View to the history stack in case we want to go back to</param>
        /// <typeparam name="T">The View class to search for</typeparam>
        public void Show<T>(bool keepInHistory = true) where T : View
        {
            foreach (var view in m_Views)
            {
                if (view is T)
                {
                    Show(view, keepInHistory);
                    break;
                }
            }
        }

        /// <summary>
        /// Makes a View visible and hides others
        /// </summary>
        /// <param name="view">The view</param>
        /// <param name="keepInHistory">Pushes the current View to the history stack in case we want to go back to</param>
        public void Show(View view, bool keepInHistory = true)
        {
            if (m_CurrentView != null)
            {
                if (keepInHistory)
                {
                    m_History.Push(m_CurrentView);
                }

                m_CurrentView.Hide();
            }
            
            view.Show();
            m_CurrentView = view;
        }

        /// <summary>
        /// Goes to the page visible previously
        /// </summary>
        public void GoBack()
        {
            if (m_History.Count != 0)
            {
                Show(m_History.Pop(), false);
            }
        }
    }
}
