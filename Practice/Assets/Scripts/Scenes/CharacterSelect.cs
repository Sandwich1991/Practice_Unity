using System;
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
    private GameObject _genderSelectUI;
    private GameObject _customizingUI;
    private GameObject[] _characters;
    private GameObject _myCharacter;
    private SkinnedMeshRenderer _hair;
    private SkinnedMeshRenderer _shirts;
    private SkinnedMeshRenderer _pants;
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
    private string _pantsOfCutomizingUIPath = "UI/CustomizingUI/Pants";
    private string _shirtsOfCutomizingUIPath = "UI/CustomizingUI/Shirts";
    private string _skinOfCutomizingUIPath = "UI/CustomizingUI/Skin";
    private string _hairOfCustomizingUIPath = "UI/CustomizingUI/Hair";
    #endregion

    #region Character Path
    private string basePref = "Character/Base";
    private string _femalePref = "Character/Female";
    private string _malePref = "Character/male";
    #endregion
    /************************************************************************/
    /************************************************************************/
    protected override void init()
    {
        base.init();

        GenerateGenderUI();
        _myCharacter = GenerateCharacter();
        _gender = Gender.Male;
        Transform child = _myCharacter.transform.GetChild(0);
        _hair = child.GetChild(1).GetComponent<SkinnedMeshRenderer>();
        _shirts = child.GetChild(2).GetComponent<SkinnedMeshRenderer>();
        _pants = child.GetChild(3).GetComponent<SkinnedMeshRenderer>();
    }
    public override void Clear()
    {
        
    }
    /************************************************************************/
    /************************************************************************/
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
    void GenerateCustomizingUI()
    {
        _customizingUI = Managers.Resource.Instantiate(_custominzingUIPath);
        GameObject texts = Managers.Resource.Instantiate(_textsOfCutomizingUIPath, _customizingUI.transform);

        GameObject skin = Managers.Resource.Instantiate(_skinOfCutomizingUIPath, texts.transform);
        GameObject shirt = Managers.Resource.Instantiate(_shirtsOfCutomizingUIPath, texts.transform);
        GameObject pants = Managers.Resource.Instantiate(_pantsOfCutomizingUIPath, texts.transform);
        GameObject hair = Managers.Resource.Instantiate(_hairOfCustomizingUIPath, texts.transform);
        
        GameObject buttons = Managers.Resource.Instantiate(_buttonsOfCustomizingUIPath, _customizingUI.transform);
        
        Button nextButton = Managers.Resource.Instantiate(_nextOfOfCustomizingUIPath, buttons.transform).GetComponent<Button>();
        Button prevButton = Managers.Resource.Instantiate(_prevOfOfCustomizingUIPath, buttons.transform).GetComponent<Button>();
        Button turnLeftButton = Managers.Resource.Instantiate(_turnLeftOfCustomizingUIPath, buttons.transform).GetComponent<Button>();
        Button turnRightButton = Managers.Resource.Instantiate(_turnRightOfCustomizingUIPath, buttons.transform).GetComponent<Button>();
        
        nextButton.onClick.AddListener(NextToStart);
        prevButton.onClick.AddListener(PrevToGenderUI);
        turnLeftButton.onClick.AddListener(TurnLeft);
        turnRightButton.onClick.AddListener(TurnRight);
    }
    GameObject GenerateCharacter()
    {
        GameObject go = Managers.Resource.Instantiate(basePref);
        GameObject male = Managers.Resource.Instantiate(_malePref, go.transform);
        go.name = "MyChar";
        go.transform.position = new Vector3(0, 0, 1);
        go.transform.rotation = Quaternion.Euler(0, 0, 0);
        return go;
    }
    void GetMeshRenderer()
    {
        Transform child = _myCharacter.transform.GetChild(0);
        _hair = child.GetChild(1).GetComponent<SkinnedMeshRenderer>();
        _shirts = child.GetChild(2).GetComponent<SkinnedMeshRenderer>();
        _pants = child.GetChild(3).GetComponent<SkinnedMeshRenderer>();
    }
    void ChangeSkin(Material newColor)
    {
        switch (_gender)
        {
            case Gender.Male:
                GetMeshRenderer();
                Material curMaleHairColor = _hair.materials[1];
                _hair.materials = new Material[] { newColor, curMaleHairColor };
                Material curMaleShitsColor = _shirts.materials[1];
                _shirts.materials = new Material[] { newColor, curMaleShitsColor };
                break;
            
            case Gender.Female:
                GetMeshRenderer();
                Material curFemaleHairColor = _hair.materials[1];
                _hair.materials = new Material[] { newColor, curFemaleHairColor };
                Material curFemaleShitsColor = _shirts.materials[0];
                _shirts.materials = new Material[] { curFemaleShitsColor, newColor };
                Material curFemalePantsColor1 = _pants.materials[0];
                Material curFemalePantsColor2 = _pants.materials[2];
                _pants.materials = new Material[] { curFemalePantsColor1, newColor, curFemalePantsColor2 };
                break;
                
        }
    }
    /************************************************************************/
    /************************************************************************/
    // ButtonEvent
    void ChangeToMale()
    {
        if (_myCharacter == null)
            return;
        Managers.Resource.Destroy(_myCharacter.transform.GetChild(0).gameObject);
        GameObject male = Managers.Resource.Instantiate(_malePref, _myCharacter.transform);
        _gender = Gender.Male;
    }
    void ChangeToFemale()
    {
        if (_myCharacter == null)
            return;
        Managers.Resource.Destroy(_myCharacter.transform.GetChild(0).gameObject);
        GameObject female = Managers.Resource.Instantiate(_femalePref, _myCharacter.transform);
        _gender = Gender.Female;
    }
    void TurnLeft()
    {
        float toLeft = 45;
        _myCharacter.transform.Rotate(Vector3.up * toLeft);
    }
    void TurnRight()
    {
        float toRight = 45;
        _myCharacter.transform.Rotate(- Vector3.up * toRight);
    }
    void NextToCustomizingUI()
    {
        Managers.Resource.Destroy(_genderSelectUI);
        GenerateCustomizingUI();
    }
    void PrevToGenderUI()
    {
        Managers.Resource.Destroy(_customizingUI);
        GenerateGenderUI();
    }
    void NextToStart()
    {
        print("Game Start!");
    }

    public void ChangeToDark()
    {
        Material dark = Managers.Resource.Load<Material>("Art/Characters/Materials/Skin/dark");
        ChangeSkin(dark);
    }
}