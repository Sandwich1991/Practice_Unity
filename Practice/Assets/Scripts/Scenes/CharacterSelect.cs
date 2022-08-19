using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
public class CharacterSelect : BaseScene
{
    enum Gender
    {
        Male,
        Female
    }

    private Gender _gender;

    /************************************************************************/
    /************************************************************************/
    #region Fields
    // UI / Character
    private GameObject _genderSelectUI;
    private GameObject _customizingUI;
    private GameObject _myCharacter;
    
    // mesh renderer
    private SkinnedMeshRenderer _hair;
    private SkinnedMeshRenderer _shirts;
    private SkinnedMeshRenderer _pants;
    
    // skin color
    private int _skinIndex = 0;
    GameObject _skinColorText;
    private List<Material> _skinColors;
    
    // hair color
    private int _hairIndex = 0;
    private GameObject _hairColorText;
    private List<Material> _hairColors;
    #endregion
    
    #region UI Paths
    // GenderUI Path
    private string _genderUIPath = "UI/GenderUI/GenderUI";
    private string _nextOfGenderUIPath = "UI/GenderUI/Buttons/NextButton";
    private string _femaleOfGenderUIPath = "UI/GenderUI/Buttons/FemaleButton";
    private string _maleOfGenderUIPath = "UI/GenderUI/Buttons/MaleButton";
    private string _turnLeftOfGenderUIPath = "UI/GenderUI/Buttons/TurnLeftButton";
    private string _turnRightOfGenderUIPath = "UI/GenderUI/Buttons/TurnRightButton";
    private string _buttonsOfGenderUIPath = "UI/GenderUI/Buttons";
    private string _textOfGenderUIPath = "UI/GenderUI/Text";

    // CutominzingUI Path
    private string _custominzingUIPath = "UI/CustomizingUI";
    private string _buttonsOfCustomizingUIPath = "UI/CustomizingUI/Buttons";
    private string _nextOfOfCustomizingUIPath = "UI/CustomizingUI/Buttons/NextButton";
    private string _prevOfOfCustomizingUIPath = "UI/CustomizingUI/Buttons/PrevButton";
    private string _turnLeftOfCustomizingUIPath = "UI/CustomizingUI/Buttons/TurnLeftButton";
    private string _turnRightOfCustomizingUIPath = "UI/CustomizingUI/Buttons/TurnRightButton";
    private string _textsOfCutomizingUIPath = "UI/CustomizingUI/Texts";
    private string _pantsTextOfCutomizingUIPath = "UI/CustomizingUI/PantsText";
    private string _shirtsTextOfCutomizingUIPath = "UI/CustomizingUI/ShirtsText";
    private string _skinTextOfCutomizingUIPath = "UI/CustomizingUI/SkinText";
    private string _hairTextOfCustomizingUIPath = "UI/CustomizingUI/HairText";
    private string _nextSkinButtonPath = "UI/CustomizingUI/Buttons/NextSkin";
    private string _prevSkinButtonPath = "UI/CustomizingUI/Buttons/PrevSkin";
    private string _skinColorTextPath = "UI/CustomizingUI/SkinColorText";
    private string _nextHairButtonPath = "UI/CustomizingUI/Buttons/NextHairColor";
    private string _prevHairButtonPath = "UI/CustomizingUI/Buttons/PrevHairColor";
    private string _hairColorTextPath = "UI/CustomizingUI/HairColorText";
    #endregion

    #region Character Path
    // prefab Path
    private string _femalePrefabPath = "Character/Female";
    private string _malePrefabPath = "Character/male";
    #endregion
    /************************************************************************/
    /************************************************************************/
    // Scene
    protected override void init()
    {
        base.init();

        GenerateGenderUI(); // 성별 선택 UI 생성
        _myCharacter = GenerateCharacter(_malePrefabPath); // 초기 캐릭터 생성(기본 남자)
        _gender = Gender.Male;
        GetMeshRenderers(); // 캐릭터의 메쉬 렌더러 불러오기
        _skinColors = Utills.GetFileListInDir<Material>("Art/Characters/Materials/Skin/"); // 스킨 메테리얼 리스트로 불러오기
        _hairColors = Utills.GetFileListInDir<Material>("Art/Characters/Materials/Hair/"); // 머리색 메테리얼 리스트로 불러오기
    }
    public override void Clear()
    {
        
    }
    /************************************************************************/
    /************************************************************************/
    // Methods
    
    // 성별 선택 UI 불러오기
    void GenerateGenderUI()
    {
        _genderSelectUI = Managers.Resource.Instantiate(_genderUIPath);
        GameObject text = Managers.Resource.Instantiate(_textOfGenderUIPath, _genderSelectUI.transform);
        GameObject buttons = Managers.Resource.Instantiate(_buttonsOfGenderUIPath, _genderSelectUI.transform);

        Button femaleButton = Managers.Resource.Instantiate(_femaleOfGenderUIPath, buttons.transform).GetComponent<Button>();
        Button maleButton = Managers.Resource.Instantiate(_maleOfGenderUIPath, buttons.transform).GetComponent<Button>();
        Button turnLeftButton = Managers.Resource.Instantiate(_turnLeftOfGenderUIPath, buttons.transform).GetComponent<Button>();
        Button turnRightButton = Managers.Resource.Instantiate(_turnRightOfGenderUIPath, buttons.transform).GetComponent<Button>();
        Button nextButton = Managers.Resource.Instantiate(_nextOfGenderUIPath, buttons.transform).GetComponent<Button>();
        
        femaleButton.onClick.AddListener(ChangeToFemale);
        maleButton.onClick.AddListener(ChangeToMale);
        turnLeftButton.onClick.AddListener(TurnLeft);
        turnRightButton.onClick.AddListener(TurnRight);
        nextButton.onClick.AddListener(NextToCustomizingUI);
    } 
    
    // 캐릭터 파츠 선택 UI 불러오기
    void GenerateCustomizingUI()
    {
        _customizingUI = Managers.Resource.Instantiate(_custominzingUIPath);
        GameObject texts = Managers.Resource.Instantiate(_textsOfCutomizingUIPath, _customizingUI.transform);

        GameObject skinText = Managers.Resource.Instantiate(_skinTextOfCutomizingUIPath, texts.transform);
        GameObject shirtText = Managers.Resource.Instantiate(_shirtsTextOfCutomizingUIPath, texts.transform);
        GameObject pantsText = Managers.Resource.Instantiate(_pantsTextOfCutomizingUIPath, texts.transform);
        GameObject hairText = Managers.Resource.Instantiate(_hairTextOfCustomizingUIPath, texts.transform);
        _skinColorText = Managers.Resource.Instantiate(_skinColorTextPath, texts.transform);
        _hairColorText = Managers.Resource.Instantiate(_hairColorTextPath, texts.transform);
        
        GameObject buttons = Managers.Resource.Instantiate(_buttonsOfCustomizingUIPath, _customizingUI.transform);
        
        Button nextButton = Managers.Resource.Instantiate(_nextOfOfCustomizingUIPath, buttons.transform).GetComponent<Button>();
        Button prevButton = Managers.Resource.Instantiate(_prevOfOfCustomizingUIPath, buttons.transform).GetComponent<Button>();
        Button turnLeftButton = Managers.Resource.Instantiate(_turnLeftOfCustomizingUIPath, buttons.transform).GetComponent<Button>();
        Button turnRightButton = Managers.Resource.Instantiate(_turnRightOfCustomizingUIPath, buttons.transform).GetComponent<Button>();
        Button nextSkinButton = Managers.Resource.Instantiate(_nextSkinButtonPath, buttons.transform).GetComponent<Button>();
        Button prevSkinButton = Managers.Resource.Instantiate(_prevSkinButtonPath, buttons.transform).GetComponent<Button>();
        Button nextHairColorButton = Managers.Resource.Instantiate(_nextHairButtonPath, buttons.transform).GetComponent<Button>();
        Button prevHairColorButton = Managers.Resource.Instantiate(_prevHairButtonPath, buttons.transform).GetComponent<Button>();
        
        nextButton.onClick.AddListener(NextToStart);
        prevButton.onClick.AddListener(PrevToGenderUI);
        turnLeftButton.onClick.AddListener(TurnLeft);
        turnRightButton.onClick.AddListener(TurnRight);
        nextSkinButton.onClick.AddListener(NextSkinColor);
        prevSkinButton.onClick.AddListener(PrevSkinColor);
        nextHairColorButton.onClick.AddListener(NextHairColor);
        prevHairColorButton.onClick.AddListener(PrevHairColor);
    } 
    
    // 캐릭터 모델 불러오기(남자 아바타가 기본)
    GameObject GenerateCharacter(string gender)
    {
        GameObject go = Managers.Resource.Instantiate(gender);
        go.name = "MyChar";
        go.transform.position = new Vector3(0, 0, 1);
        go.transform.rotation = Quaternion.Euler(0, 180, 0);
        return go;
    } 
    
    // 캐릭터의 스킨메쉬 렌더러 불러오기
    void GetMeshRenderers()
    {
        _hair = _myCharacter.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>();
        _shirts = _myCharacter.transform.GetChild(2).GetComponent<SkinnedMeshRenderer>();
        _pants = _myCharacter.transform.GetChild(3).GetComponent<SkinnedMeshRenderer>();
    }

    // 피부 색 바꾸기
    void ChangeSkinColor(int idx) 
    {
        switch (_gender)
        {
            case Gender.Male:
                GetMeshRenderers();
                Material curMaleHairColor = _hair.materials[1];
                _hair.materials = new Material[] { _skinColors[idx], curMaleHairColor };
                Material curMaleShitsColor = _shirts.materials[1];
                _shirts.materials = new Material[] { _skinColors[idx], curMaleShitsColor };
                break;
            
            case Gender.Female:
                GetMeshRenderers();
                Material curFemaleHairColor = _hair.materials[1];
                _hair.materials = new Material[] { _skinColors[idx], curFemaleHairColor };
                Material curFemaleShitsColor = _shirts.materials[0];
                _shirts.materials = new Material[] { curFemaleShitsColor, _skinColors[idx] };
                Material curFemalePantsColor1 = _pants.materials[0];
                Material curFemalePantsColor2 = _pants.materials[2];
                _pants.materials = new Material[] { curFemalePantsColor1, _skinColors[idx], curFemalePantsColor2 };
                break;
        }
    }
    
    // 머리 색 바꾸기
    void ChangeHairColor(int idx)
    {
        GetMeshRenderers();
        Material curSkinColor = _hair.materials[0];
        _hair.materials = new Material[] { curSkinColor, _hairColors[idx] };
    }
    /************************************************************************/
    /************************************************************************/
    // ButtonEvent
    void ChangeToMale() // 성별 남자로 바꾸기
    {
        if (_myCharacter == null)
            return;
        Managers.Resource.Destroy(_myCharacter);
        _myCharacter = GenerateCharacter(_malePrefabPath);
        _gender = Gender.Male;
    }
    void ChangeToFemale() // 성별 여자로 바꾸기
    {
        if (_myCharacter == null)
            return;
        Managers.Resource.Destroy(_myCharacter);
        _myCharacter = GenerateCharacter(_femalePrefabPath);
        _gender = Gender.Female;
    }
    void TurnLeft() // 캐릭터 왼쪽으로 회전
    {
        float toLeft = 45;
        _myCharacter.transform.Rotate(Vector3.up * toLeft);
    }
    void TurnRight() // 캐릭터 오른쪽으로 회전
    {
        float toRight = 45;
        _myCharacter.transform.Rotate(- Vector3.up * toRight);
    }
    void NextToCustomizingUI() // 파츠 선택 화면으로 넘어가기
    {
        Managers.Resource.Destroy(_genderSelectUI);
        GenerateCustomizingUI();
    }
    void PrevToGenderUI() // 성별 선택 화면으로 돌아가기
    {
        Managers.Resource.Destroy(_customizingUI);
        GenerateGenderUI();
    }
    void NextToStart() // 게임 시작하기(미구현)
    {
        print("Game Start!");
    }
    void NextSkinColor() // 다음 피부색으로 바꾸기
    {
        _skinIndex++;
        if (_skinIndex > _skinColors.Count - 1)
            _skinIndex = 0;
        ChangeSkinColor(_skinIndex);
        _skinColorText.GetComponent<Text>().text = _skinColors[_skinIndex].name;
    }
    void PrevSkinColor() // 이전 피부색으로 바꾸기
    {
        _skinIndex--;
        if (_skinIndex < 0)
            _skinIndex = _skinColors.Count - 1;
        ChangeSkinColor(_skinIndex);
        _skinColorText.GetComponent<Text>().text = _skinColors[_skinIndex].name;
    }
    void NextHairColor() // 다음 머리색으로 바꾸기
    {
        _hairIndex++;
        if (_hairIndex > _hairColors.Count - 1)
            _hairIndex = 0;
        ChangeHairColor(_hairIndex);
        _hairColorText.GetComponent<Text>().text = _hairColors[_hairIndex].name;
    }
    void PrevHairColor() // 이전 머리색으로 바꾸기
    {
        _hairIndex--;
        if (_hairIndex < 0)
            _hairIndex = _hairColors.Count - 1;
        ChangeHairColor(_hairIndex);
        _hairColorText.GetComponent<Text>().text = _hairColors[_hairIndex].name;
    }
}