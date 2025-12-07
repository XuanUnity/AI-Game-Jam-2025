using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    public RawImage rawImage1;   // dành cho video 1
    public RawImage rawImage2;   // dành cho video 2

    public VideoClip video1;
    public VideoClip video2;

    public Button skipButton;

    public Image Name;

    private bool firstTimePlayed;

    void Start()
    {
        skipButton.gameObject.SetActive(false);
        rawImage1.gameObject.SetActive(false);
        rawImage2.gameObject.SetActive(false);
        Name.gameObject.SetActive(false);

        // Nếu chưa từng chạy → FirstTimePlayed = false
        firstTimePlayed = PlayerPrefs.GetInt("FirstTimePlayed", 1) == 1;
        //firstTimePlayed = false;
        skipButton.onClick.AddListener(() =>
        {
            AudioStartManager.Instance.PlayButtonClick();
            SkipVideo();
        });
    }
    public void PlayContinue()
    {
        firstTimePlayed = false;
/*        PlayerPrefs.SetInt("FirstTimePlayed", 0);
        PlayerPrefs.Save();*/
        OnPlayClicked();
    }

    public void PlayNewGame()
    {
        firstTimePlayed = true;
        PlayerPrefs.SetInt("FirstTimePlayed", 1);
        PlayerPrefs.SetInt("FirstTime", 1);
        PlayerPrefs.Save();
        OnPlayClicked();
    }
    // Gọi hàm này khi nhấn nút Play
    public void OnPlayClicked()
    {
        videoPlayer.Stop();
        videoPlayer.loopPointReached -= OnVideoFinished;
        videoPlayer.loopPointReached += OnVideoFinished;

        if (firstTimePlayed)
        {
            // ========== VIDEO 1 ==========
            rawImage1.gameObject.SetActive(true);
            rawImage2.gameObject.SetActive(false);

            videoPlayer.clip = video1;
            videoPlayer.targetTexture = rawImage1.texture as RenderTexture;
            videoPlayer.Play();

            skipButton.gameObject.SetActive(true);

            firstTimePlayed = true;
        }
        else
        {
            // ========== VIDEO 2 ==========
            rawImage1.gameObject.SetActive(false);
            rawImage2.gameObject.SetActive(true);
            Name.gameObject.SetActive(true);

            videoPlayer.clip = video2;
            videoPlayer.targetTexture = rawImage2.texture as RenderTexture;
            videoPlayer.Play();
        }
    }

    void SkipVideo()
    {
        videoPlayer.Stop();

        DOVirtual.DelayedCall(0.01f, () =>
        {
            SceneManager.LoadScene("MainScene");
        });
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        DOVirtual.DelayedCall(0.01f, () =>
        {
            SceneManager.LoadScene("MainScene");
        });
    }
}
