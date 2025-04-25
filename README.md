# Markup Parser

## Language description

| **Syntax** | **Output** | Description |
| --------------- | --------------- | --------------- |
| ` **text** ` | `<i> text </i>`  | Makes italicised text |
| `'' text ''` | `<b> text </b>`  | Makes bolded text |
| `## text`    | `<h1> text </h1>`| Makes header 1. Broken by newlines |

## How to use

```bash
dotnet run -- [options] <filepath>
```

see `dotnet run -- --help` for a list of options

This program was developed using .NET 8 and is not guaranteed to function
correctly on any other version of the .NET runtime.
For best compatibility and stability, please use .NET 8
when building or running this application.

## TODO list

- Prevent headers from appearing in the middle of a sentence
- consider making text use \<p\> instead of \<br\>
- Decide if bold and italic should be allowed in headers and
if not should they be read as text or error
- Add more header levels
- Add embedded links
- Add (un)ordered lists
- Add tables