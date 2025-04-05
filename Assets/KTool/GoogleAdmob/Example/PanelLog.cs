using TMPro;
using UnityEngine;

namespace KTool.GoogleAdmob.Example
{
    public class PanelLog : MonoBehaviour
    {
        #region Properties
        private const string LOG_FORMAT = "{0}\n";

        [SerializeField]
        private TextMeshProUGUI txtLog;
        #endregion

        #region Unity Events

        #endregion

        #region Methods
        public void Init()
        {

        }
        public void AddLog(string log)
        {
            txtLog.text += string.Format(LOG_FORMAT, log);
        }
        #endregion

        #region Ui Event
        public void OnClick_Clear()
        {
            txtLog.text = string.Empty;
        }
        #endregion
    }
}
