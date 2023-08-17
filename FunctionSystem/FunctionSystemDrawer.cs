#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Anvil
{
    [CustomPropertyDrawer(typeof(FunctionSystem<,>))]
    public class FunctionSystemDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var container = new VisualElement();
            SerializedProperty functionsProperty = property.FindPropertyRelative("_functions");

            for (int i = 0; i < functionsProperty.arraySize; i++)
            {
                SerializedProperty arrayElement = functionsProperty.GetArrayElementAtIndex(i);
                
                var serializedField = new PropertyField(arrayElement);
                container.Add(serializedField);
            }
            
            // create a button to add a new type to the list
            var button = new Button(() =>
            {
                // create a menu and add items to it
                var menu = new GenericMenu();
                var types = TypeCache.GetTypesDerivedFrom(typeof(IFunction<>));
                foreach (var type in types)
                {
                    menu.AddItem(new GUIContent(type.Name), false, () =>
                    {
                        // add a new element to the list
                        AddElement(functionsProperty.arraySize, functionsProperty);
                        // set the type of the new element
                        SerializedProperty arrayElement = functionsProperty.GetArrayElementAtIndex(functionsProperty.arraySize - 1);
                        arrayElement.managedReferenceValue = System.Activator.CreateInstance(type);
                        property.serializedObject.ApplyModifiedProperties();
                    });
                }
                // display the menu
                menu.ShowAsContext();
            })
            {
                text = "Add Function"
            };
            
            container.Add(button);

            return container;
        }

        private void AddElement(int index, SerializedProperty functionsProperty)
        {
            functionsProperty.InsertArrayElementAtIndex(index);
            // You can add additional logic here to initialize the new element as needed
        }
    }
}
#endif