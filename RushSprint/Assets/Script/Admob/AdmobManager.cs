using UnityEngine;
using GoogleMobileAds.Api;

public class AdMobManager : MonoBehaviour
{
    private BannerView bannerAd;
    private InterstitialAd interstitialAd;
    private RewardedAd rewardedAd;

    // Replace with your AdMob Ad Unit IDs
    private string bannerAdId = "ca-app-pub-7700860700735087~4934221345";
    private string interstitialAdId = "ca-app-pub-7700860700735087/1518824169";
    private string rewardedAdId = "ca-app-pub-7700860700735087/3704931978";

    void Start()
    {
        // Initialize AdMob SDK
        MobileAds.Initialize(initStatus => { Debug.Log("AdMob Initialized!"); });

        // Load Ads
        if (GameManager.instance.showAd)
        {
            LoadBannerAd();
            LoadInterstitialAd();
            LoadRewardedAd();
        }
    }

    // Load Banner Ad
    private void LoadBannerAd()
    {
        if (bannerAd != null)
        {
            bannerAd.Destroy();
        }

        bannerAd = new BannerView(bannerAdId, AdSize.Banner, AdPosition.Bottom);
        AdRequest request = new AdRequest();
        bannerAd.LoadAd(request);
    }

    // Load Interstitial Ad
    private void LoadInterstitialAd()
    {
        InterstitialAd.Load(interstitialAdId, new AdRequest(), (InterstitialAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError("Interstitial Ad Failed to Load: " + error);
                return;
            }

            interstitialAd = ad;
            Debug.Log("Interstitial Ad Loaded");

            interstitialAd.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("Interstitial Ad Closed, Reloading...");
                LoadInterstitialAd();
            };
        });
    }

    // Show Interstitial Ad
    public void ShowInterstitialAd()
    {
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            interstitialAd.Show();
        }
        else
        {
            Debug.Log("Interstitial Ad is not ready yet.");
        }
    }

    // Load Rewarded Ad
    private void LoadRewardedAd()
    {
        RewardedAd.Load(rewardedAdId, new AdRequest(), (RewardedAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError("Rewarded Ad Failed to Load: " + error);
                return;
            }

            rewardedAd = ad;
            Debug.Log("Rewarded Ad Loaded");

            rewardedAd.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("Rewarded Ad Closed, Reloading...");
                LoadRewardedAd();
            };
        });
    }

    // Show Rewarded Ad
    public void ShowRewardedAd()
    {
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                Debug.Log("Player Earned Reward: " + reward.Amount);
                // Add logic to give extra lives, coins, etc.
            });
        }
        else
        {
            Debug.Log("Rewarded Ad is not ready yet.");
        }
    }
}
