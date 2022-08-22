using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoNPCController : NPCController
{
    /************************************************************************/
    /************************************************************************/
    // Fields
    private GameObject _selectUI;
    
    private GameObject _videoCanvas;
    private RawImage _videoRawImage;
    private VideoPlayer _videoPlayer;
    private RenderTexture _renderTexture;
    
    // Paths
    // 비디오를 재생할지 여부를 선택하는 UI
    private string _selectUIPath = "UI/VideoNPCUI/SelectUI";
    private string _playButtonPath = "UI/VideoNPCUI/PlayButton";
    private string _CancleButtonPath = "UI/VideoNPCUI/CancleButton";

    // 비디오 재생에 관한 UI
    private string _cavasPath = "UI/VideoUI/VideoCanvas";
    private string _rawImagePath = "UI/VideoUI/VideoRawImage";
    private string _videoPlayerPath = "UI/VideoUI/VideoPlayer";
    string _renderTexturePath = "Art/Video/VideoRenderTexture";
    private string _videoClipPath = "Art/Video/Video";
    private string _stopButtonPath = "UI/VideoUI/StopButton";
    
    public override void OnClicked()
    {
         GenerateSelectUI();
    }

    void GenerateSelectUI() // NPC를 클릭하면 비디오를 재생할지 취소할지 선택하는 UI 발생
    {
        _selectUI = Managers.Resource.Instantiate(_selectUIPath);
        Button playButton = Managers.Resource.Instantiate(_playButtonPath, _selectUI.transform).GetComponent<Button>();
        Button cancleButton = Managers.Resource.Instantiate(_CancleButtonPath, _selectUI.transform).GetComponent<Button>();
        
        playButton.onClick.AddListener(PlayVideo);
        cancleButton.onClick.AddListener(Cancle);
    }

    void PlayVideo() // 비디오 재생을 위한 메소드
    {
        if (_selectUI != null)
            Managers.Resource.Destroy(_selectUI);
        
        // 비디오를 재생할 Canvas 생성 후 rawimage와 비디오플레이어 불러오기, 렌더텍스쳐 불러오기
        _videoCanvas = Managers.Resource.Instantiate(_cavasPath);
        _videoRawImage = Managers.Resource.Instantiate(_rawImagePath, _videoCanvas.transform).GetComponent<RawImage>();
        _videoPlayer = Managers.Resource.Instantiate(_videoPlayerPath, _videoCanvas.transform).GetComponent<VideoPlayer>();
        _renderTexture = Managers.Resource.Load<RenderTexture>(_renderTexturePath);

        // 비디오 UI를 없앨 버튼 추가
        Button stopButton = Managers.Resource.Instantiate(_stopButtonPath, _videoCanvas.transform)
            .GetComponent<Button>();
        stopButton.onClick.AddListener(StopVideo);
        
        // 비디오 재생
        _videoRawImage.texture = _renderTexture;
        VideoClip videoClip = Managers.Resource.Load<VideoClip>(_videoClipPath);
        _videoPlayer.clip = videoClip;
        _videoPlayer.targetTexture = _renderTexture;
        _videoPlayer.Play();
    }
    
    /************************************************************************/
    /************************************************************************/
    // 버튼 이벤트
    void Cancle()
    {
        if (_selectUI != null)
            Managers.Resource.Destroy(_selectUI);
    }

    void StopVideo()
    {
        if (_videoCanvas != null)
        {
            _videoPlayer.Stop();
            Managers.Resource.Destroy(_videoCanvas);
        }
    }
}
