using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;

public static class TabletManifestoWriter
{
    public const string AndroidManifestPath = "Plugins/Android/AndroidManifest.xml";
    public static string AndroidManifestFullPath => Path.Combine(Application.dataPath, AndroidManifestPath);
    public const string TabletManifestFileName = "TabletXMLManifest_UniqueName123456789";

    public static string FindTabletManifestFile()
    {
        var tabletManifestFiles = AssetDatabase.FindAssets(TabletManifestFileName);
        foreach (var tabPath in tabletManifestFiles)
        {
            var path = AssetDatabase.GUIDToAssetPath(tabPath);
            // there really should only be one...
            return path;
        }

        return "";
    }

    public static void WriteTabletCamStuffToTheManifest(out string oldManifest)
    {
        // read raw manifest
        var sr = new StreamReader(AndroidManifestFullPath);
        oldManifest = sr.ReadToEnd();
        sr.Close();

        // replace text in the raw manifest
        var newManifest = oldManifest;
        {
            var tabletManifestPath = FindTabletManifestFile();
            var tsr = new StreamReader(tabletManifestPath);
            var tabletManifest = tsr.ReadToEnd();
            tsr.Close();

            // replace with the unique string of the build.
            tabletManifest = tabletManifest.Replace("UNIQUEUNIQUEUNIQUEUNIQUEUNIQUEUNIQUE", GetBundleId());

            newManifest = tabletManifest;


            // this shit doesn't work.
            //var androidManifestDoc = new XmlDocument();
            //androidManifestDoc.LoadXml(oldManifest);
            // // using this as a guide: https://stackoverflow.com/a/642314
            // XmlDocument tabletDoc = new XmlDocument();
            // tabletDoc.Load(tabletManifestPath);
            //
            // // put some text in the manifest node of the androidManifest
            // var manifestNode = androidManifestDoc.DocumentElement.SelectSingleNode("/manifest");
            // var insideManifestNode = tabletDoc.DocumentElement.SelectSingleNode("/manifest/put-inside-manifest");
            // manifestNode.InnerText = manifestNode.InnerText + insideManifestNode.InnerText;
            //
            // Debug.Log("Found Mani: " + manifestNode.InnerText);
            //
            // // put some text in the application node
            // var applicationNode = androidManifestDoc.DocumentElement.SelectSingleNode("/manifest/application");
            // var insideApplicationNode =
            //     tabletDoc.DocumentElement.SelectSingleNode("/manifest/application/put-inside-application");
            // // first we have to replace the unique parameter thing with the unique parameter thing.
            // // and also in that script from the tablet cam....
            // Debug.Log("don't forget copying the unique name thing!!!");
            //
            // Debug.Log("Found Appl pre: " + applicationNode.InnerText);
            // applicationNode.InnerText = applicationNode.InnerText + insideApplicationNode.InnerText;
            // Debug.Log("Found Appl post: " + applicationNode.InnerText);
            //
            //
            //newManifest = androidManifestDoc.ToString();
        }

        Debug.Log("Writing new mani:\n" + newManifest);
        
        // consider renaming the old file...???
        // write the new manifest
        var sw = new StreamWriter(AndroidManifestFullPath, false);
        sw.Write(newManifest);
        sw.Close();
    }

    private static string GetBundleId()
    {
        return Application.identifier;
    }

    public static void RevertManifest(string oldMan)
    {
        if (string.IsNullOrWhiteSpace(oldMan))
        {
            Debug.LogError("trying to revert to nonexisting manifest!!! aborting");
            return;
        }
        else
        {
            Debug.Log("Reverting mani:\n" + oldMan);
        }
        
        
        // write the new manifest
        var sw = new StreamWriter(AndroidManifestFullPath, false);
        sw.Write(oldMan);
        sw.Close();
    }
}