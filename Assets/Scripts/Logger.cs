using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

public class Logger
{
    public static string logDir = @"\myLogs\";
    //public static Logger debug = new("debug");
    public static Logger ui = new("ui");
    private int funcNum = 0;
    public string path;
    public string name;
    //public StreamWriter writter;
    public Logger(string name)
    {
        if (!Directory.Exists(Logger.logDir))
            Directory.CreateDirectory(Logger.logDir);
        /*this.path = path + DateTime.Now.ToString().Replace(':', '.') + ".txt";
        File.Create(path).Close();
        FileStream log = File.OpenWrite(path);
        StreamWriter write = new StreamWriter(log);
        write.WriteLine(path);*/
        this.name = name;
        var timeText = DateTime.Now.ToString().Replace(':', '.');
        path = logDir + timeText + name + ".txt";
        File.CreateText(path).Close();
        Application.logMessageReceived += logException;
    }
    ~Logger()
    {
    }
    public void log(string text)
    {
        using(StreamWriter sw = new StreamWriter(path, true))
            sw.WriteLine(new String(' ', funcNum) + text);
    }
    public void startFunc(string name, string parameters = "")
    {
        funcNum++;
        log(name + '(' + parameters + ')');
    }
    public T endFunc<T>(string name, T retme)
    {
        log(name + "() = " + retme.ToString());
        funcNum--;
        return retme;
    }
    public void endFunc(string name)
    {
        log(name + "() is finished");
        funcNum--;
    }
    public void logException(string exception, string stackRace, LogType type)
    {
        if (type == LogType.Exception)
        {
            log('{' + exception);
            log(stackRace + '}');
            funcNum = 0;
        }
    }
}
