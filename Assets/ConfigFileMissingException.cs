using System;
using System.Collections.Generic;
using System.IO;


class ConfigFileMissingException : FileNotFoundException
{
    public ConfigFileMissingException(string Message, string Filename): base(Message, Filename) {}
}

