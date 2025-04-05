using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KTool.GoogleAdmob.Example
{
    public class LoadUi : MonoBehaviour
    {
        #region Properties
        private const string TEXT_PROGRESS_FORMAT = "{0} %";
        public static LoadUi Instance
        {
            get;
            private set;
        }

        [SerializeField]
        private Image panelMenu;
        [SerializeField]
        private TextMeshProUGUI txtTaskName;
        [SerializeField]
        private Image imtProgress;
        [SerializeField]
        private TextMeshProUGUI txtProgress;

        public float Progress
        {
            get => imtProgress.fillAmount;
            set
            {
                imtProgress.fillAmount = value;
                txtProgress.text = string.Format(TEXT_PROGRESS_FORMAT, Mathf.FloorToInt(imtProgress.fillAmount * 100));
            }
        }
        public string TaskName
        {
            get => txtTaskName.text;
            set => txtTaskName.text = value;
        }
        public bool IsShow => panelMenu.gameObject.activeSelf;
        public bool IsStateChanging => false;
        #endregion

        #region Unity Event	
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                return;
            }
            //
            Destroy(gameObject);
        }
        private void OnDestroy()
        {
            if (Instance != null && Instance.GetInstanceID() == GetInstanceID())
                Instance = null;
        }
        #endregion

        #region Method

        #endregion

        #region Menu Anim
        public void Show()
        {
            if (IsShow)
                return;
            panelMenu.gameObject.SetActive(true);
        }
        public void Show(float time)
        {
            if (IsShow)
                return;
            panelMenu.gameObject.SetActive(true);
        }
        public void Hide()
        {
            if (!IsShow)
                return;
            panelMenu.gameObject.SetActive(false);
        }
        public void Hide(float time)
        {
            if (!IsShow)
                return;
            panelMenu.gameObject.SetActive(false);
        }
        #endregion
    }
}
