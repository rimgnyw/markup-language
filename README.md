# Markup Parser

A C# program for turning a basic custom markup language into HTML.

## Language description

| **Syntax** | **Output** | Description |
| --------------- | --------------- | --------------- |
| ` **text** ` | `<i> text </i>`  | Makes italicised text |
| `'' text ''` | `<b> text </b>`  | Makes bolded text |
| `## text`    | `<h1> text </h1>`| Makes heading 1. Broken by newlines |

## How to use

```bash
dotnet run -- [options] <filepath>
```

see `dotnet run -- --help` for a list of options

This program was developed using .NET 8 and is not guaranteed to function
correctly on any other version of the .NET runtime.
For best compatibility and stability, please use .NET 8
when building or running this application.

## TODO

- Add more header levels
- Add embedded links
- Add (un)ordered lists
- Add tables