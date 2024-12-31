using System.Reflection;
using NyanMath.Engine.Parser;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace NyanMath
{
    public class NyanMathPlugin : MonoBehaviour, VNyanInterface.IButtonClickedHandler, VNyanInterface.ITriggerHandler
    {
        [FormerlySerializedAs("windowPrefab")] public GameObject aboutWindow;

        private GameObject _aboutWindow;

        public void Awake()
        {
            VNyanInterface.VNyanInterface.VNyanUI.registerPluginButton("NyanMath", this);
            VNyanInterface.VNyanInterface.VNyanTrigger.registerTriggerListener(this);

            _aboutWindow = (GameObject) VNyanInterface.VNyanInterface.VNyanUI.instantiateUIPrefab(aboutWindow);

            if (_aboutWindow != null)
            {
                _aboutWindow.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
                _aboutWindow.SetActive(false);

                _aboutWindow.transform.Find("Panel/CloseButton").GetComponent<Button>()?.onClick
                    .AddListener(pluginButtonClicked);
                _aboutWindow.transform.Find("Panel/GHButton").GetComponent<Button>()?.onClick
                    .AddListener(() => Application.OpenURL("https://github.com/lumibnuuy/NyanMath"));

                _aboutWindow.transform.Find("Panel/Version").GetComponent<Text>().text =
                    Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public void pluginButtonClicked()
        {
            if (_aboutWindow != null)
            {
                _aboutWindow.SetActive(!_aboutWindow.activeSelf);
                if (_aboutWindow.activeSelf)
                {
                    _aboutWindow.transform.SetAsLastSibling();
                }
            }
        }

        public void triggerCalled(string triggerName, int value1, int value2, int value3, string text1, string text2,
            string text3)
        {
            if (triggerName != "NyanMath")
            {
                return;
            }

            var expression = text1;
            var outputParam = text2;

            if (string.IsNullOrEmpty(outputParam))
            {
                outputParam = "NyanMathResult";
            }

            try
            {
                var result = Evaluator.Evaluate(expression);

                VNyanInterface.VNyanInterface.VNyanParameter.setVNyanParameterFloat(outputParam, result);
                VNyanInterface.VNyanInterface.VNyanParameter.setVNyanParameterString("NyanMathError", "");
            }
            catch (ParseException e)
            {
                Debug.LogError($"Error parsing expression: {e.Message}");
                VNyanInterface.VNyanInterface.VNyanParameter.setVNyanParameterString("NyanMathError", e.ToString());
            }
        }
    }
}