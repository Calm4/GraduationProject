using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace App.Scripts.Resources
{
  public class ResourcesUI : MonoBehaviour
  {
    [SerializeField] private TMP_Text woodTextField;
    [SerializeField] private TMP_Text ironTextField;
    [SerializeField] private TMP_Text stoneTextField;
    [SerializeField] private TMP_Text skinTextField;
    [SerializeField] private ResourcesManager resourcesManager;

    private void Start()
    {
      UpdateTextFields();
      resourcesManager.OnUpdateResources += UpdateTextFields;
    }

    private void UpdateTextFields()
    {
      woodTextField.text = resourcesManager.GetResourceData(ResourceType.Wood).currentAmount.ToString();
      ironTextField.text = resourcesManager.GetResourceData(ResourceType.Iron).currentAmount.ToString();
      stoneTextField.text = resourcesManager.GetResourceData(ResourceType.Cotton).currentAmount.ToString();
      skinTextField.text = resourcesManager.GetResourceData(ResourceType.Skin).currentAmount.ToString();
    }
  }
}
