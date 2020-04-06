///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER
/// Date : 01/03/2020 14:21
///-----------------------------------------------------------------

using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using Com.Isartdigital.Common.Audio;
using System.Collections;

namespace Com.Isartdigital.Common {

	[CustomEditor(typeof(AudioList))]
	public class AudioListEditor : Editor {

		private const int EDITOR_SPACE = 10;
		private const string GENERATE_BTN = "Generate Tags";

		private ReorderableList list;

		private const float LIST_PADDING = 10;
		private const float LIST_PADDING_PROPERTY = 4;
		private const string LIST_HEADER = "Sound List";

		private void OnEnable()
		{
			if (target == null) return;

			list = new ReorderableList(serializedObject, serializedObject.FindProperty("sounds"), true, true, true, true);

			list.drawHeaderCallback = (Rect rect) => {

				rect.x += rect.width / 2 - (((float)LIST_HEADER.Length / 2f) * 5.5f);
				rect.y += 2;

				EditorGUI.LabelField(rect, LIST_HEADER);
			};

			float propertyHeight = LIST_PADDING_PROPERTY + EditorGUIUtility.singleLineHeight;
			float soundHeight = LIST_PADDING + propertyHeight * 8;


			list.drawElementCallback =
			(Rect rect, int index, bool isActive, bool isFocused) => {

				SerializedProperty sound = list.serializedProperty.GetArrayElementAtIndex(index);

				rect.y += 4;
				rect.x += LIST_PADDING;
				rect.width -= LIST_PADDING * 2.5f;
				rect.height = EditorGUIUtility.singleLineHeight;

				bool showInfo = EditorGUI.Foldout(rect, sound.FindPropertyRelative("showInfoEditor").boolValue, 
								sound.FindPropertyRelative("tag").stringValue);

				sound.FindPropertyRelative("showInfoEditor").boolValue = showInfo;

				if (!showInfo) return;

				IEnumerator propertyEnumerator = sound.GetEnumerator();

				while (propertyEnumerator.MoveNext())
				{
					rect.y += propertyHeight;
					EditorGUI.PropertyField(rect, (SerializedProperty)propertyEnumerator.Current);
				}
			};


			list.elementHeightCallback = (int index) =>
			{
				SerializedProperty sound = list.serializedProperty.GetArrayElementAtIndex(index);

				return sound.FindPropertyRelative("showInfoEditor").boolValue ? soundHeight : propertyHeight;
			};

			//ReorderableList.
			/*
			list.onAddCallback = (ReorderableList list) =>
			{

			};
			*/
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			AudioList audioList = (AudioList)target;

			GUILayout.Space(EDITOR_SPACE);

			SerializedProperty serializedProperty = serializedObject.FindProperty("globalMixer");
			EditorGUILayout.PropertyField(serializedProperty);

			GUILayout.Space(EDITOR_SPACE);

			list.DoLayoutList();

			GUILayout.Space(EDITOR_SPACE);

			if (GUILayout.Button(GENERATE_BTN))
			{
				audioList.GenerateTags();
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}