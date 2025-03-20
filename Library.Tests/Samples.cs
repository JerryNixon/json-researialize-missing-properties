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
}