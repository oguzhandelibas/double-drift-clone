using UnityEngine;

namespace DoubleDrift.UIModule
{
    public abstract class View : MonoBehaviour
    {
        /// <summary>
        /// Initializes the View
        /// </summary>
        public virtual void Initialize()
        {
        }

        #region UI BUTTON

        public void _ClosePanel()
        {
            //UIManager.Instance.GoBack();
        }

        #endregion
        
        
        /// <summary>
        /// Makes the View visible
        /// </summary>
        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        /// <summary>
        /// Hides the view
        /// </summary>
        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
