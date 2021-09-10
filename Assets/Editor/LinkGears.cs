using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class GearLinker : EditorWindow {
    [MenuItem("Window/Link gears in level")]
    public static void ShowWindow() {
        EditorWindow.GetWindow(typeof(GearLinker));
    }

    void OnGUI() {
        if (GUILayout.Button("Link")) {
            List<Gear> allGears = new List<Gear>();
            Dictionary<int, Gear> orderedGears = new Dictionary<int, Gear>();
            allGears.AddRange(FindObjectsOfType<Gear>());
            foreach (Gear gear in allGears) {
                int number = (gear.name.Contains("Counter") ? 1 : 0) + (2 * (gear.name.Contains("(") ? int.Parse(Regex.Match(gear.name, @"\d+").Value) : 0));
                orderedGears.Add(number, gear);
            }
            for (int i = 0; i < orderedGears.Count; i++) {
                if (orderedGears[i].bouncables.Count == 0 && i != 0) {
                    orderedGears[i].bouncables.Add(orderedGears[i - 1]);
                }
                //Debug.Log("Gear \"" + orderedGears[i].name + "\" is " + i + "th in the list");
            }
        }
    }
}