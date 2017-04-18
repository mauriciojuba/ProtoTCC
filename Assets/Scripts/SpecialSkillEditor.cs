using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(SpecialSkill))]
public class SpecialSkillEditor : Editor
{
	override public void OnInspectorGUI()
	{
		var myScript = target as SpecialSkill;

		myScript.Rush = EditorGUILayout.Toggle("Rush Skill", myScript.Rush);
		myScript.Magic = EditorGUILayout.Toggle ("Magick Skill", myScript.Magic);

		if (myScript.Rush)
			myScript.Magic = false;
		else
			myScript.Magic = true;

		using (var group = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(!myScript.Rush)))
		{
			if (group.visible == false)
			{
				EditorGUI.indentLevel++;
				EditorGUILayout.PrefixLabel ("Button To Use");
				myScript.ButtonToUse = EditorGUILayout.TextField (myScript.ButtonToUse);
				EditorGUILayout.PrefixLabel("Distance To Rush");
				myScript.RushDistance = EditorGUILayout.FloatField (myScript.RushDistance);
				EditorGUILayout.PrefixLabel("Rush Damage");
				myScript.RushDamage = EditorGUILayout.FloatField (myScript.RushDamage);
				EditorGUILayout.PrefixLabel ("Push Force");
				myScript.RushPushForce = EditorGUILayout.FloatField (myScript.RushPushForce);
				EditorGUILayout.PrefixLabel ("Rush Velocity");
				myScript.RushForce = EditorGUILayout.FloatField (myScript.RushForce);


				EditorGUI.indentLevel--;
			}
		}

		using (var group = new EditorGUILayout.FadeGroupScope(Convert.ToSingle(!myScript.Magic)))
		{
			if (group.visible == false)
			{
				EditorGUI.indentLevel++;
				EditorGUILayout.PrefixLabel ("Button To Use");
				myScript.ButtonToUse = EditorGUILayout.TextField (myScript.ButtonToUse);
				EditorGUILayout.PrefixLabel("Prefab of Magic");
				myScript.MagicPrefab = EditorGUILayout.ObjectField (myScript.MagicPrefab,typeof(GameObject),false);
				myScript.MyStyle = (SpecialSkill.MagicStyle)EditorGUILayout.EnumPopup ("Style of Magic", myScript.MyStyle);
				switch (myScript.MyStyle) {
				case SpecialSkill.MagicStyle.Projectile:
					EditorGUILayout.PrefixLabel ("Qunatity of Projectiles");
					myScript.ProjectileQuant = EditorGUILayout.IntSlider (myScript.ProjectileQuant, 1, 10);
					EditorGUILayout.PrefixLabel ("Projectile Damage");
					myScript.ProjectileDamage = EditorGUILayout.FloatField (myScript.ProjectileDamage);
					EditorGUILayout.PrefixLabel ("Projectile Force");
					myScript.ProjectileForce = EditorGUILayout.FloatField (myScript.ProjectileForce);
					break;
				case SpecialSkill.MagicStyle.AOE:
					EditorGUILayout.PrefixLabel ("Damage");
					myScript.AOEDamage = EditorGUILayout.FloatField (myScript.AOEDamage);
					EditorGUILayout.PrefixLabel ("Distance to AOE Go");
					myScript.AOEDistance = EditorGUILayout.FloatField (myScript.AOEDistance);
					EditorGUILayout.BeginVertical ();
					EditorGUILayout.PrefixLabel ("Force to Pull/Push");
					myScript.AOEForce = EditorGUILayout.FloatField (myScript.AOEForce);
					EditorGUILayout.EndVertical ();
					EditorGUILayout.PrefixLabel ("Pull Enemies");
					myScript.Pull = EditorGUILayout.Toggle (myScript.Pull);
					EditorGUILayout.PrefixLabel ("Push Enemies");
					myScript.Push = EditorGUILayout.Toggle (myScript.Push);
					if (myScript.Pull) {
						myScript.Push = false;
					}
					break;
				}
				EditorGUI.indentLevel--;
			}
		}
	}
}
