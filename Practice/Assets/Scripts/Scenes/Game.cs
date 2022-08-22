using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Game : BaseScene
{
    /************************************************************************/
    // Fields
    private GameObject _myCharacter;
    private GameObject _videoNPC;
    private Camera _camera;
    
    // Paths
    private string _animConPath = "Art/Characters/Animations/AnimationController/PlayerAnimController";

    private string _videoNPCPath = "Character/VideoNPC"; 
    
    /************************************************************************/
    /************************************************************************/
    protected override void init()
    {
        base.init();
        
        // 카메라
        _camera = Camera.main;
        _camera.AddComponent<CameraController>(); // 쿼터뷰 설정을 위해 카메라컨트롤 스크립트 불러옴
        
        // 나의 캐릭터 불러오기
        GenerateMyCharacter();
        
        // NPC 불러오기
        _videoNPC = GenerateNPCs(_videoNPCPath, new Vector3(0, 0, 60));
        _videoNPC.AddComponent<VideoNPCController>();
    }

    public override void Clear()
    {
        
    }
    
    /************************************************************************/
    /************************************************************************/
    // Methods

    void GenerateMyCharacter() // 매니져 클래스에서 내 캐릭터를 받아옴
    {
        _myCharacter = Managers.MyCharacter;
        _myCharacter.transform.position = new Vector3(16, 0, 55);
        
        Rigidbody rigidbody = _myCharacter.AddComponent<Rigidbody>();
        rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        
        RuntimeAnimatorController playerAnimController = Managers.Resource.Load<RuntimeAnimatorController>(_animConPath);
        _myCharacter.GetComponent<Animator>().runtimeAnimatorController = playerAnimController;

        _myCharacter.AddComponent<PlayerController>();
    }

    GameObject GenerateNPCs(string path, Vector3 pos) // NPC 불러오기
    {
        GameObject NPC = Managers.Resource.Instantiate(path);
        NPC.transform.position = pos;
        NPC.transform.rotation = Quaternion.Euler(0, 180, 0);
        return NPC;
    }
}
