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

    void Start()
    {
        // Awal: video tampil, panel akhir disembunyikan
        panelVideo.SetActive(true);
        panelAkhir.SetActive(false);

        // Hubungkan event video selesai
        videoPlayer.loopPointReached += VideoSelesai;

        // Mulai video
        videoPlayer.Play();
    }

    void Update()
    {
        // U hanya bisa digunakan kalau sudah pernah masuk gameplay
        if (sudahPernahMain && Input.GetKeyDown(KeyCode.U))
        {
            SkipVideo();
        }
    }

    void VideoSelesai(VideoPlayer vp)
    {
        TampilkanPanelAkhir();
    }

    void SkipVideo()
    {
        videoPlayer.Stop();
        TampilkanPanelAkhir();
    }

    void TampilkanPanelAkhir()
    {
        panelVideo.SetActive(false);
        panelAkhir.SetActive(true);
    }

    void OnDestroy()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= VideoSelesai;
        }
    }
}