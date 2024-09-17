using LoadSave;
using Scenes._10_PlayerScene.Scripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CarShowCaseRoomManager : MonoBehaviour
{

    [SerializeField] private List<GameObject> ShowcasedCar = new List<GameObject>();
    [SerializeField] private List<CarMaterialInfo> CarListMaterials;
    [SerializeField] private Transform ShowcasedSpawnPoint;
    private GameObject spawnedCar;
    [SerializeField] private float rotationSpeed = 20f;  // Adjust speed here
    string clickedMaterialName = string.Empty;


    private PlayerData playerData;
    [SerializeField] private TextMeshProUGUI LettersTxt;
    private int lettersCount;
    [SerializeField] private Sprite buyImg;
    [SerializeField] private Sprite equipImg;
    [SerializeField] private Sprite haveEquipImg;
    [SerializeField] private Image imgHolder;
    [SerializeField] private Image priceHolder;
    [SerializeField] TextMeshProUGUI price;

    private CarShowCaseButtons buttonInstance;
    private string clickedButtonName;
    // Start is called before the first frame update
    void Start()
    {
        playerData = PlayerManager.Instance.SpawnedPlayer.GetComponent<PlayerData>();
        UpdateValues();
        foreach (var item in PlayerManager.Instance.SpawnedPlayer.GetComponent<PlayerData>().listOfCars)
        {
            if (item.IsActive)
            {
                SpawnCar(ReturnThePlayerCar(item.Name));
                PreviewColorOfCar(new CarShowCaseButtons(ReturnTheRightMaterial(item.MaterialName), item.MaterialName));
                StartCoroutine(StartRotationOfCar());
                break;
            }
        }


        //SpawnCar(ShowcasedCar[0]);
        //StartCoroutine(StartRotationOfCar());
    }
    private void UpdateValues()
    {
        lettersCount = playerData.CollectedLetters.Count;
        LettersTxt.text = lettersCount.ToString();
    }
    private GameObject ReturnThePlayerCar(string value)
    {
        foreach (var item in ShowcasedCar)
        {
            if (value == item.name)
            {
                return item;
            }
        }
        return null;
    }
    private Material ReturnTheRightMaterial(string value)
    {
        foreach (var item in CarListMaterials)
        {
            if (value == item.CarName)
            {
                return item.CarMaterial;
            }
        }
        return null;
    }


    public void PreviewColorOfCar(CarShowCaseButtons previewMaterial)
    {
        Transform carBodyTransform = spawnedCar.transform.Find("Body");

        if (carBodyTransform != null)
        {
            Renderer carRenderer = carBodyTransform.GetComponent<Renderer>();

            if (carRenderer != null)
            {
                // Set the new material
                carRenderer.material = previewMaterial.material;
                clickedMaterialName = previewMaterial.nameOfMaterial;
            }
            else
            {
                Debug.LogWarning("Renderer component not found on the car's body.");
            }
        }
        else
        {
            Debug.LogWarning("Car body child object not found.");
        }
    }

    public void SettingButtonsUp(CarShowCaseButtons button)
    {
        buttonInstance = button;
        if (button.Bought)
        {
            priceHolder.enabled = false;
            price.enabled = false;
            imgHolder.sprite = equipImg;
        }
        else if (!button.Bought)
        {
            priceHolder.enabled = true;
            imgHolder.sprite = buyImg;
            price.text = button.price.ToString();
        }
    }
    //public void SetStringNameOfMaterial(string materialName) => clickedMaterialName = materialName;


    public void SaveMaterialName()
    {
        if (buttonInstance.Bought)
        {
            PlayerManager.Instance.SpawnedPlayer.GetComponent<PlayerData>().listOfCars[0].MaterialName = clickedMaterialName;
        }
        else
        {
            //Buying a color
            if (buttonInstance.price <= lettersCount)
            {
                RemoveLetters(buttonInstance.price);
                var tmp = GameObject.Find(clickedButtonName);
                tmp.GetComponent<CarShowCaseButtons>().Bought = true;
                PlayerManager.Instance.SpawnedPlayer.GetComponent<PlayerData>().listOfCars[0].MaterialName = clickedMaterialName;

                priceHolder.enabled = false;
                price.enabled = false;
                imgHolder.sprite = equipImg;

                UpdateValues();
            }
            else
            {
                print("Can't afford the color");
            }
        }
    }
    public void SetButtonName(GameObject gO) => clickedButtonName = gO.name;
    private void RemoveLetters(int amountTimes)
    {
        for (int i = 0; i < amountTimes; i++)
        {
            playerData.CollectedLetters.RemoveAt(0);
        }
    }

    private void SpawnCar(GameObject ActiveCar)
    {
        spawnedCar = Instantiate(ActiveCar, ShowcasedSpawnPoint);
    }
    private IEnumerator StartRotationOfCar()
    {
        while (true)
        {
            // Rotate smoothly at a constant speed around the Y-axis
            ShowcasedSpawnPoint.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
            yield return null;  // Yield until the next frame
        }
    }
}
