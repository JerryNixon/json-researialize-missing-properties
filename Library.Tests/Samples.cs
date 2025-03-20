using System.IO;

namespace Library.Tests;

public static class Samples
{
    // Sample 1: Only required property with value
    public static string Sample1 => """
    {
        "required-property": {
            "value": false
        }
    }
    """;

    // Sample 2: Required property with value and optional property
    public static string Sample2 => """
    {
        "required-property": {
            "value": false
        },
        "optional-property": true
    }
    """;

    // Sample 3: Missing required property (special case)
    public static string Sample3 => """
    {
        "optional-property": true
    }
    """;

    // Sample 4: Required property with value and optional property (duplicate of Sample2)
    public static string Sample4 => """
    {
        "required-property": {
            "value": false
        },
        "optional-property": true
    }
    """;

    // Sample 5: All properties including nested property
    public static string Sample5 => """
    {
        "required-property": {
            "value": true,
            "nested-property": 42
        },
        "optional-property": false
    }
    """;

    // Sample 6: Required property with nested property, no optional property
    public static string Sample6 => """
    {
        "required-property": {
            "value": true,
            "nested-property": 200
        }
    }
    """;

    // Sample 7: Missing required value in required-property object
    public static string Sample7 => """
    {
        "required-property": {
            "nested-property": 300
        }
    }
    """;

    // Dictionary of sample names to content
    public static Dictionary<string, string> AllSamples => new()
    {
        ["sample1.json"] = Sample1,
        ["sample2.json"] = Sample2,
        ["sample3.json"] = Sample3,
        ["sample4.json"] = Sample4,
        ["sample5.json"] = Sample5,
        ["sample6.json"] = Sample6,
        ["sample7.json"] = Sample7
    };

    // Create sample files for testing
    public static void CreateSampleFiles(string directory)
    {
        Directory.CreateDirectory(directory);
        
        foreach (var sample in AllSamples)
        {
            File.WriteAllText(Path.Combine(directory, sample.Key), sample.Value);
        }
    }
}