﻿using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;


public static class DesktopTxt 
{
	public static string Write(string name, string[] lines, string ext = ".txt", bool open = false)
	{
		string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop).Replace("\\", "/") + "/" + name + ext;
		File.WriteAllLines(path, lines);
		if(open)
			Process.Start("notepad.exe", path);
		return path;
	}


	public static string[] Read(string name, string ext = ".txt")
	{
		string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop).Replace("\\", "/") + "/" + name + ext;
		return File.Exists(path) ? File.ReadAllLines(path) : new string[0];
	}
}


public static class DocumentsTxt 
{
	public static string Write(string name, string[] lines, string ext = ".txt", bool open = false)
	{
		string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).Replace("\\", "/") + "/" + name + ext;
		File.WriteAllLines(path, lines);
		if(open)
			Process.Start("notepad.exe", path);
		return path;
	}


	public static string[] Read(string name, string ext = ".txt")
	{
		string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).Replace("\\", "/") + "/" + name + ext;
		return File.Exists(path) ? File.ReadAllLines(path) : new string[0];
	}
}


public static class ProjectTxt 
{
	public static string Write(string name, string[] lines, string ext = ".txt", bool open = false)
	{
		string path = Application.dataPath + "/" + name + ext;
		File.WriteAllLines(path, lines);
		if(open)
			Process.Start("notepad.exe", path);
		return path;
	}


	public static string[] Read(string name, string ext = ".txt")
	{
		string path = Application.dataPath + "/" + name + ext;
		return File.Exists(path) ? File.ReadAllLines(path) : new string[0];
	}
}


public static class ResourceTxt 
{
	public static string Write(string name, string[] lines, string ext = ".txt", bool open = false)
	{
		string path = Application.dataPath + "/Resources/" + name + ext;
		File.WriteAllLines(path, lines);
		if(open)
			Process.Start("notepad.exe", path);
		return path;
	}


	public static string[] Read(string name)
	{
		return Resources.Load<TextAsset>(name).text.Split('\n');
	}
}


public static class DesktopBytes 
{
	public static void Write(string name, byte[] bytes)
	{
		string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop).Replace("\\", "/") + "/" + name + ".bytes";
		File.WriteAllBytes(path, bytes);
	}


	public static byte[] Read(string name)
	{
		string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop).Replace("\\", "/") + "/" + name + ".bytes";
		return File.Exists(path) ? File.ReadAllBytes(path) : new byte[0];
	}
}

public static class DocumentsBytes 
{
	public static void Write(string name, byte[] bytes)
	{
		string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).Replace("\\", "/") + "/" + name + ".bytes";
		File.WriteAllBytes(path, bytes);
	}


	public static byte[] Read(string name)
	{
		string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).Replace("\\", "/") + "/" + name + ".bytes";
		return File.Exists(path) ? File.ReadAllBytes(path) : new byte[0];
	}
}