using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CharacterSelect : BaseScene
{
    /************************************************************************/
    #region Enums
    enum Gender
    {
        Male,
        Female
    }

    enum Parts
    {
        Skin,
        Hair,
        Shirts,
        Pants,
    }

    enum UI
    {
        GenderUI,
        CustomizingUI,
        EnterNameUI,
    }
    #endregion
    /************************************************************************/
    /************************************************************************/
    #region Fields
    // UI / Character
    private GameObject _mainUI;
    private GameObject _nextButton;
    private GameObject _prevButton;
    private GameObject _genderSelectUI;
    private GameObject _customizingUI;
    private GameObject _enterNameUI;
    private GameObject _inputName;
    private  GameObject _myCharacter;
    private Gender _gender;
    private UI _ui = UI.GenderUI;
    
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

    // clothes color
    private int _shirtsIndex = 0;
    private int _pantsIndex = 0;
    private GameObject _shirtsColorText;
    private GameObject _pantsColorText;
    private List<Material> _fabricColors;
    #endregion

    #region UI Paths
    // GenderUI Path
    private string _genderUIPath = "UI/GenderUI/GenderUI";
    private string _femaleOfGenderUIPath = "UI/GenderUI/Buttons/FemaleButton";
    private string _maleOfGenderUIPath = "UI/GenderUI/Buttons/MaleButton";
    private string _turnLeftOfGenderUIPath = "UI/GenderUI/Buttons/TurnLeftButton";
    private string _turnRightOfGenderUIPath = "UI/GenderUI/Buttons/TurnRightButton";
    private string _buttonsOfGenderUIPath = "UI/GenderUI/Buttons";
    private string _textOfGenderUIPath = "UI/GenderUI/Text";

    // CutominzingUI Path
    private string _custominzingUIPath = "UI/CustomizingUI/CustomizingUI";
    private string _buttonsOfCustomizingUIPath = "UI/CustomizingUI/Buttons";
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
    private string _nextShirtsButtonPath = "UI/CustomizingUI/Buttons/NextShirtsColor";
    private string _nextPantsButtonPath = "UI/CustomizingUI/Buttons/NextPantsColor";
    private string _prevShirtsButtonPath = "UI/CustomizingUI/Buttons/PrevShirtsColor";
    private string _prevPantsButtonPath = "UI/CustomizingUI/Buttons/PrevPantsColor";
    string _shirtsColorTextPath = "UI/CustomizingUI/ShirtsColorText";
    string _pantsColorTextPath = "UI/CustomizingUI/PantsColorText";
    
    // EnterNameUI Path
    private string _enterNameUIPath = "UI/EnterNameUI/EnterNameUI";
    private string _inputPath = "UI/EnterNameUI/Input";
    private string _confirmButtonPath = "UI/EnterNameUI/ConfirmButton";
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

        GenerateMainUI(); // 성별 선택 UI 생성
        _myCharacter = GenerateCharacter(_malePrefabPath); // 초기 캐릭터 생성(기본 남자)
        _gender = Gender.Male;
        GetMeshRenderers(); // 캐릭터의 메쉬 렌더러 불러오기
        _skinColors = Utills.GetFileListInDir<Material>("Art/Characters/Materials/Skin/"); // 스킨 메테리얼 리스트로 불러오기
        _hairColors = Utills.GetFileListInDir<Material>("Art/Characters/Materials/Hair/"); // 머리색 메테리얼 리스트로 불러오기
        _fabricColors = Utills.GetFileListInDir<Material>("Art/Characters/Materials/Fabric/"); // 옷 색 메테리얼 리스트 불러오기
    }
    public override void Clear()
    {
        _skinColors = null;
        _hairColors = null;
        _fabricColors = null;
    }
    /************************************************************************/
    /************************************************************************/
    // Methods
    
    // 메인 UI 불러오기
    void GenerateMainUI()
    {
        if (_mainUI == null)
        {
            _mainUI = Managers.Resource.Instantiate("UI/MainUI");
            _nextButton = Managers.Resource.Instantiate("UI/NextButton", _mainUI.transform);
            _nextButton.GetComponent<Button>().onClick.AddListener(NextButtonEvent);
            _prevButton = Managers.Resource.Instantiate("UI/PrevButton", _mainUI.transform);
            _prevButton.GetComponent<Button>().onClick.AddListener(PrevButtonEvent);
        }
        
        switch (_ui)
        {
            case UI.GenderUI:
                _prevButton.SetActive(false);
                GenerateGenderUI();
                break;
            
            case UI.CustomizingUI:
                _prevButton.SetActive(true);
                GenerateCustomizingUI();
                break;
            
            case UI.EnterNameUI:
                _prevButton.SetActive(true);
                GenerateEnterNameUI();
                break;
        }
    }
    
    // 성별 선택 UI 불러오기
    void GenerateGenderUI()
    {
        if (_mainUI == null)
            GenerateMainUI();
        
        _genderSelectUI = Managers.Resource.Instantiate(_genderUIPath);
        GameObject text = Managers.Resource.Instantiate(_textOfGenderUIPath, _genderSelectUI.transform);
        GameObject buttons = Managers.Resource.Instantiate(_buttonsOfGenderUIPath, _genderSelectUI.transform);
        
        // 버튼 추가
        Button femaleButton = Managers.Resource.Instantiate(_femaleOfGenderUIPath, buttons.transform).GetComponent<Button>();
        Button maleButton = Managers.Resource.Instantiate(_maleOfGenderUIPath, buttons.transform).GetComponent<Button>();
        Button turnLeftButton = Managers.Resource.Instantiate(_turnLeftOfGenderUIPath, buttons.transform).GetComponent<Button>();
        Button turnRightButton = Managers.Resource.Instantiate(_turnRightOfGenderUIPath, buttons.transform).GetComponent<Button>();

        // 버튼 이벤트 추가
        femaleButton.onClick.AddListener(ChangeToFemale);
        maleButton.onClick.AddListener(ChangeToMale);
        turnLeftButton.onClick.AddListener(TurnLeft);
        turnRightButton.onClick.AddListener(TurnRight);
    } 
    
    // 캐릭터 파츠 선택 UI 불러오기
    void GenerateCustomizingUI()
    {
        if (_mainUI == null)
            GenerateMainUI();
        
        _customizingUI = Managers.Resource.Instantiate(_custominzingUIPath);
        GameObject texts = Managers.Resource.Instantiate(_textsOfCutomizingUIPath, _customizingUI.transform);

        GameObject skinText = Managers.Resource.Instantiate(_skinTextOfCutomizingUIPath, texts.transform);
        GameObject shirtText = Managers.Resource.Instantiate(_shirtsTextOfCutomizingUIPath, texts.transform);
        GameObject pantsText = Managers.Resource.Instantiate(_pantsTextOfCutomizingUIPath, texts.transform);
        GameObject hairText = Managers.Resource.Instantiate(_hairTextOfCustomizingUIPath, texts.transform);
        
        // 캐릭터 색상이름 표시
        _skinColorText = Managers.Resource.Instantiate(_skinColorTextPath, texts.transform);
        _skinColorText.GetComponent<Text>().text = GetMaterialName(Parts.Skin);
        
        _hairColorText = Managers.Resource.Instantiate(_hairColorTextPath, texts.transform);
        _hairColorText.GetComponent<Text>().text = GetMaterialName(Parts.Hair);
        
        _shirtsColorText = Managers.Resource.Instantiate(_shirtsColorTextPath, texts.transform);
        _shirtsColorText.GetComponent<Text>().text = GetMaterialName(Parts.Shirts);
        
        _pantsColorText = Managers.Resource.Instantiate(_pantsColorTextPath, texts.transform);
        _pantsColorText.GetComponent<Text>().text = GetMaterialName(Parts.Pants);
        
        GameObject buttons = Managers.Resource.Instantiate(_buttonsOfCustomizingUIPath, _customizingUI.transform);
        
        // 버튼 추가
        Button turnLeftButton = Managers.Resource.Instantiate(_turnLeftOfCustomizingUIPath, buttons.transform).GetComponent<Button>();
        Button turnRightButton = Managers.Resource.Instantiate(_turnRightOfCustomizingUIPath, buttons.transform).GetComponent<Button>();
        Button nextSkinButton = Managers.Resource.Instantiate(_nextSkinButtonPath, buttons.transform).GetComponent<Button>();
        Button prevSkinButton = Managers.Resource.Instantiate(_prevSkinButtonPath, buttons.transform).GetComponent<Button>();
        Button nextHairColorButton = Managers.Resource.Instantiate(_nextHairButtonPath, buttons.transform).GetComponent<Button>();
        Button prevHairColorButton = Managers.Resource.Instantiate(_prevHairButtonPath, buttons.transform).GetComponent<Button>();
        Button nextShirtsColorButton = Managers.Resource.Instantiate(_nextShirtsButtonPath, buttons.transform).GetComponent<Button>();
        Button prevShirtsColorButton = Managers.Resource.Instantiate(_prevShirtsButtonPath, buttons.transform).GetComponent<Button>();
        Button nextPantsColorButton = Managers.Resource.Instantiate(_nextPantsButtonPath, buttons.transform).GetComponent<Button>();
        Button prevPantsColorButton = Managers.Resource.Instantiate(_prevPantsButtonPath, buttons.transform).GetComponent<Button>();
        
        // 버튼 이벤트 추가
        turnLeftButton.onClick.AddListener(TurnLeft);
        turnRightButton.onClick.AddListener(TurnRight);
        nextSkinButton.onClick.AddListener(NextSkinColor);
        prevSkinButton.onClick.AddListener(PrevSkinColor);
        nextHairColorButton.onClick.AddListener(NextHairColor);
        prevHairColorButton.onClick.AddListener(PrevHairColor);
        nextShirtsColorButton.onClick.AddListener(NextShirtsColor);
        prevShirtsColorButton.onClick.AddListener(PrevShirtsColor);
        nextPantsColorButton.onClick.AddListener(NextPantsColor);
        prevPantsColorButton.onClick.AddListener(PrevPantsColor);
    }
    
    // 이름 입력 화면
    void GenerateEnterNameUI() 
    {
        if (_mainUI == null)
            GenerateMainUI();
        
        _enterNameUI = Managers.Resource.Instantiate(_enterNameUIPath);
        _inputName = Managers.Resource.Instantiate(_inputPath, _enterNameUI.transform);
        Button confirmButton = Managers.Resource.Instantiate(_confirmButtonPath, _enterNameUI.transform).GetComponent<Button>();

        confirmButton.onClick.AddListener(ConfirmName);
    }
    
    // 캐릭터 모델 불러오기(남자 아바타가 기본)
    GameObject GenerateCharacter(string path)
    {
        GameObject go = Managers.Resource.Instantiate(path);
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
    
    // 상의 색 바꾸기
    void ChangeShritsColor(int idx)
    {
        switch (_gender)
        {
            case Gender.Female:
                GetMeshRenderers();
                Material curFemaleSkinColor = _shirts.materials[1];
                _shirts.materials = new Material[] { _fabricColors[idx], curFemaleSkinColor };
                break;
            case Gender.Male:
                GetMeshRenderers();
                Material curMaleSkinColor = _shirts.materials[0];
                _shirts.materials = new Material[] { curMaleSkinColor, _fabricColors[idx] };
                break;
        }
    }
    
    // 하의 색 바꾸기
    void ChangePantsColor(int idx)
    {
        switch (_gender)
        {
            case Gender.Female:
                GetMeshRenderers();
                Material curFemaleSkinColor = _pants.materials[1];
                Material curFabricColor = _pants.materials[2];
                _pants.materials = new Material[] { _fabricColors[idx], curFemaleSkinColor, curFabricColor };
                break;
            case Gender.Male:
                GetMeshRenderers();
                Material curBootsSkinColor = _shirts.materials[1];
                _pants.materials = new Material[] { _fabricColors[idx], curBootsSkinColor };
                break;
        }
    }

    string GetMaterialName(Parts part)
    {
        GetMeshRenderers();
        string colorName = null;
        
        switch (part)
        {
            case Parts.Skin:
                colorName = _hair.materials[0].name;
                break;
            
            case Parts.Hair:
                colorName = _hair.materials[1].name;
                break;
            
            case Parts.Shirts:
                switch (_gender)
                {
                    case Gender.Male:
                        colorName = _shirts.materials[1].name;
                        break;
                    case Gender.Female:
                        colorName = _shirts.materials[0].name;
                        break;
                }
                break;
            
            case Parts.Pants:
                colorName = _pants.materials[0].name;
                break;
        }

        if (colorName.Contains("(Instance)"))
        {
            int idx = colorName.LastIndexOf(" ");
            colorName = colorName.Substring(0, idx);
        }
        return colorName;
    }

    void ReturnMyCharacter()
    {
        Managers.MyCharacter = _myCharacter;
        DontDestroyOnLoad(Managers.MyCharacter);
    }
    /************************************************************************/
    /************************************************************************/
    // ButtonEvent
    // UI 버튼에 추가할 메소드
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

    void NextSkinColor() // 다음 피부색으로 바꾸기
    {
        _skinIndex++;
        if (_skinIndex > _skinColors.Count - 1)
            _skinIndex = 0;
        ChangeSkinColor(_skinIndex);
        _skinColorText.GetComponent<Text>().text = GetMaterialName(Parts.Skin);
    }
    
    void PrevSkinColor() // 이전 피부색으로 바꾸기
    {
        _skinIndex--;
        if (_skinIndex < 0)
            _skinIndex = _skinColors.Count - 1;
        ChangeSkinColor(_skinIndex);
        _skinColorText.GetComponent<Text>().text = GetMaterialName(Parts.Skin);
    }
    
    void NextHairColor() // 다음 머리색으로 바꾸기
    {
        _hairIndex++;
        if (_hairIndex > _hairColors.Count - 1)
            _hairIndex = 0;
        ChangeHairColor(_hairIndex);
        _hairColorText.GetComponent<Text>().text = GetMaterialName(Parts.Hair);
    }
    
    void PrevHairColor() // 이전 머리색으로 바꾸기
    {
        _hairIndex--;
        if (_hairIndex < 0)
            _hairIndex = _hairColors.Count - 1;
        ChangeHairColor(_hairIndex);
        _hairColorText.GetComponent<Text>().text = GetMaterialName(Parts.Hair);
    }
    
    void NextShirtsColor() // 다음 상의색으로 바꾸기
    {
        _shirtsIndex++;
        if (_shirtsIndex > _fabricColors.Count - 1)
            _shirtsIndex = 0;
        ChangeShritsColor(_shirtsIndex);
        _shirtsColorText.GetComponent<Text>().text = GetMaterialName(Parts.Shirts);
    }
    
    void PrevShirtsColor() // 이전 상의색으로 바꾸기
    {
        _shirtsIndex--;
        if (_shirtsIndex < 0)
            _shirtsIndex = _fabricColors.Count - 1;
        ChangeHairColor(_shirtsIndex);
        _shirtsColorText.GetComponent<Text>().text = GetMaterialName(Parts.Shirts);
    }
    
    void NextPantsColor() // 다음 하의색으로 바꾸기
    {
        _pantsIndex++;
        if (_pantsIndex > _fabricColors.Count - 1)
            _pantsIndex = 0;
        ChangePantsColor(_pantsIndex);
        _pantsColorText.GetComponent<Text>().text = GetMaterialName(Parts.Pants);
    }
    
    void PrevPantsColor() // 이전 하의색으로 바꾸기
    {
        _pantsIndex--;
        if (_pantsIndex < 0)
            _pantsIndex = _fabricColors.Count - 1;
        ChangePantsColor(_pantsIndex);
        _pantsColorText.GetComponent<Text>().text = GetMaterialName(Parts.Pants);
    }
    
    void ConfirmName() // 입력한 이름 선택
    {
        _myCharacter.name = _inputName.GetComponent<InputField>().text;
    }

    void NextButtonEvent()
    {
        switch (_ui)
        {
            case UI.GenderUI:
                _ui = UI.CustomizingUI;
                Managers.Resource.Destroy(_genderSelectUI);
                GenerateMainUI();
                break;
            case UI.CustomizingUI:
                _ui = UI.EnterNameUI;
                Managers.Resource.Destroy(_customizingUI);
                GenerateMainUI();
                break;
            case UI.EnterNameUI:
                ReturnMyCharacter();
                Managers.Resource.Destroy(_enterNameUI);
                Managers.Scene.Clear();
                Managers.Scene.LoadScene(Define.Scene.Game);
                break;
        }
    }
    
    void PrevButtonEvent()
    {
        switch (_ui)
        {
            case UI.GenderUI:
                break;
            case UI.CustomizingUI:
                _ui = UI.GenderUI;
                Managers.Resource.Destroy(_customizingUI);
                GenerateMainUI();
                break;
            case UI.EnterNameUI:
                _ui = UI.CustomizingUI;
                Managers.Resource.Destroy(_enterNameUI);
                GenerateMainUI();
                break;
        }
    }
}