﻿namespace NameSorterCore.Tests.Helpers;

public class ConsoleOutput : IDisposable
{
    private StringWriter stringWriter;
    private TextWriter originalOutput;

    public ConsoleOutput()
    {
        stringWriter = new StringWriter();
        originalOutput = Console.Out;
        Console.SetOut(stringWriter);
    }

    public string GetOutput()
    {
        return stringWriter.ToString();
    }

    public void Dispose()
    {
        Console.SetOut(originalOutput);
        stringWriter.Dispose();
    }
}