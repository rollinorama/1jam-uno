using UnityEngine;
using System.Collections;
using UnityEditor;
using SG.Unit;

[CustomEditor(typeof(UnitFieldOfView))]
public class FieldOfViewEditor : Editor
{
	void OnSceneGUI()
	{
		UnitFieldOfView fow = (UnitFieldOfView)target;

		//Line Radius
		Handles.color = Color.white;
		Handles.DrawWireArc(fow.transform.position, Vector3.forward, Vector3.up, 360, fow.viewRadius);

		//Line Angle
		Vector3 viewAngleA = fow.DirFromAngle(-fow.viewAngle / 2, false);
		Vector3 viewAngleB = fow.DirFromAngle(fow.viewAngle / 2, false);
		Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.viewRadius);
		Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.viewRadius);

		//Line VisibleTargets
		Handles.color = Color.red;
		foreach (Transform visibleTarget in fow.visibleTargets)
		{
			Handles.DrawLine(fow.transform.position, visibleTarget.position);
		}
	}

}