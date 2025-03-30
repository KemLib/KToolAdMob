using UnityEngine;

namespace KTool.GoogleAdmob
{
    [System.Serializable]
    public class AdMobSettingAdId
    {
        #region Properties
        [SerializeField]
        private string androidId,
            iosId;

        public string AdID
        {
            get
            {
#if UNITY_EDITOR
                if (!string.IsNullOrEmpty(androidId))
                    return androidId;
                if (!string.IsNullOrEmpty(iosId))
                    return iosId;
                return string.Empty;
#elif UNITY_ANDROID
                return androidId;
#elif UNITY_IOS
                return iosId;
#endif
            }
        }
        #endregion

        #region Construction
        public AdMobSettingAdId()
        {

        }
        #endregion

        #region Method

        #endregion
    }
}
