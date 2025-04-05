using KTool.Attribute.Editor;
using UnityEditor;
using UnityEngine;

namespace KTool.GoogleAdmob.Editor
{
    [CustomPropertyDrawer(typeof(SelectAdIdAttribute))]
    public class SelectAdIdDrawer : PropertyDrawer
    {
        #region Properties
        #endregion
        public SelectAdIdDrawer() : base()
        {

        }
        #region Properties

        #endregion

        #region Unity Event
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (fieldInfo.FieldType == typeof(int) || fieldInfo.FieldType == typeof(int[]))
            {
                string[] adIds = GetAdIds();
                int index = property.intValue;
                if (index < 0)
                {
                    index = 0;
                    property.intValue = index;
                }
                else if (index >= adIds.Length)
                {
                    index = adIds.Length - 1;
                    property.intValue = index;
                }
                EditorGui_Draw.DrawPopup_Int(position, label, adIds, property);
                return;
            }
            EditorGUI.LabelField(position, label, new GUIContent("type of property not is Int"));
        }
        #endregion

        #region Method
        private string[] GetAdIds()
        {
            AdMobSetting adMobSetting = AdMobSetting.GetInstance();
            if (adMobSetting == null)
                return new string[0];
            //
            SelectAdIdAttribute adIdAttribute = attribute as SelectAdIdAttribute;
            int count = adMobSetting.Ad_Count(adIdAttribute.Type);
            string[] adIds = new string[count];
            for (int i = 0; i < count; i++)
                adIds[i] = adMobSetting.Ad_Get(adIdAttribute.Type, i).AdID;
            return adIds;
        }
        #endregion
    }
}
