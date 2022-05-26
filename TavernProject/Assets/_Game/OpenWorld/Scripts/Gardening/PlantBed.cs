using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantBed : MonoBehaviour, IInteractable //make a pickable parent class? or not? is tag enough? interface? 
{
    public Plant heldPlant;

    [HideInInspector] public bool free = true;
    private int _progress;

    private bool _growing = false;
    private int _timesHarvested = 0;

    [SerializeField] private int _watered = 0;
    private const int _maxWatered = 3;

    private float _timeSinceUpgrade = 0;
    public float growthSpeed = 0.1f;
    private Coroutine _wateredCoroutine;

    private GameObject _basePlantObject;

    [SerializeField] private string _itemActionText = "Pick ";
    [SerializeField] private string _plantBedActionText = "Plant";
    [SerializeField] private string _waterActionText = "Water";
    [SerializeField] private MeshRenderer _bedRenderer;
    private InventoryDisplay.voidDelegate2 _itemAction;
    private InventoryDisplay _display;

    private PlantPickable _grownPlant;

    [SerializeField] private Color[] _wateredColors = new Color[_maxWatered];

    public void Interact()
    {
        if (!heldPlant)
        {
            //open inventory to choose seed to plant
            _display = ItemDisplayWindow.CreateDisplay(_itemActionText, PlantByIndex, CancelDisplay, GameManager.Instance.inventory.GetItemsOfType<Seed>());
        }
        else 
        {
            Water();
        }
    }

    public string GetInteractionText()
    {
        //if (_grownPlant) return _itemActionText + _grownPlant.name;
        if (!heldPlant) return _plantBedActionText;
        else return _waterActionText;
    }

    public bool GetInteractive()
    {
        if (_display) return false;
        if (_grownPlant) return false;
        return true;
    }

    public void CancelDisplay()
    {
        ItemDisplayWindow.Destroy(_display.gameObject);
        _display = null;
    }

    private void PlantByIndex(int i, PickableItem item)
    {
        //get player
        //get player inventory
        //plant item nr i
        ItemDisplayWindow.Destroy(_display.gameObject);

        GameManager.Instance.inventory.RemoveItem(item);

        Seed plantItem = (Seed)item;
        if (plantItem) Plant(plantItem.plant);
    }
    private void Start()
    {
        SetWatered(_watered);
    }
    private void FixedUpdate()
    {
        if (heldPlant)
        {
            //count growth phase progress 
            if (_growing && _watered > 0)
            {
                _timeSinceUpgrade += growthSpeed;
            }
            if (_timeSinceUpgrade >= heldPlant.timeNeedGrow)
            {
                UpdateGrowthStage();
            }
        }
    }

    public void Plant(Plant _newPlant)
    {
        if (!free) return;

        _growing = true;
        _progress = 0;
        _timeSinceUpgrade = 0;

        heldPlant = _newPlant;
        _basePlantObject = Instantiate(heldPlant.plantStages[_progress], transform);
    }


    private void UpdateGrowthStage()
    {
        _progress++;
        bool fullyGrown = heldPlant.plantStages.Count <= _progress;

        //if not fully grown:
        //start next growth coroutine, otherwise drop fruit and stop growing until picked
        if (fullyGrown)
        {
            ProducePickableFruit();
            if (heldPlant.pickAll) Destroy(_basePlantObject);
        }
        else
        {
            Destroy(_basePlantObject);
            _basePlantObject = Instantiate(heldPlant.plantStages[_progress], transform);
        }

        _timeSinceUpgrade = 0;
    }

    private void ProducePickableFruit()
    {
        _growing = false;

        //spawn interactable
        _grownPlant = Instantiate(heldPlant.plantPickablePrefab, transform).GetComponent<PlantPickable>();
        _grownPlant.bed = this;
    }

    //called from plantPickable when interacted
    public void Pick()
    {
        _timesHarvested++;
        _growing = true;
        _grownPlant = null;

        bool depleted = heldPlant.finite && heldPlant.cycles <= _timesHarvested;
        //if depleted: remove plant
        if (depleted)
        {
            FreeBed();
        }
        else
        {
            _progress--;
            _timeSinceUpgrade = 0;
        }
    }

    private void FreeBed()
    {
        free = true;
        heldPlant = null;
        if (_basePlantObject) Destroy(_basePlantObject);
    }

    public void Water()
    {
        //watered full
        //coroutine to decrement watered
        SetWatered(_maxWatered-1);
        if (_wateredCoroutine != null) StopCoroutine(_wateredCoroutine);
        _wateredCoroutine = StartCoroutine(LoseWaterCoroutine());

    }
    private IEnumerator LoseWaterCoroutine()
    {
        while (true)
        {
            // TODO: save globally! every plant needs exactly the same amount of time!
            yield return new WaitForSecondsRealtime(GameManager.Instance.worldSettings.waterDuration);
            if (_watered > 0)
            {
                SetWatered(_watered - 1);
            }
        }
    }
    private void SetWatered(int i)
    {
        _watered = i;
        _bedRenderer.material.color = _wateredColors[i];
    }

    //rewrite inventory system so it destroys objects on pickup instead of just hiding them

}
