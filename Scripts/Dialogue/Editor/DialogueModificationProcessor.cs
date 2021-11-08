using System.Collections;
using System.Collections.Generic;
using System.IO;
using RPG.Dialogue;
using UnityEditor;
using UnityEngine;

public class DialogueModificationProcessor : UnityEditor.AssetModificationProcessor
{
    private static AssetMoveResult OnWillMoveAsset(string sourcePath, string destinationPath)
    {
        var dialogue = AssetDatabase.LoadMainAssetAtPath(sourcePath) as Dialogue;
        if (dialogue == null)
        {
            return AssetMoveResult.DidNotMove;
        }
        if (MovingDirectory(sourcePath, destinationPath))
        {
            return AssetMoveResult.DidNotMove;
        }
        dialogue.name = Path.GetFileNameWithoutExtension(destinationPath);
        return AssetMoveResult.DidNotMove;
    }
    private static bool MovingDirectory(string sourcePath, string destinationPath)
    {
        return Path.GetDirectoryName(sourcePath) != Path.GetDirectoryName(destinationPath);
    }
}