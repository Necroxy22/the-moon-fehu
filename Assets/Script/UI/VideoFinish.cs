using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoFinish : MonoBehaviour
{
    public GameObject panelAkhir;

    private VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += VideoSelesai;
    }

    void VideoSelesai(VideoPlayer vp)
    {
        panelAkhir.SetActive(true);
    }
}