﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static DaftAppleGames.CreaturePetModSn.CreaturePetModSnPlugin;

namespace CreaturePetMod_SN.Utils
{
    /// <summary>
    /// Utilities class to help construct custom UIs
    /// </summary>
    internal static class ModUiUtils
    {
        /// <summary>
        /// Disables "original" UI elements in the source UI
        /// Returns a "new" clean one for use in the mod
        /// </summary>
        /// <param name="sourceUi"></param>
        /// <param name="newScreenName"></param>
        public static GameObject InitUi(GameObject sourceUi, string newScreenName, 
            string activeScreenName = "Active", string inactiveScreenName = "Inactive")
        {
            // Disable "Active" and "Inactive"
            Log.LogDebug($"ModUiUtils: InitUi looking for active screen {activeScreenName}...");
            GameObject activeScreen = sourceUi.transform.Find(activeScreenName).gameObject;
            Log.LogDebug($"ModUiUtils: InitUi looking for inactive screen {inactiveScreenName}...");
            GameObject inactiveScreen = sourceUi.transform.Find(inactiveScreenName).gameObject;

            Log.LogDebug($"ModUiUtils: InitUi disabling active and inactive screens...");
            activeScreen.SetActive(false);
            inactiveScreen.SetActive(false);

            Log.LogDebug($"ModUiUtils: InitUi cleaning up...");
            SubNameInput subNameInput = sourceUi.GetComponent<SubNameInput>();
            GameObject.Destroy(subNameInput);

            Log.LogDebug($"ModUiUtils: InitUi creating new screen...");
            GameObject newScreen = GameObject.Instantiate(activeScreen);
            newScreen.name = newScreenName;
            newScreen.transform.SetParent(sourceUi.transform);
            newScreen.transform.position = inactiveScreen.transform.position;
            newScreen.transform.rotation = inactiveScreen.transform.rotation;
            newScreen.transform.localScale = inactiveScreen.transform.localScale;

            // Deactivate all existing content
            foreach (Transform child in newScreen.transform)
            {
                child.gameObject.SetActive(false);
            }

            Image backgroundImage = newScreen.GetComponent<Image>();
            if (backgroundImage)
            {
                backgroundImage.enabled = false;
            }

            Log.LogDebug("ModUiUtils: InitUi setting up CanvasRenderer...");
            

            newScreen.SetActive(true);

            Log.LogDebug($"ModUiUtils: InitUi done.");

            return newScreen;
        }

        /// <summary>
        /// Creates a new button and returns it as a GameObject
        /// </summary>
        /// <param name="sourceUi"></param>
        /// <param name="sourceButtonName"></param>
        /// <param name="newButtonName"></param>
        /// <param name="newButtonText"></param>
        /// <param name="targetUi"></param>
        /// <param name="localPosition"></param>
        /// <returns></returns>
        public static GameObject CreateButton(GameObject sourceUi, string sourceButtonName, string newButtonName, string newButtonText, GameObject targetUi, Vector3 localPosition)
        {
            GameObject origButtonGameObject = null;

            foreach (Transform child in sourceUi.GetComponentsInChildren<Transform>(true))
            {
                // Log.LogDebug($"UiUtils: CreateButton comparing: {child.name} to {sourceButtonName}...");
                if (child.name == sourceButtonName && child.GetComponent<Button>())
                {
                    origButtonGameObject = child.gameObject;
                    break;
                }
            }

            if (!origButtonGameObject)
            {
                Log.LogDebug($"UiUtils: CreateButton can't find a Button in {sourceUi}");
                return null;
            }

            // Clone the button Game Object
            GameObject newButtonGameObject = GameObject.Instantiate(origButtonGameObject);

            // Set new button properties
            newButtonGameObject.name = newButtonName;

            // Find and set the label
            TextMeshProUGUI buttonLabel = newButtonGameObject.GetComponentInChildren<TextMeshProUGUI>(true);
            if (buttonLabel)
            {
                buttonLabel.text = newButtonText.ToUpper();
            }
            else
            {
                Log.LogDebug($"UiUtils: CreateButton can't find a TextMeshProUGUI component in {newButtonGameObject}");
            }

            newButtonGameObject.transform.SetParent(targetUi.transform);
            newButtonGameObject.transform.localPosition = localPosition;
            newButtonGameObject.transform.localRotation = new Quaternion(0, 0, 0, 0);
            newButtonGameObject.transform.localScale = new Vector3(1, 1, 1);

            newButtonGameObject.SetActive(true);

            return newButtonGameObject;

        }

        public static GameObject CreateTextEntry(GameObject sourceUi, string sourceTextName, string newTextName, GameObject targetUi, Vector3 localPosition)
        {
            GameObject origTextGameObject = null;

            foreach (Transform child in sourceUi.GetComponentsInChildren<Transform>(true))
            {
                if (child.name == sourceTextName && child.GetComponent<uGUI_InputField>())
                {
                    origTextGameObject = child.gameObject;
                    break;
                }
            }

            if (!origTextGameObject)
            {
                Log.LogDebug($"UiUtils: CreateButton can't find a TextEntry in {sourceUi}");
                return null;
            }

            // Clone the button Game Object
            GameObject newTextGameObject = GameObject.Instantiate(origTextGameObject);

            // Set new button properties
            newTextGameObject.name = sourceTextName;


            newTextGameObject.transform.SetParent(targetUi.transform);
            newTextGameObject.transform.localPosition = localPosition;
            newTextGameObject.transform.localRotation = new Quaternion(0, 0, 0, 0);
            newTextGameObject.transform.localScale = new Vector3(1, 1, 1);

            newTextGameObject.SetActive(true);

            return newTextGameObject;
        }
    }
}
