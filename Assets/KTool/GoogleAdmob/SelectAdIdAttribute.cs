using System;
using UnityEngine;

namespace KTool.GoogleAdmob
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class SelectAdIdAttribute : PropertyAttribute
    {
        #region Properties
        private AdMobAdType type;

        public AdMobAdType Type => type;
        #endregion

        #region Construction
        public SelectAdIdAttribute(AdMobAdType type) : base()
        {
            this.type = type;
        }
        #endregion

        #region Method

        #endregion
    }
}
