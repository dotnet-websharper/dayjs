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
    // The templates are loaded from the DOM, so you just can edit index.html
    // and refresh your browser, no need to recompile unless you add or remove holes.
    type IndexTemplate = Template<"wwwroot/index.html", ClientLoad.FromDocument>

    let result = Var.Create ""

    let showNow(today: Dayjs) =
        result := $"Now: {today.Format()}"

    let add7Days(today: Dayjs) = 
        let futureDate = today.Add(7, "day").Format("YYYY-MM-DD HH:mm:ss")
        result := $"7 days from now: {futureDate}"

    let showFormatted(today: Dayjs) =
        let formatted = today.Format("dddd, MMMM D, YYYY")
        result := $"Today is: {formatted}"

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
