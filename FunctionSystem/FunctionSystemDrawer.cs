// #if UNITY_EDITOR
// using System.Linq;
// using UnityEditor;
// using UnityEditor.UIElements;
// using UnityEngine;
// using UnityEngine.UIElements;
//
// namespace Anvil
// {
//     [CustomPropertyDrawer(typeof(FunctionSystem<,>))]
//     public class FunctionSystemDrawer : PropertyDrawer
//     {
//         public override VisualElement CreatePropertyGUI(SerializedProperty property)
//         {
//             var container = new VisualElement();
//             SerializedProperty functionsProperty = property.FindPropertyRelative("_functions");
//
//             for (int i = 0; i < functionsProperty.arraySize; i++)
//             {
//                 container.Add(DrawFunctionElement(functionsProperty, i));
//             }
//             
//             // create a button to add a new type to the list
//             var button = new Button(() =>
//             {
//                 // create a menu and add items to it
//                 var menu = new GenericMenu();
//                 var types = TypeCache.GetTypesDerivedFrom(typeof(IFunction<>));
//                 foreach (var type in types)
//                 {
//                     menu.AddItem(new GUIContent(type.Name), false, () =>
//                     {
//                         AddElement(functionsProperty.arraySize, functionsProperty);
//                         SerializedProperty arrayElement = functionsProperty.GetArrayElementAtIndex(functionsProperty.arraySize - 1);
//                         arrayElement.managedReferenceValue = System.Activator.CreateInstance(type);
//                         property.serializedObject.ApplyModifiedProperties();
//
//                     });
//                 }
//                 // display the menu
//                 menu.ShowAsContext();
//             })
//             {
//                 text = "Add Function"
//             };
//             
//             container.Add(button);
//
//             return container;
//         }
//
//         private VisualElement DrawFunctionElement(SerializedProperty functionsProperty, int i)
//         {
//             var horizontalContainer = new VisualElement();
//             horizontalContainer.style.flexDirection = FlexDirection.Row;
//             
//             SerializedProperty arrayElement = functionsProperty.GetArrayElementAtIndex(i);
//             var serializedField = new PropertyField(arrayElement);
//             
//             var type = arrayElement.managedReferenceFullTypename.Split(' ').Last().Split('.').Last();
//             serializedField.label = type;
//             
//             horizontalContainer.Add(serializedField);
//             horizontalContainer.Add(DrawListButtons(functionsProperty, i));
//             
//             return horizontalContainer;
//         }
//
//         private VisualElement DrawListButtons(SerializedProperty functionsProperty, int i)
//         {
//             var buttonGroup = new VisualElement();
//             buttonGroup.style.flexDirection = FlexDirection.Row;
//             
//             // create move up button
//             var moveUpButton = new Button(() =>
//             {
//                 var index = functionsProperty.GetArrayElementAtIndex(i);
//                 functionsProperty.MoveArrayElement(i, i - 1);
//                 functionsProperty.serializedObject.ApplyModifiedProperties();
//             })
//             {
//                 text = "↑"
//             };
//             
//             // create move down button
//             var moveDownButton = new Button(() =>
//             {
//                 var index = functionsProperty.GetArrayElementAtIndex(i);
//                 functionsProperty.MoveArrayElement(i, i + 1);
//                 functionsProperty.serializedObject.ApplyModifiedProperties();
//             })
//             {
//                 text = "↓"
//             };
//             
//             // create remove button
//             var removeButton = new Button(() =>
//             {
//                 var index = functionsProperty.GetArrayElementAtIndex(i);
//                 functionsProperty.DeleteArrayElementAtIndex(i);
//                 functionsProperty.serializedObject.ApplyModifiedProperties();
//             })
//             {
//                 text = "-"
//             };
//             
//             if(i==0)
//                 moveUpButton.SetEnabled(false);
//             if (i == functionsProperty.arraySize - 1)
//                 moveDownButton.SetEnabled(false);
//                 
//             buttonGroup.Add(moveUpButton);
//             buttonGroup.Add(moveDownButton);
//             buttonGroup.Add(removeButton);
//             
//             return buttonGroup;
//         }
//
//         private void AddElement(int index, SerializedProperty functionsProperty)
//         {
//             functionsProperty.InsertArrayElementAtIndex(index);
//             // You can add additional logic here to initialize the new element as needed
//         }
//     }
// }
// #endif