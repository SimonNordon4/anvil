#if UNITY_EDITOR
using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEngine;

namespace Anvil
{
    [CustomPropertyDrawer(typeof(FunctionSystem<,>))]
    public class FunctionSystemDrawer : PropertyDrawer
    {
        private SerializedProperty _property;
        private SerializedProperty _functionsProperty;
        private VisualElement _container;
        private VisualElement _parentContainer;


        private VisualElement DrawFoldout()
        {
            var headerFoldout = new Foldout();
            headerFoldout.style.flexDirection = FlexDirection.Column;
            headerFoldout.style.backgroundColor = new Color(0.0f, 0.0f, 0.0f, 0.15f); // Change to your desired color
            
            var fieldName = _property.displayName;
            var functionStruct = fieldInfo.FieldType.GenericTypeArguments.First().Name;
            var functionGeneric = fieldInfo.FieldType.GenericTypeArguments.Last().Name;
            
            // Create a foldout element

            headerFoldout.text = $"{fieldName} ({functionStruct}, {functionGeneric})"; // Set the header text
            headerFoldout.value = true; // Initially open
            headerFoldout.Add(DrawBody());
            return headerFoldout;
        }

        private VisualElement DrawBody()
        {
            var bodyGroup = new VisualElement();
            

            // Access the _buffer and _functions fields
            var functionsProperty = _property.FindPropertyRelative("functions");

            // for each no null function in the list, draw a custom field for it.
            for (var i = 0; i < functionsProperty.arraySize; i++)
            {
                var elementGroup = new VisualElement();
                // change background color to lighter
                elementGroup.style.backgroundColor = new Color(1.0f, 1.0f, 1.0f, 0.05f); // Change to your desired color
                elementGroup.style.marginTop = 5;
                elementGroup.style.marginLeft = -5;
                elementGroup.style.marginRight = 5;
                
                // rund teh corners
                elementGroup.style.borderTopLeftRadius = 2;
                elementGroup.style.borderTopRightRadius = 2;
                elementGroup.style.borderBottomLeftRadius = 2;
                elementGroup.style.borderBottomRightRadius = 2;
                    
                
                // add padding
                elementGroup.style.paddingTop = 4;
                elementGroup.style.paddingLeft = 4;
                elementGroup.style.paddingRight = 4;
                elementGroup.style.paddingBottom = 4;
                
                var element = functionsProperty.GetArrayElementAtIndex(i);
                if (element.managedReferenceValue == null) continue;


                var serializedElement = functionsProperty.GetArrayElementAtIndex(i);

                // Create a horizontal group
                var horizontalGroup = new VisualElement
                {
                    style =
                    {
                        flexDirection = FlexDirection.Row,
                        justifyContent = Justify.SpaceBetween,
                        alignContent = Align.Center
                    }
                };


                // Make the name label a button itself
                var functionName = element.managedReferenceValue.GetType().Name;

                var label = new Label(functionName)
                {
                    style =
                    {
                        unityFontStyleAndWeight = FontStyle.Bold,
                        unityTextAlign = TextAnchor.MiddleCenter
                    }
                };
                horizontalGroup.Add(label);

                var buttonGroup = new VisualElement();

                // make button group horizontal
                buttonGroup.style.flexDirection = FlexDirection.Row;

                // Create an up, down, remove and inspect button.
                var i1 = i;
                var upButton = new Button(() =>
                {
                    // Move the function up in the list
                    var index = functionsProperty.GetArrayElementAtIndex(i1);
                    functionsProperty.MoveArrayElement(i1, i1 - 1);
                    UpdateUI(functionsProperty);
                });

                var i2 = i;
                var downButton = new Button(() =>
                {
                    // Move the function down in the list
                    var index = functionsProperty.GetArrayElementAtIndex(i2);
                    functionsProperty.MoveArrayElement(i2, i2 + 1);
                    UpdateUI(functionsProperty);
                });

                var i3 = i;
                var removeButton = new Button(() =>
                {
                    // Remove the function from the list
                    var index = functionsProperty.GetArrayElementAtIndex(i3);
                    functionsProperty.DeleteArrayElementAtIndex(i3);
                    UpdateUI(functionsProperty);
                });

                upButton.text = "↑";
                downButton.text = "↓";
                removeButton.text = "╳";
                
                if(i==0)
                    upButton.SetEnabled(false);
                if(i == functionsProperty.arraySize-1)
                    downButton.SetEnabled(false);

                buttonGroup.Add(upButton);
                buttonGroup.Add(downButton);
                buttonGroup.Add(removeButton);
                horizontalGroup.Add(buttonGroup);
                elementGroup.Add(horizontalGroup);

                // get the serialized element at index


                SerializedProperty iterator = serializedElement.Copy();
                
                bool enterChildren = true;
                while (iterator.NextVisible(enterChildren))
                {
                    // Make sure to set enterChildren to false for subsequent iterations
                    enterChildren = false;

                    if (iterator.name == "data") continue;

                    var propertyField = new PropertyField(iterator.Copy());
                    // Just had to add this line.
                    propertyField.BindProperty(iterator.Copy());
                    elementGroup.Add(propertyField);
                }
                serializedElement.serializedObject.ApplyModifiedProperties();
                bodyGroup.Add(elementGroup);
            }

            // Add a button to body group that adds a new type of function
            var button = new Button(() =>
            {
                var menu = new GenericMenu();

                // Get the type of the generic type parameter
                var genericType = fieldInfo.FieldType.GenericTypeArguments[1];

                // Find all classes in the assembly that implement the generic type (for example IWishDirectionHandler)
                // Find all classes in the assembly that implement the generic type
                var types = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(s => s.GetTypes())
                    .Where(p => genericType.IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract);

                // Add a menu item for each type
                foreach (var type in types)
                {
                    menu.AddItem(new GUIContent(type.Name), false, () =>
                    {
                        // Add the function to the list
                        var index = functionsProperty.arraySize++;
                        var element = functionsProperty.GetArrayElementAtIndex(index);
                        element.managedReferenceValue = Activator.CreateInstance(type);
                        UpdateUI(functionsProperty);
                    });
                }

                menu.ShowAsContext();
            });
            button.text = "Add Function";
            
            bodyGroup.Add(button);
            return bodyGroup;
        }

        // Override the CreatePropertyGUI method to provide your custom UI
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            _property = property;
            _functionsProperty = property.FindPropertyRelative("_functions");
            
            _container = new VisualElement();
            _container.Add(DrawFoldout());


            // This will bind a change listener to the serialized object, so that whenever it is updated, the UI will also update
            property.serializedObject.ApplyModifiedProperties();
            //property.serializedObject.UpdateIfRequiredOrScript();

            return _container;
        }

        private void UpdateUI(SerializedProperty property)
        {
            // Clear the existing UI and redraw it
            _container.Clear();
            _container.Add(DrawFoldout());
        }
    }
}
#endif