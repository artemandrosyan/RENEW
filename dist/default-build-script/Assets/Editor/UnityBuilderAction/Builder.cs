using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor;
//using UnityEditor;
//using UnityEditor.Build.Reporting;
using UnityEngine;

using System.Linq;
using System.Reflection;
using UnityBuilderAction.Input;
using UnityBuilderAction.Reporting;
using UnityBuilderAction.Versioning;
using UnityEditor;
using UnityEditor.Build.Reporting;


namespace UnityBuilderAction{
public class Builder {
  static string[] SCENES = FindEnabledEditorScenes();

  static string TARGET_DIR = "./";


  static void PerformIOSBuild() {
    GenericBuild(SCENES, TARGET_DIR + "/ios/", UnityEditor.BuildTarget.iOS, UnityEditor.BuildOptions.None);
  }


  static void PerformAndroidBuild() {
    // Gather values from args
    var options = ArgumentsParser.GetValidatedOptions();
    UnityEditor.EditorUserBuildSettings.exportAsGoogleAndroidProject = true;
       // Get all buildOptions from options
      BuildOptions buildOptions = BuildOptions.None;
      foreach (string buildOptionString in Enum.GetNames(typeof(BuildOptions))) {
        if (options.ContainsKey(buildOptionString)) {
          BuildOptions buildOptionEnum = (BuildOptions) Enum.Parse(typeof(BuildOptions), buildOptionString);
          buildOptions |= buildOptionEnum;
        }
      }
     // Set version for this build
      VersionApplicator.SetVersion(options["buildVersion"]);
      VersionApplicator.SetAndroidVersionCode(options["androidVersionCode"]);
    
    GenericBuild(SCENES, "android/", UnityEditor.BuildTarget.Android, UnityEditor.BuildOptions.None);
   
  }

  private static string[] FindEnabledEditorScenes() {
    List<string> EditorScenes = new List<string>();
    foreach (UnityEditor.EditorBuildSettingsScene scene in UnityEditor.EditorBuildSettings.scenes) {
      if (!scene.enabled)
        continue;
      EditorScenes.Add(scene.path);
    }
    return EditorScenes.ToArray();
  }

  static void GenericBuild(string[] scenes, string target_dir, UnityEditor.BuildTarget build_target, UnityEditor.BuildOptions build_options) {
    UnityEditor.EditorUserBuildSettings.SwitchActiveBuildTarget(build_target);
    if (TryGetEnv(VERSION_CODE, out string value))
        {
            if (int.TryParse(value, out int version))
            {
                PlayerSettings.Android.bundleVersionCode = version;
            }
    }


    // Perform build
    UnityEditor.Build.Reporting.BuildReport buildReport = UnityEditor.BuildPipeline.BuildPlayer(scenes, target_dir, build_target, build_options);
    UnityEditor.Build.Reporting.BuildSummary summary = buildReport.summary;
     
    Console.WriteLine(summary.outputPath);
    Debug.Log(summary.outputPath);

    UnityEditor.Build.Reporting.BuildResult result = summary.result;
    Console.WriteLine(result.ToString());
     
  }
}
}
