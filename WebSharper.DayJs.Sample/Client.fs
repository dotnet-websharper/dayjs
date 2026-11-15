namespace WebSharper.DayJs.Sample

open WebSharper
open WebSharper.JavaScript
open WebSharper.UI
open WebSharper.UI.Client
open WebSharper.UI.Templating
open WebSharper.DayJs

[<JavaScript>]
module Client =

    // The template is the whole document, loaded from the DOM, so editing
    // index.html and refreshing the browser is enough.
    type IndexTemplate = Template<"index.html", ClientLoad.FromDocument>

    // ---------------------------------------------------------------
    // Helpers
    // ---------------------------------------------------------------

    let private join (parts: string[]) =
        let mutable s = ""
        for i in 0 .. parts.Length - 1 do
            s <- s + (if i = 0 then "" else ", ") + parts.[i]
        s

    let private num (x: float) =
        string (System.Math.Round(x * 1000.) / 1000.)

    let private flag (b: bool) = if b then "true" else "false"

    /// Write a titled label/value report into a `Var` bound to a `<pre>` hole.
    let private report (v: Var<string>) (title: string) (rows: (string * string) list) =
        let body =
            rows
            |> List.fold (fun acc (label, value) -> acc + label + ": " + value + "\n") ""
        v.Value <- title + "\n\n" + body

    // ---------------------------------------------------------------
    // State bound into the template
    // ---------------------------------------------------------------

    // Theme: light is the default; the choice is persisted in localStorage.
    // The storage key is shared with the bootstrap script in index.html.
    let themeStorageKey = "websharper-dayjs-theme"
    let theme = Var.Create "light"
    let themeLabel = Var.Create "🌙 Dark mode"

    let notice =
        Var.Create "Pick a section below — every call runs in your browser against the bundled Day.js 1.11.x."

    let parseReport = Var.Create "…"
    let manipulateReport = Var.Create "…"
    let queryReport = Var.Create "…"
    let diffReport = Var.Create "…"
    let zoneReport = Var.Create "…"
    let localeReport = Var.Create "…"
    let durationReport = Var.Create "…"
    let objectReport = Var.Create "…"
    let pluginReport = Var.Create "…"

    let applyTheme () =
        JS.Document.DocumentElement.SetAttribute("data-theme", theme.Value)
        themeLabel.Value <-
            if theme.Value = "dark" then "☀️ Light mode" else "🌙 Dark mode"

    let toggleTheme () =
        theme.Value <- (if theme.Value = "dark" then "light" else "dark")
        try JS.Window.LocalStorage.SetItem(themeStorageKey, theme.Value) with _ -> ()
        applyTheme ()

    // ---------------------------------------------------------------
    // A fixed reference date, so the reports are reproducible
    // ---------------------------------------------------------------

    let reference = Dayjs("2026-02-14T09:30:00")
    let earlier = Dayjs("2025-12-24T18:00:00")
    let later = Dayjs("2026-06-01T00:00:00")

    // ---------------------------------------------------------------
    // Parse & format
    // ---------------------------------------------------------------

    let parseNow () =
        let now = Dayjs()
        report parseReport "Now"
            [ "Format()", now.Format()
              "ToString()", now.ToString()
              "ToJSON()", now.ToJSON()
              "ToISOString()", now.ToISOString()
              "ValueOf() (ms)", string (now.ValueOf())
              "Unix() (s)", string (now.Unix())
              "ToDate().GetFullYear()", string (now.ToDate().GetFullYear())
              "IsValid()", flag (now.IsValid())
              "Clone() is a new object", flag (now.Clone().ValueOf() = now.ValueOf()) ]

    let parseCustom () =
        let parsedFormat = Dayjs("14/02/2026 09:30", "DD/MM/YYYY HH:mm")
        let strict = Dayjs("2026-02-14", "YYYY-MM-DD", "en", true)
        let fromArray = Dayjs([| 2026; 1; 14; 9; 30 |])
        let fromObject = Dayjs(ObjectSupport(Year = 2026, Month = 1, Date = 14))
        let fromUnix = Dayjs.Unix(1771000200.)
        let fromDate = Dayjs(reference.ToDate())
        let withFormatObject =
            Dayjs("2026-02-14T09:30:00", FormatObject(Format = "YYYY-MM-DD HH:mm", Utc = true))
        report parseReport "Parsing"
            [ "customParseFormat \"14/02/2026 09:30\"", parsedFormat.Format("YYYY-MM-DD HH:mm")
              "strict \"2026-02-14\"", strict.Format("YYYY-MM-DD") + " (strict)"
              "arraySupport [2026,1,14,9,30]", fromArray.Format("YYYY-MM-DD HH:mm")
              "objectSupport { year, month, date }", fromObject.Format("YYYY-MM-DD")
              "Unix(1771000200)", fromUnix.Format("YYYY-MM-DD HH:mm")
              "from a JS Date", fromDate.Format("YYYY-MM-DD HH:mm")
              "format object (utc)", withFormatObject.Format("YYYY-MM-DD HH:mm") + " UTC" ]

    let formatPatterns () =
        report parseReport "Format patterns"
            [ "default", reference.Format()
              "YYYY-MM-DD HH:mm:ss", reference.Format("YYYY-MM-DD HH:mm:ss")
              "dddd, MMMM D, YYYY", reference.Format("dddd, MMMM D, YYYY")
              "advancedFormat Do MMMM", reference.Format("Do MMMM YYYY")
              "localizedFormat L", reference.Format("L")
              "localizedFormat LLLL", reference.Format("LLLL")
              "escaped [YYYY] MM", reference.Format("[YYYY] MM")
              "diff default (ISO 8601)", reference.Format() ]

    // ---------------------------------------------------------------
    // Manipulate
    // ---------------------------------------------------------------

    let addSubtract () =
        report manipulateReport "Immutable arithmetic"
            [ "reference", reference.Format("YYYY-MM-DD HH:mm:ss")
              "+7 days", reference.Add(7., "day").Format("YYYY-MM-DD HH:mm:ss")
              "-2 months", reference.Subtract(2., "month").Format("YYYY-MM-DD HH:mm:ss")
              "+1 week", reference.Add(1., "week").Format("YYYY-MM-DD HH:mm:ss")
              "+90 minutes", reference.Add(90., "minute").Format("YYYY-MM-DD HH:mm:ss")
              "+30 seconds", reference.Add(30., "second").Format("YYYY-MM-DD HH:mm:ss")
              "+1 year", reference.Add(1., "year").Format("YYYY-MM-DD HH:mm:ss")
              "reference is unchanged", flag (reference.Format("YYYY-MM-DD HH:mm:ss") = "2026-02-14 09:30:00") ]

    let startEnd () =
        report manipulateReport "startOf / endOf"
            [ "startOf month", reference.StartOf("month").Format("YYYY-MM-DD HH:mm:ss")
              "endOf month", reference.EndOf("month").Format("YYYY-MM-DD HH:mm:ss")
              "startOf year", reference.StartOf("year").Format("YYYY-MM-DD HH:mm:ss")
              "endOf week", reference.EndOf("week").Format("YYYY-MM-DD HH:mm:ss")
              "startOf isoWeek", reference.StartOf("isoWeek").Format("YYYY-MM-DD HH:mm:ss")
              "endOf quarter", reference.EndOf("quarter").Format("YYYY-MM-DD HH:mm:ss")
              "startOf hour", reference.StartOf("hour").Format("YYYY-MM-DD HH:mm:ss") ]

    let setGet () =
        let shifted =
            reference
                .Set("hour", 9)
                .Set("date", 20)
                .Year(2030)
                .Month(11)
                .Date(25)
                .Hour(18)
                .Minute(45)
                .Second(30)
                .Millisecond(500)
        report manipulateReport "set/get and the unit setters"
            [ "Get(\"hour\")", string (reference.Get("hour"))
              "Set(\"hour\", 9)", reference.Set("hour", 9).Format("HH:mm")
              "Get(\"month\") (0-based)", string (reference.Get("month"))
              "chained setters", shifted.Format("YYYY-MM-DD HH:mm:ss.SSS")
              "Day() (0 = Sunday)", string (reference.Day())
              "DaysInMonth()", string (reference.DaysInMonth())
              "Year / Month / Date", string (reference.Year()) + " / " + string (reference.Month()) + " / " + string (reference.Date())
              "Hour / Minute / Second / Millisecond",
              string (reference.Hour()) + " / " + string (reference.Minute()) + " / "
              + string (reference.Second()) + " / " + string (reference.Millisecond()) ]

    let pluralQuarter () =
        report manipulateReport "Plural getters, week and quarter"
            [ "pluralGetSet Years/Months/Dates", string (reference.Years()) + " / " + string (reference.Months()) + " / " + string (reference.Dates())
              "pluralGetSet Weeks/Days", string (reference.Weeks()) + " / " + string (reference.Days())
              "pluralGetSet Hours/Minutes/Seconds/Milliseconds",
              string (reference.Hours()) + " / " + string (reference.Minutes()) + " / "
              + string (reference.Seconds()) + " / " + string (reference.Milliseconds())
              "quarterOfYear Quarter()", string (reference.Quarter())
              "quarterOfYear Quarter(3)", reference.Quarter(3).Format("YYYY-MM-DD")
              "weekOfYear Week()", string (reference.Week())
              "weekYear WeekYear()", string (reference.WeekYear())
              "weekday Weekday()", string (reference.Weekday())
              "dayOfYear DayOfYear()", string (reference.DayOfYear())
              "isoWeek IsoWeek()", string (reference.IsoWeek())
              "isoWeek IsoWeekday()", string (reference.IsoWeekday())
              "isoWeek IsoWeekYear()", string (reference.IsoWeekYear())
              "isoWeeksInYear IsoWeeksInYear()", string (reference.IsoWeeksInYear()) ]

    // ---------------------------------------------------------------
    // Query
    // ---------------------------------------------------------------

    let runQueries () =
        report queryReport "Comparisons"
            [ "reference", reference.Format("YYYY-MM-DD HH:mm")
              "IsValid()", flag (reference.IsValid())
              "IsBefore(later)", flag (reference.IsBefore(later))
              "IsAfter(earlier)", flag (reference.IsAfter(earlier))
              "IsSame(\"2026-02-14\", \"day\")", flag (reference.IsSame("2026-02-14", "day"))
              "IsSame(\"2026-02-14 23:59\") (ms)", flag (reference.IsSame("2026-02-14 23:59"))
              "IsBetween(earlier, later, \"day\", \"[]\")", flag (reference.IsBetween(earlier, later, "day", "[]"))
              "IsBetween(..., \"()\")", flag (reference.IsBetween(earlier, later, "day", "()"))
              "IsSameOrAfter(earlier, \"year\")", flag (reference.IsSameOrAfter(earlier, "year"))
              "IsSameOrBefore(later, \"day\")", flag (reference.IsSameOrBefore(later, "day"))
              "IsToday()", flag (reference.IsToday())
              "IsTomorrow()", flag (reference.IsTomorrow())
              "IsYesterday()", flag (reference.IsYesterday())
              "IsLeapYear()", flag (reference.IsLeapYear())
              "IsDayjs(reference)", flag (Dayjs.IsDayjs(reference))
              "IsMoment(reference)", flag (Dayjs.IsMoment(reference))
              "IsDuration(42)", flag (Dayjs.IsDuration(42)) ]

    // ---------------------------------------------------------------
    // Difference & relative time
    // ---------------------------------------------------------------

    let runDiff () =
        report diffReport "Differences"
            [ "reference", reference.Format("YYYY-MM-DD HH:mm")
              "Diff(earlier) (ms)", num (reference.Diff(earlier))
              "Diff(earlier, \"day\")", num (reference.Diff(earlier, "day"))
              "Diff(earlier, \"hour\")", num (reference.Diff(earlier, "hour"))
              "Diff(earlier, \"week\")", num (reference.Diff(earlier, "week"))
              "Diff(earlier, \"month\")", num (reference.Diff(earlier, "month"))
              "Diff(earlier, \"year\")", num (reference.Diff(earlier, "year"))
              "Diff(later, \"day\")", num (reference.Diff(later, "day"))
              "Diff(later, \"day\", true)", num (reference.Diff(later, "day", true)) ]

    let runRelative () =
        report diffReport "Relative time"
            [ "reference.FromNow()", reference.FromNow()
              "reference.FromNow(true)", reference.FromNow(true)
              "reference.ToNow()", reference.ToNow()
              "reference.From(earlier)", reference.From(earlier)
              "reference.To(later)", reference.To(later)
              "earlier.FromNow()", earlier.FromNow()
              "later.FromNow()", later.FromNow()
              "reference.Calendar()", reference.Calendar()
              "reference.Calendar(formats)", reference.Calendar((null: Dayjs), CalendarFormats(SameDay = "[Today at] HH:mm", SameElse = "YYYY-MM-DD")) ]

    // ---------------------------------------------------------------
    // UTC & timezones
    // ---------------------------------------------------------------

    let runUtc () =
        report zoneReport "UTC mode"
            [ "Dayjs.UtcStatic()", Dayjs.UtcStatic().Format("YYYY-MM-DD HH:mm:ss") + " (offset " + string (Dayjs.UtcStatic().UtcOffset()) + ")"
              "Dayjs.UtcStatic(\"2026-02-14 09:30\")", Dayjs.UtcStatic("2026-02-14 09:30").Format()
              "reference.Utc()", reference.Utc().Format()
              "reference.Utc().IsUTC()", flag (reference.Utc().IsUTC())
              "reference.Utc().Local()", reference.Utc().Local().Format()
              "reference.UtcOffset()", string (reference.UtcOffset())
              "reference.UtcOffset(120)", reference.UtcOffset(120).Format() + " (offset " + string (reference.UtcOffset(120).UtcOffset()) + ")" ]

    let runTimezone () =
        let row label (f: unit -> string) =
            try label, f ()
            with _ -> label, "not available in this browser"
        report zoneReport "Timezones (timezone plugin, Intl data)"
            [ row "Dayjs.TzStatic(\"2026-02-14T09:30\", \"America/Toronto\")" (fun () ->
                Dayjs.TzStatic("2026-02-14T09:30:00", "America/Toronto").Format("YYYY-MM-DD HH:mm Z"))
              row "TzWithFormat(..., \"Asia/Tokyo\")" (fun () ->
                Dayjs.TzWithFormat("2026-02-14 09:30", "YYYY-MM-DD HH:mm", "Asia/Tokyo").Format("YYYY-MM-DD HH:mm Z"))
              row "reference.Tz(\"Europe/Budapest\")" (fun () ->
                reference.Tz("Europe/Budapest").Format("YYYY-MM-DD HH:mm Z"))
              row "OffsetName(\"short\")" (fun () ->
                match reference.Tz("Europe/Budapest").OffsetName("short") with
                | Defined x -> x
                | Undefined -> "-")
              row "OffsetName(\"long\")" (fun () ->
                match reference.Tz("Europe/Budapest").OffsetName("long") with
                | Defined x -> x
                | Undefined -> "-")
              row "Dayjs.TzGuess()" (fun () -> Dayjs.TzGuess()) ]

    // ---------------------------------------------------------------
    // Locales
    // ---------------------------------------------------------------

    let runLocale () =
        // Importing the locale module registers it with day.js (the module calls
        // `dayjs.locale(...)` on load); only referenced locales are bundled.
        Locales.Fr |> ignore
        let french = reference.Locale("fr")
        // A locale object passed to `locale(name, object)` *replaces* the
        // registered locale, so give a custom locale its own name.
        let custom = Locale(Name = "ws-custom", WeekStart = 1)
        let rows =
            [ "current locale (before)", Dayjs.LocaleStatic()
              "Dayjs.LocaleStatic(\"fr\")", Dayjs.LocaleStatic("fr")
              "Locale(\"fr\").Format(\"dddd D MMMM YYYY\")", french.Format("dddd D MMMM YYYY")
              "Locale(\"fr\").Format(\"LL\")", french.Format("LL")
              "Locale(\"fr\").Format(\"Do MMMM\")", french.Format("Do MMMM")
              "Locale() on the instance", french.Locale()
              "Dayjs.LocaleStatic(custom object)", Dayjs.LocaleStatic(custom)
              "back to en", reference.Locale("en").Format("dddd D MMMM YYYY") ]
        // Restore the global locale for the other sections.
        Dayjs.LocaleStatic("en") |> ignore
        localeReport.Value <-
            "Locales\n\n"
            + (rows |> List.fold (fun acc (label, value) -> acc + label + ": " + value + "\n") "")

    let runLocaleData () =
        let gld = Dayjs.LocaleData()
        let ld = reference.LocaleData()
        report localeReport "localeData plugin"
            [ "Weekdays()", join (Dayjs.Weekdays())
              "WeekdaysShort()", join (Dayjs.WeekdaysShort())
              "WeekdaysMin()", join (Dayjs.WeekdaysMin())
              "Months()", join (Dayjs.Months())
              "MonthsShort()", join (Dayjs.MonthsShort())
              "LocaleData().FirstDayOfWeek()", string (gld.FirstDayOfWeek.Invoke())
              "LocaleData().LongDateFormat(\"L\")", gld.LongDateFormat.Invoke("L")
              "LocaleData().Meridiem(9, 30, false)",
              (try gld.Meridiem.Invoke(9, 30, false)
               with _ -> "not defined by the en locale")
              "LocaleData().Ordinal(3)", string (gld.Ordinal.Invoke(3))
              // Passing an instance makes day.js return the formatted name
              // (the upstream types say otherwise); null keeps the array form.
              "reference.LocaleData().Weekdays()", join (ld.Weekdays.Invoke(null))
              "Ls registry: Ls.[\"en\"].Name", Dayjs.Ls.["en"].Name
              "Ls registry: Ls.[\"en\"].Weekdays.Length", string (Dayjs.Ls.["en"].Weekdays.Length) ]

    let runUpdateLocale () =
        let custom =
            Locale(
                Weekdays = [| "Sundae"; "Moonday"; "Tuesdee"; "Wednesdee"; "Thursdee"; "Fridee"; "Saturdee" |],
                RelativeTime = RelativeTimeKeys(Minute = "a wee minute", Minutes = "%d wee minutes")
            )
        let updated = Dayjs.UpdateLocale("en", custom)
        let formatted = reference.Format("dddd")
        report localeReport "updateLocale plugin"
            [ "Weekdays of the updated locale", join updated.Weekdays
              "RelativeTime.Minute", updated.RelativeTime.Minute
              "reference.Format(\"dddd\")", formatted
              "reference.FromNow() with the custom keys", reference.FromNow() ]

    // ---------------------------------------------------------------
    // Durations
    // ---------------------------------------------------------------

    let runDuration () =
        let ninety = Dayjs.Duration(90, "minutes")
        let iso = Dayjs.Duration("PT1H30M")
        let units = Dayjs.Duration(DurationUnitsObjectType(Hours = 1, Minutes = 30))
        report durationReport "Durations"
            [ "Dayjs.Duration(90, \"minutes\")", ninety.Humanize()
              "AsMinutes()", num (ninety.AsMinutes())
              "AsHours()", num (ninety.AsHours())
              "Milliseconds()/Seconds()/Minutes()",
              string (ninety.Milliseconds()) + " / " + string (ninety.Seconds()) + " / " + string (ninety.Minutes())
              "Format(\"HH:mm\")", ninety.Format("HH:mm")
              "ToISOString()", ninety.ToISOString()
              "ToJSON()", ninety.ToJSON()
              "ISO 8601 input", iso.Humanize()
              "units object", units.Humanize()
              "Add(15, \"minutes\")", ninety.Add(15, "minutes").Humanize()
              "Add(Duration(1, \"hour\"))", ninety.Add(Dayjs.Duration(1, "hour")).Humanize()
              "Subtract(Duration(1, \"hour\"))", ninety.Subtract(Dayjs.Duration(1, "hour")).Humanize()
              "IsDuration(ninety)", flag (Dayjs.IsDuration(ninety)) ]

    // ---------------------------------------------------------------
    // Objects, arrays & min/max
    // ---------------------------------------------------------------

    let runObjects () =
        let added = reference.Add(ObjectSupport(Days = 3))
        let subtracted = reference.Subtract(ObjectSupport(Months = 1))
        let set = reference.Set(ObjectSupport(Hours = 6, Minutes = 15))
        let obj = reference.ToObject()
        let arr = reference.ToArray()
        let min = Dayjs.Min([| earlier; reference; later |])
        let max = Dayjs.Max([| earlier; reference; later |])
        report objectReport "Objects, arrays & min/max"
            [ "Add({ days: 3 })", added.Format("YYYY-MM-DD HH:mm:ss")
              "Subtract({ months: 1 })", subtracted.Format("YYYY-MM-DD HH:mm:ss")
              "Set({ hours: 6, minutes: 15 })", set.Format("YYYY-MM-DD HH:mm:ss")
              "ToObject().Years/Months/Date", string obj.Years + " / " + string obj.Months + " / " + string obj.Date
              "ToObject().Hours/Minutes/Seconds/Milliseconds",
              string obj.Hours + " / " + string obj.Minutes + " / " + string obj.Seconds + " / " + string obj.Milliseconds
              "ToArray()", arr |> Array.map string |> join
              "Min([earlier; reference; later])",
              (match min with Defined d -> d.Format("YYYY-MM-DD") | Undefined -> "undefined")
              "Max([earlier; reference; later])",
              (match max with Defined d -> d.Format("YYYY-MM-DD") | Undefined -> "undefined")
              "Min([||])",
              (match Dayjs.Min([||] : Dayjs[]) with
               | Defined d when isNull (box d) -> "null"
               | Defined _ -> "a date"
               | Undefined -> "undefined") ]

    // ---------------------------------------------------------------
    // Type checks, calendar & installed plugins
    // ---------------------------------------------------------------

    let runPlugins () =
        let installed =
            [ "advancedFormat"; "arraySupport"; "badMutable"; "bigIntSupport"; "buddhistEra";
              "calendar"; "customParseFormat"; "dayOfYear"; "devHelper"; "duration"; "isBetween";
              "isLeapYear"; "isMoment"; "isSameOrAfter"; "isSameOrBefore"; "isToday";
              "isTomorrow"; "isYesterday"; "isoWeek"; "isoWeeksInYear"; "localeData";
              "localizedFormat"; "minMax"; "negativeYear"; "objectSupport"; "pluralGetSet";
              "preParsePostFormat"; "quarterOfYear";
              "relativeTime"; "timezone"; "toArray"; "toObject"; "updateLocale"; "utc";
              "weekOfYear"; "weekYear"; "weekday" ]
        report pluginReport "Type checks & the calendar plugin"
            [ "IsDayjs(reference)", flag (Dayjs.IsDayjs(reference))
              "IsMoment(reference)", flag (Dayjs.IsMoment(reference))
              "IsMoment(document)", flag (Dayjs.IsMoment(JS.Document))
              "IsDuration(42)", flag (Dayjs.IsDuration(42))
              "IsDuration(Dayjs.Duration(1, \"day\"))", flag (Dayjs.IsDuration(Dayjs.Duration(1, "day")))
              "reference.Calendar()", reference.Calendar()
              "Calendar(custom formats)",
              reference.Calendar((null: Dayjs), CalendarFormats(
                  SameDay = "[Today at] HH:mm",
                  LastDay = "[Yesterday at] HH:mm",
                  NextDay = "[Tomorrow at] HH:mm",
                  LastWeek = "[Last] dddd [at] HH:mm",
                  NextWeek = "dddd [at] HH:mm",
                  SameElse = "YYYY-MM-DD"))
              "Plugins available from the binding", string installed.Length
              "Plugin names", join (List.toArray installed) ]

    // ---------------------------------------------------------------
    // Entry point
    // ---------------------------------------------------------------

    /// Install the plugins the sample exercises. Day.js keeps a global plugin
    /// registry, so this only needs to happen once.
    let installPlugins () =
        Dayjs.Extend(Plugins.AdvancedFormat) |> ignore
        Dayjs.Extend(Plugins.ArraySupport) |> ignore
        Dayjs.Extend(Plugins.Calendar) |> ignore
        Dayjs.Extend(Plugins.CustomParseFormat) |> ignore
        Dayjs.Extend(Plugins.DayOfYear) |> ignore
        Dayjs.Extend(Plugins.Duration) |> ignore
        Dayjs.Extend(Plugins.IsBetween) |> ignore
        Dayjs.Extend(Plugins.IsLeapYear) |> ignore
        Dayjs.Extend(Plugins.IsMoment) |> ignore
        Dayjs.Extend(Plugins.IsSameOrAfter) |> ignore
        Dayjs.Extend(Plugins.IsSameOrBefore) |> ignore
        Dayjs.Extend(Plugins.IsToday) |> ignore
        Dayjs.Extend(Plugins.IsTomorrow) |> ignore
        Dayjs.Extend(Plugins.IsYesterday) |> ignore
        Dayjs.Extend(Plugins.IsoWeek) |> ignore
        Dayjs.Extend(Plugins.IsoWeeksInYear) |> ignore
        Dayjs.Extend(Plugins.LocaleData) |> ignore
        Dayjs.Extend(Plugins.LocalizedFormat) |> ignore
        Dayjs.Extend(Plugins.MinMax) |> ignore
        Dayjs.Extend(Plugins.PluralGetSet) |> ignore
        // quarterOfYear wraps `add`; objectSupport must wrap it *after*, or the
        // quarter wrapper turns the units object into NaN.
        Dayjs.Extend(Plugins.QuarterOfYear) |> ignore
        Dayjs.Extend(Plugins.ObjectSupport) |> ignore
        Dayjs.Extend(Plugins.RelativeTime) |> ignore
        Dayjs.Extend(Plugins.Timezone) |> ignore
        Dayjs.Extend(Plugins.ToArray) |> ignore
        Dayjs.Extend(Plugins.ToObject) |> ignore
        Dayjs.Extend(Plugins.UpdateLocale) |> ignore
        Dayjs.Extend(Plugins.Utc) |> ignore
        Dayjs.Extend(Plugins.WeekOfYear) |> ignore
        Dayjs.Extend(Plugins.WeekYear) |> ignore
        Dayjs.Extend(Plugins.Weekday) |> ignore

    [<SPAEntryPoint>]
    let Main () =
        installPlugins ()

        // Restore a previously chosen theme (light is the default).
        let savedTheme =
            try JS.Window.LocalStorage.GetItem(themeStorageKey) with _ -> null
        if savedTheme = "dark" || savedTheme = "light" then
            theme.Value <- savedTheme
        applyTheme ()

        // `.Bind()` renders the whole document body, so the elements below are
        // in the DOM only after this call.
        IndexTemplate()
            .themeLabel(themeLabel.V)
            .notice(notice.V)
            .parseReport(parseReport.V)
            .manipulateReport(manipulateReport.V)
            .queryReport(queryReport.V)
            .diffReport(diffReport.V)
            .zoneReport(zoneReport.V)
            .localeReport(localeReport.V)
            .durationReport(durationReport.V)
            .objectReport(objectReport.V)
            .pluginReport(pluginReport.V)
            .ToggleTheme(fun _ -> toggleTheme ())
            .ParseNow(fun _ -> parseNow ())
            .ParseCustom(fun _ -> parseCustom ())
            .FormatPatterns(fun _ -> formatPatterns ())
            .AddSubtract(fun _ -> addSubtract ())
            .StartEnd(fun _ -> startEnd ())
            .SetGet(fun _ -> setGet ())
            .PluralQuarter(fun _ -> pluralQuarter ())
            .RunQueries(fun _ -> runQueries ())
            .RunDiff(fun _ -> runDiff ())
            .RunRelative(fun _ -> runRelative ())
            .RunUtc(fun _ -> runUtc ())
            .RunTimezone(fun _ -> runTimezone ())
            .RunLocale(fun _ -> runLocale ())
            .RunLocaleData(fun _ -> runLocaleData ())
            .RunUpdateLocale(fun _ -> runUpdateLocale ())
            .RunDuration(fun _ -> runDuration ())
            .RunObjects(fun _ -> runObjects ())
            .RunPlugins(fun _ -> runPlugins ())
            .Bind()

        // Populate the first cards once the template is in the DOM.
        parseNow ()
        addSubtract ()
        runQueries ()