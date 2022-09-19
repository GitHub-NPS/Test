using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif

#if UNITY_IOS_SIGNIN
using AppleAuth.Editor;
#endif

public class PostProcessBuilder : MonoBehaviour
{
#if UNITY_IOS && UNITY_IOS_SIGNIN
    [PostProcessBuild(1)]
    public static void OnPostprocessBuild(BuildTarget buildTarget, string path)
    {
        var appId = "ca-app-pub-9819920607806935~8806080847";

#if IOS_FREE_PRODUCTION
        appId = "ca-app-pub-9819920607806935~8806080847";
#endif

        var plistPath = Path.Combine(path, "Info.plist");
        var plist = new PlistDocument();

        plist.ReadFromFile(plistPath);
        plist.root.SetString("GADApplicationIdentifier", appId);
        File.WriteAllText(plistPath, plist.WriteToString());

        if (buildTarget != BuildTarget.iOS) return;

        var projectPath = PBXProject.GetPBXProjectPath(path);

#if UNITY_2019_3_OR_NEWER
        var project = new PBXProject();
        project.ReadFromString(System.IO.File.ReadAllText(projectPath));

        // project.AddFrameworkToProject(project.GetUnityMainTargetGuid(),"UnityFramework.framework", false);

        var manager = new ProjectCapabilityManager(projectPath, "Entitlements.entitlements", null, project.GetUnityMainTargetGuid());
        manager.AddSignInWithAppleWithCompatibility(project.GetUnityFrameworkTargetGuid());
        manager.WriteToFile();

        // project.WriteToFile(projectPath);
#else
        var manager = new ProjectCapabilityManager(projectPath, "Entitlements.entitlements", PBXProject.GetUnityTargetName());
        manager.AddSignInWithAppleWithCompatibility();
        manager.WriteToFile();
#endif
    }
#endif
}
