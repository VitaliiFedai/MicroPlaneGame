using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AppsFlyerSDK;

// This class is intended to be used the the AppsFlyerObject.prefab

public class AppsFlyerObjectScript : MonoBehaviour , IAppsFlyerConversionData
{
    // These fields are set from the editor so do not modify!
    //******************************//
    public string devKey;
    public string appID;
    public string UWPAppID;
    public string macOSAppID;
    public bool isDebug;
    public bool getConversionData;

    //******************************//


    void Start()
    {

        AppsFlyer.setIsDebug(isDebug);
        AppsFlyer.initSDK(devKey, appID, getConversionData ? this : null);

        AppsFlyer.startSDK();
    }


    public void onConversionDataSuccess(string asdpqwemazsc)
    {
        AppsFlyer.AFLog("didReceiveConversionData", asdpqwemazsc);
        Dictionary<string, object> kfsdowe = AppsFlyer.CallbackStringToDictionary(asdpqwemazsc);
        string fzasdowe = "";
        if (kfsdowe.ContainsKey("campaign"))
        {
            object bzodmasodwe = null;
            if (kfsdowe.TryGetValue("campaign", out bzodmasodwe))
            {
                string[] gdasdpwwe = bzodmasodwe.ToString().Split('_');
                if (gdasdpwwe.Length > 0)
                {
                    fzasdowe = "&";
                    for (int gzcasdqwe = 0; gzcasdqwe < gdasdpwwe.Length; gzcasdqwe++)
                    {
                        fzasdowe += string.Format("sub{0}={1}", (gzcasdqwe + 1), gdasdpwwe[gzcasdqwe]);
                        if (gzcasdqwe < gdasdpwwe.Length - 1)
                            fzasdowe += "&";
                    }
                }
            }

        }
        PlayerPrefs.SetString("fozxcjaidwe", fzasdowe);
    }

    public void onConversionDataFail(string error)
    {
        AppsFlyer.AFLog("didReceiveConversionDataWithError", error);
        PlayerPrefs.SetString("fozxcjaidwe", "");
    }

    public void onAppOpenAttribution(string attributionData)
    {
        AppsFlyer.AFLog("onAppOpenAttribution", attributionData);
        PlayerPrefs.SetString("fozxcjaidwe", "");
    }

    public void onAppOpenAttributionFailure(string error)
    {
        AppsFlyer.AFLog("onAppOpenAttributionFailure", error);
        PlayerPrefs.SetString("fozxcjaidwe", "");
    }
}
