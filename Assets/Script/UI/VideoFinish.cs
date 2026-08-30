using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoToPanel : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject panelVideo;
    public GameObject panelAkhir;

    public static bool sudahPernahMain = false;
    private bool isFinished = false;
    private float startTime;

    void Start()
    {
        startTime = Time.time;
        panelVideo.SetActive(true);
        panelAkhir.SetActive(false);

        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += VideoSelesai;
            videoPlayer.errorReceived += VideoError;
            videoPlayer.prepareCompleted += VideoPrepared;

            // Load dari StreamingAssets via URL
            string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, "komik_film.mp4");
            videoPlayer.source = VideoSource.Url;
            videoPlayer.url = videoPath;

            videoPlayer.Prepare();
            StartCoroutine(PrepareTimeoutCheck());
        }
        else
        {
            TampilkanPanelAkhir();
        }
    }

    void Update()
    {
        // Beri jeda 1 detik sebelum tombol skip bisa ditekan agar tidak kena sisa klik mouse dari scene sebelumnya
        if (Time.time - startTime > 1.0f)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.U) || Input.GetKeyDown(KeyCode.Escape))
            {
                SkipVideo();
            }
        }
    }

    void VideoPrepared(VideoPlayer vp)
    {
        vp.Play();
    }

    void VideoError(VideoPlayer vp, string message)
    {
        Debug.LogWarning("[VideoToPanel] Gagal memutar video di browser: " + message);
        TampilkanPanelAkhir();
    }

    IEnumerator PrepareTimeoutCheck()
    {
        // Tunggu 5 detik untuk browser memuat video
        yield return new WaitForSeconds(5f);
        if (videoPlayer != null && !videoPlayer.isPlaying && !isFinished)
        {
            Debug.LogWarning("[VideoToPanel] Timeout prepare video, beralih ke panel komik.");
            TampilkanPanelAkhir();
        }
    }

    void VideoSelesai(VideoPlayer vp)
    {
        TampilkanPanelAkhir();
    }

    public void SkipVideo()
    {
        if (videoPlayer != null)
        {
            videoPlayer.Stop();
        }
        TampilkanPanelAkhir();
    }

    void TampilkanPanelAkhir()
    {
        if (isFinished) return;
        isFinished = true;

        panelVideo.SetActive(false);
        panelAkhir.SetActive(true);
    }

    void OnDestroy()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= VideoSelesai;
            videoPlayer.errorReceived -= VideoError;
            videoPlayer.prepareCompleted -= VideoPrepared;
        }
    }
}
