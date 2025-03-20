# json-researialize-missing-properties

This is a sample library created to demonstrate how to read JSON files with both required and optional properties. The library ensures that the JSON data can be deserialized into objects, manipulated if needed, and then serialized back into identical JSON format.

## Features

- Handles JSON files with required and optional properties, ensuring compatibility with various JSON structures.
- Ensures deserialized objects can be re-serialized into the original JSON structure, maintaining data integrity during processing.
- Provides a framework for working with JSON data, offering a consistent and reliable approach to JSON handling.

## Tests

The library includes detailed tests to validate its functionality. These tests focus on the reader and writer components, ensuring proper deserialization and serialization of JSON data.

### Test Samples

| Sample   | Contains Required Properties | Contains Optional Properties | Valid JSON | Nested Structure | Description                                                                 |
|----------|-------------------------------|-------------------------------|------------|------------------|-----------------------------------------------------------------------------|
| Sample1  | Yes                           | Yes                           | Yes        | No               | Tests standard deserialization and serialization with required and optional properties. |
| Sample2  | Yes                           | Yes                           | Yes        | No               | Similar to Sample1, with slight variations in optional property values.    |
| Sample3  | No                            | No                            | No         | No               | Invalid JSON used to test error handling for malformed input.              |
| Sample5  | Yes                           | Yes                           | Yes        | Yes              | Tests handling of nested structures with both required and optional properties. |
| Sample6  | Yes                           | Yes                           | Yes        | Yes              | Similar to Sample5, with different nested property values.                 |
| Sample7  | No                            | No                            | No         | No               | Another invalid JSON sample to validate error handling.                    |

The test files in the repository provide concrete examples of how the library handles these scenarios, ensuring data integrity and consistency.