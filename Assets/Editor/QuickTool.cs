using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class QuickTool : EditorWindow
{
    [MenuItem("QuickTool/Open _%#T")]
    public static void ShowWindow()
    {
        var window = GetWindow<QuickTool>();
        window.titleContent = new GUIContent("QuickTool");
        window.minSize = new Vector2(280,50);
    }

   /* public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Creates our button and sets its Text property.
        var myButton = new Button() { text = "My Button" };

        // Give it some style.
        myButton.style.width = 160;
        myButton.style.height = 30;

        // Adds it to the root.
        root.Add(myButton);

    }*/

    private void CreateGUI()
    {
        // Reference to the root of the window.
        var root = rootVisualElement;

        // Associates a stylesheet to our root. Thanks to inheritance, all root’s
        // children will have access to it.
        root.styleSheets.Add(Resources.Load<StyleSheet>("QuickTool_Style"));

        // Loads and clones our VisualTree (eg. our UXML structure) inside the root.
        var quickToolVisualTree = Resources.Load<VisualTreeAsset>("QuickTool_Main");
        quickToolVisualTree.CloneTree(root);

        // Queries all the buttons (via class name) in our root and passes them
        // in the SetupButton method.
        var toolButtons = root.Query(className: "quicktool-button");
        toolButtons.ForEach(SetupButton);
    }

    private void SetupButton(VisualElement button) 
    {
        // Reference to the VisualElement inside the button that serves
        // as the button's icon.
        var buttonIcon = button.Q(className: "quicktool-button-icon");

        // Icon's path in our project.
        var iconPath = "Icons/" + button.parent.name + "_icon";

        // Loads the actual asset from the above path.
        var iconAsset = Resources.Load<Texture2D>(iconPath);

        // Applies the above asset as a background image for the icon.
        buttonIcon.style.backgroundImage = iconAsset;

        // Instantiates our primitive object on a left click.
        button.RegisterCallback<PointerUpEvent, string>(CreateObject, button.parent.name);

        // Sets a basic tooltip to the button itself.
        button.tooltip = button.parent.name;
    }

    private void CreateObject(PointerUpEvent _, string primitiveTypeName)
    {    
        var pt = (PrimitiveType) System.Enum.Parse
                    (typeof(PrimitiveType), primitiveTypeName, true);
        var go = ObjectFactory.CreatePrimitive(pt);
        go.transform.position = Vector3.zero;
    }


}
