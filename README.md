# WebSharper Dayjs Binding

This repository provides an F# [WebSharper](https://websharper.com/) binding for the [Day.js](https://day.js.org/) JavaScript library, enabling developers to handle dates and times in a lightweight and powerful way using F# and WebSharper.

## Repository Structure

The repository consists of two main projects:

1. **Binding Project**

   - Contains the F# WebSharper binding for Day.js.

2. **Sample Project**
   - Demonstrates how to use Day.js in a WebSharper application.
   - Includes examples for formatting, adding days, calculating date differences, and using plugins.

## Installation

To use this package in your WebSharper project, install it via NuGet:

```bash
dotnet add package WebSharper.DayJs
```

## Building

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download)
- Node.js and npm (for front-end builds)

### Steps

1. Clone the repository:

```bash
git clone https://github.com/dotnet-websharper/DayJs.git
cd DayJs
```

2. Build the binding project:

```bash
dotnet build WebSharper.DayJs/WebSharper.DayJs.fsproj
```

3. Build and run the sample project:

```bash
cd WebSharper.DayJs.Sample
dotnet build
dotnet run
```

Then open your browser to the provided local URL.

## Example Usage

```fsharp
namespace WebSharper.DayJs.Sample

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Templating
open WebSharper.UI.Notation
open WebSharper.DayJs

[<JavaScript>]
module Client =
    // Bind the template to the HTML document
    type IndexTemplate = Template<"wwwroot/index.html", ClientLoad.FromDocument>

    let result = Var.Create ""

    // Show the current date and time using Dayjs
    let showNow(today: Dayjs) =
        result := $"Now: {today.Format()}"

    // Add 7 days to the current date
    let add7Days(today: Dayjs) =
        let futureDate = today.Add(7, "day").Format("YYYY-MM-DD HH:mm:ss")
        result := $"7 days from now: {futureDate}"

    // Show a formatted version of today's date
    let showFormatted(today: Dayjs) =
        let formatted = today.Format("dddd, MMMM D, YYYY")
        result := $"Today is: {formatted}"

    // Calculate how many days until 2026
    let daysUntil2026(today: Dayjs) =
        let newYear = Dayjs("2026-01-01")
        let daysLeft = newYear.Diff(today, "day")
        result := $"Days until 2026: {daysLeft}"

    [<SPAEntryPoint>]
    let Main () =
        let today = Dayjs()

        IndexTemplate.Main()
            .ShowNow(fun _ -> showNow(today))
            .Add7Days(fun _ -> add7Days(today))
            .ShowFormatted(fun _ -> showFormatted(today))
            .DaysUntil2026(fun _ -> daysUntil2026(today))
            .ShowUTCNow(fun _ ->
                Dayjs.Extend(Plugins.Utc) |> ignore
                let utcNow = Dayjs().Utc()
                result := $"Current UTC Time: {utcNow} UTC"
            )
            .Result(result.V)
            .Doc()
        |> Doc.RunById "main"
```

## Features

- Access to core Day.js functionalities: formatting, manipulating, and comparing dates.
- Plugin support (e.g., UTC, timezone).
- Lightweight and immutable API.
- Fully integrated into WebSharper SPA model.
