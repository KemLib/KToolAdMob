using KTool.Advertisement;
using UnityEngine;

namespace KTool.GoogleAdmob
{
    public static class Utility
    {
        #region Properties
        public static float DensityScreen
        {
            get
            {
#if UNITY_EDITOR
                return Screen.height / 800f;
#else
                return GoogleMobileAds.Api.MobileAds.Utils.GetDeviceScale();
#endif
            }
        }
        #endregion

        #region Method
        public static Vector2 Unity_GetScreen()
        {
            return new Vector2(Screen.width, Screen.height);
        }

        public static Vector2 Unity_Get(AdPosition adPosition, Vector2 bannerSize)
        {
            Vector2 positionAdMob = AdMob_Get(adPosition, Convert_UnityToAdMob(bannerSize));
            return Convert_AdMobToUnity(positionAdMob);
        }
        public static Vector2 Unity_Get(AdSize adSize)
        {
            Vector2 sizeAdMob = AdMob_Get(adSize);
            return Convert_AdMobToUnity(sizeAdMob);
        }
        public static Vector2 Convert_UnityToAdMob(Vector2 vecter)
        {
            float scale = DensityScreen;
            return new Vector2(vecter.x / scale, vecter.y / scale);
        }
        public static Vector2 Convert_AdMobToUnity(Vector2 vecter)
        {
            float scale = DensityScreen;
            return new Vector2(vecter.x * scale, vecter.y * scale);
        }
        public static GoogleMobileAds.Api.AdSize ConvertSize(AdSize adSize)
        {
            switch(adSize)
            {
                case AdSize.Standard:
                    return GoogleMobileAds.Api.AdSize.Banner;
                case AdSize.Large:
                    return GoogleMobileAds.Api.AdSize.MediumRectangle;
                case AdSize.Medium:
                    return GoogleMobileAds.Api.AdSize.IABBanner;
                case AdSize.FullSize:
                    return GoogleMobileAds.Api.AdSize.Leaderboard;
                default:
                    return GoogleMobileAds.Api.AdSize.Banner;

            }
        }
        public static GoogleMobileAds.Api.AdPosition ConvertPosition(AdPosition adPosition)
        {
            switch(adPosition)
            {
                case AdPosition.TopLeft:
                    return GoogleMobileAds.Api.AdPosition.TopLeft;
                case AdPosition.TopCenter:
                    return GoogleMobileAds.Api.AdPosition.Top;
                case AdPosition.TopRight:
                    return GoogleMobileAds.Api.AdPosition.TopRight;
                case AdPosition.MidLeft:
                case AdPosition.MidCenter:
                case AdPosition.MidRight:
                    return GoogleMobileAds.Api.AdPosition.Center;
                case AdPosition.BotLeft:
                    return GoogleMobileAds.Api.AdPosition.BottomLeft;
                case AdPosition.BotCenter:
                    return GoogleMobileAds.Api.AdPosition.Bottom;
                case AdPosition.BotRight:
                    return GoogleMobileAds.Api.AdPosition.BottomRight;
                default: 
                    return GoogleMobileAds.Api.AdPosition.Bottom;

            }
        }
        public static GoogleMobileAds.Api.BannerView Create_AdBanner(string id, AdSize adSize, AdPosition adPosition, Vector2 position)
        {
            if (adPosition == AdPosition.Custom)
            {
                Vector2 point = Convert_UnityToAdMob(position);
                return new GoogleMobileAds.Api.BannerView(id, ConvertSize(adSize), (int)point.x, (int)point.y);
            }
            return new GoogleMobileAds.Api.BannerView(id, ConvertSize(adSize), ConvertPosition(adPosition));
        }
        #endregion

        #region Admob
        public static Vector2 AdMob_GetScreen()
        {
            Vector2 unityScreen = Unity_GetScreen();
            return Convert_UnityToAdMob(unityScreen);
        }
        public static Vector2 AdMob_Get(AdSize adSize)
        {
            switch (adSize)
            {
                case AdSize.Standard:
                    return new Vector2(320, 50);
                case AdSize.Large:
                    return new Vector2(320, 100);
                case AdSize.Medium:
                    return new Vector2(300, 250);
                case AdSize.FullSize:
                    return new Vector2(720, 90);
                case AdSize.Leaderboard:
                    return new Vector2(320, 50);
                case AdSize.Smart:
                    Vector2 screenSize = AdMob_GetScreen();
                    float width = screenSize.x,
                        height;
                    if (screenSize.y <= 400)
                        height = 32;
                    else if (screenSize.y <= 720)
                        height = 50;
                    else
                        height = 90;
                    return new Vector2(width, height);
                default:
                    return new Vector2(320, 50);
            }
        }
        public static Vector2 AdMob_Get(AdPosition adPosition, Vector2 bannerSize)
        {
            Vector2 screenSize = AdMob_GetScreen();
            switch (adPosition)
            {
                case AdPosition.TopLeft:
                    return new Vector2(0, 0);
                case AdPosition.TopCenter:
                    return new Vector2((screenSize.x - bannerSize.x) / 2, 0);
                case AdPosition.TopRight:
                    return new Vector2(screenSize.x - bannerSize.x, 0);
                //
                case AdPosition.MidLeft:
                    return new Vector2(0, (screenSize.y - bannerSize.y) / 2);
                case AdPosition.MidCenter:
                    return new Vector2((screenSize.x - bannerSize.x) / 2, (screenSize.y - bannerSize.y) / 2);
                case AdPosition.MidRight:
                    return new Vector2((screenSize.x - bannerSize.x) / 2, (screenSize.y - bannerSize.y) / 2);
                //
                case AdPosition.BotLeft:
                    return new Vector2(0, screenSize.y - bannerSize.y);
                case AdPosition.BotCenter:
                    return new Vector2((screenSize.x - bannerSize.x) / 2, screenSize.y - bannerSize.y);
                case AdPosition.BotRight:
                    return new Vector2(screenSize.x - bannerSize.x, screenSize.y - bannerSize.y);
                default:
                    return new Vector2(0, 0);
            }
        }
        #endregion
    }
}
