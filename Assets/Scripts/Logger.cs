using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

public class Logger
{
    public string path;
    public Logger(string path)
    {
        
        this.path = Path.Combine(path, (DateTime.Now + ".txt"));
        File.Create(path).Close();
        FileStream log = File.OpenWrite(path);
        StreamWriter write = new StreamWriter(log);
        write.WriteLine(path);
    }
}
