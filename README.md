# Markup Parser

A parser for turning a very basic markup language into HTML.

## Language description

| **Syntax** | **Output** | Description |
| --------------- | --------------- | --------------- |
| ` **text** ` | `<i> text </i>`  | Makes italicised text |
| `'' text ''` | `<b> text </b>`  | Makes bolded text |

## How to use

`dotnet run -- [options] <filepath>`
see `dotnet run -- --help` for a list of options

This program was developed using .NET 8 and is not guaranteed to function
correctly on any other version of the .NET runtime.
For best compatibility and stability, please use .NET 8
when building or running this application.