using Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class PlayVideo : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private RawImage rawImage;

    public void Play(string videoFile, bool isLoop)
    {
        try
        {
            videoPlayer.source = VideoSource.Url;
            videoPlayer.url = Application.streamingAssetsPath + "/Videos/" + videoFile + ".mp4";

            videoPlayer.isLooping = isLoop;
            videoPlayer.SetTargetAudioSource(0, AudioManager.S.SoundSource);

            videoPlayer.prepareCompleted += VideoPlayer_prepareCompleted;
            videoPlayer.errorReceived += VideoPlayer_errorReceived;

            videoPlayer.Prepare();
        }
        catch (Exception e)
        {
            Debug.Log("Error Prepare Video: " + e.Message);
        }
    }

    public void Loop(bool isLoop)
    {
        videoPlayer.isLooping = isLoop;
    }

    public void Resume()
    {
        videoPlayer.Play();
    }

    public void Pause()
    {
        videoPlayer.Pause();
    }

    public void Stop()
    {
        videoPlayer.time = 0;
        videoPlayer.Stop();
    }

    private void VideoPlayer_errorReceived(VideoPlayer source, string message)
    {
        videoPlayer.errorReceived -= VideoPlayer_errorReceived;

        EndVideo();
    }

    private void VideoPlayer_prepareCompleted(VideoPlayer source)
    {
        videoPlayer.prepareCompleted -= VideoPlayer_prepareCompleted;
        PrepareCompletedVideo();
    }

    private void PrepareCompletedVideo()
    {
        try
        {
            videoPlayer.time = 0;

            videoPlayer.Play();
            videoPlayer.loopPointReached += EndReached;
        }
        catch (Exception e)
        {
            Debug.LogError("Error Play Complete Video: " + e.Message);
            EndVideo();
            videoPlayer.Stop();
        }
    }

    private void EndReached(VideoPlayer source)
    {
        videoPlayer.loopPointReached -= EndReached;
        if(!videoPlayer.isLooping) EndVideo();
    }

    private void EndVideo()
    {
        videoPlayer.time = 0;
    }
}
