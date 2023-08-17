// #if UNITY_EDITOR
// using System;
// using System.Linq;
// using UnityEditor;
// using UnityEditor.UIElements;
// using UnityEngine.UIElements;
// using UnityEngine;
//
// namespace Anvil
// {
//     [CustomPropertyDrawer(typeof(FunctionSystem<,>))]
//     public class FunctionSystemDrawer : PropertyDrawer
//     {
//
//         private SerializedProperty _property;
//         private SerializedProperty _functionsProperty;
//         private VisualElement _container;
//         
//         private VisualElement DrawHeader()
//         {
//             var headerGroup = new VisualElement();
//             headerGroup.style.flexDirection = FlexDirection.Column;
//             headerGroup.style.backgroundColor = new Color(0.0f, 0.0f, 0.0f, 0.15f); // Change to your desired color
//             headerGroup.Add(new Label("Function System"));
//             return headerGroup;
//         }
//
//         private VisualElement DrawBody()
//         {
//             var bodyGroup = new VisualElement();
//
//             // Access the _buffer and _functions fields
//             var functionsProperty = _property.FindPropertyRelative("_functions");
//
//             // for each no null function in the list, draw a custom field for it.
//             for (var i = 0; i < functionsProperty.arraySize; i++)
//             {
//                 var element = functionsProperty.GetArrayElementAtIndex(i);
//                 if (element.managedReferenceValue != null)
//                 {
//                     // Create a horizontal group
//                     var horizontalGroup = new VisualElement
//                     {
//                         style =
//                         {
//                             flexDirection = FlexDirection.Row,
//                             justifyContent = Justify.SpaceBetween,
//                             alignContent = Align.Center
//                         }
//                     };
//
//
//                     // Make the name label a button itself
//                     var label = new Label(element.managedReferenceValue.GetType().Name)
//                     {
//                         style =
//                         {
//                             unityFontStyleAndWeight = FontStyle.Normal,
//                             unityTextAlign = TextAnchor.MiddleCenter
//                         }
//                     };
//                     horizontalGroup.Add(label);
//
//                     var buttonGroup = new VisualElement();
//                     
//                     // make button group horizontal
//                     buttonGroup.style.flexDirection = FlexDirection.Row;
//                     
//                     
//                     
//                     // Create an up, down, remove and inspect button.
//                     var i1 = i;
//                     var upButton = new Button(() =>
//                     {
//                         // Move the function up in the list
//                         var index = functionsProperty.GetArrayElementAtIndex(i1);
//                         functionsProperty.MoveArrayElement(i1, i1 - 1);
//                         _property.serializedObject.ApplyModifiedProperties();
//                         _container.Clear();
//                         _container.Add(CreatePropertyGUI(_property));
//                     });
//
//                     var i2 = i;
//                     var downButton = new Button(() =>
//                     {
//                         // Move the function down in the list
//                         var index = functionsProperty.GetArrayElementAtIndex(i2);
//                         functionsProperty.MoveArrayElement(i2, i2 + 1);
//                         _property.serializedObject.ApplyModifiedProperties();
//                         _container.Clear();
//                         _container.Add(CreatePropertyGUI(_property));
//                     });
//
//                     var i3 = i;
//                     var removeButton = new Button(() =>
//                     {
//                         // Remove the function from the list
//                         var index = functionsProperty.GetArrayElementAtIndex(i3);
//                         functionsProperty.DeleteArrayElementAtIndex(i3);
//                         _property.serializedObject.ApplyModifiedProperties();
//                         _container.Clear();
//                         _container.Add(CreatePropertyGUI(_property));
//                     });
//
//                     upButton.text = "↑";
//                     downButton.text = "↓";
//                     removeButton.text = "╳";
//                     
//                     buttonGroup.Add(upButton);
//                     buttonGroup.Add(downButton);
//                     buttonGroup.Add(removeButton);
//                     horizontalGroup.Add(buttonGroup);
//                     bodyGroup.Add(horizontalGroup);
//
//                     var propertyGroup = new VisualElement();
//                     var manageReferenceValue = element.managedReferenceValue;
//                     
//                     var name = manageReferenceValue.GetType().Name;
//                     var valueProperty = element.FindPropertyRelative(name);
//                     
//                     // Draw the property field for the function
//                     var propertyField = new PropertyField(valueProperty);
//                     propertyField.Bind(_property.serializedObject);
//                     propertyGroup.Add(propertyField);
//                     bodyGroup.Add(propertyGroup);
//                     
//                 }
//             }
//
//             // Add a button to body group that adds a new type of function
//             var button = new Button(() =>
//             {
//                 var menu = new GenericMenu();
//
//                 // Get the type of the generic type parameter
//                 var genericType = fieldInfo.FieldType.GenericTypeArguments[1];
//
//                 // Find all classes in the assembly that implement the generic type (for example IWishDirectionHandler)
//                 // Find all classes in the assembly that implement the generic type
//                 var types = AppDomain.CurrentDomain.GetAssemblies()
//                     .SelectMany(s => s.GetTypes())
//                     .Where(p => genericType.IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract);
//
//                 // Add a menu item for each type
//                 foreach (var type in types)
//                 {
//                     menu.AddItem(new GUIContent(type.Name), false, () =>
//                     {
//                         // Add the function to the list
//                         var index = functionsProperty.arraySize++;
//                         var element = functionsProperty.GetArrayElementAtIndex(index);
//                         element.managedReferenceValue = Activator.CreateInstance(type);
//
//                         _property.serializedObject.ApplyModifiedProperties();
//                         
//                         // force redraw the entire field
//                         _container.Clear();
//                         _container.Add(CreatePropertyGUI(_property));
//                     });
//                 }
//                 
//                 menu.ShowAsContext();
//                 
//             });
//             button.text = "Add Function";
//             bodyGroup.Add(button);
//             return bodyGroup;
//         }
//
//         private void Refresh()
//         {
//             _property.serializedObject.ApplyModifiedProperties();
//             _container.Clear();
//             _container.Add(CreatePropertyGUI(_property));
//         }
//         
//         // Override the CreatePropertyGUI method to provide your custom UI
//         public override VisualElement CreatePropertyGUI(SerializedProperty property)
//         {
//             _property = property;
//             _functionsProperty = property.FindPropertyRelative("_functions");
//             _container = new VisualElement();
//             
//             // Add the header group to the container
//             _container.Add(DrawHeader());
//             _container.Add(DrawBody());
//             
//             // apply all properties
//             property.serializedObject.ApplyModifiedProperties();
//
//             return _container;
//         }
//     }
// }
// #endif