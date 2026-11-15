namespace WebSharper.DayJs

open WebSharper
open WebSharper.JavaScript
open WebSharper.InterfaceGenerator

module Definition =

    /// The Day.js factory/constructor. `Dayjs()` calls the default export of the
    /// npm package, so `Dayjs(date, format, locale, strict)` parses a date.
    /// Docs: https://day.js.org/docs/en/parse/now
    let Dayjs =
        Class "Dayjs"
        |> ImportDefault "dayjs"

    // Unit names accepted by the get/set/add/subtract/startOf/endOf/diff APIs.
    // Day.js units are case insensitive and accept plural and short forms, so
    // these are plain strings rather than enums.
    let UnitType = T<string>

    let OpUnitType = T<string>

    let QUnitType = T<string>

    let ISOUnitType = T<string>

    let ManipulateType = T<string>

    let DurationUnitType = T<string>

    /// An object in day.js' object-support format, e.g. `{ year: 2020, month: 0 }`.
    /// Adds the `objectSupport` plugin to use it.
    let ObjectSupport =
        Pattern.Config "ObjectSupport" {
            Required = []
            Optional = [
                "years", T<string> + T<int>
                "year", T<string> + T<int>
                "y", T<string> + T<int>

                "months", T<string> + T<int>
                "month", T<string> + T<int>
                "M", T<string> + T<int>

                "days", T<string> + T<int>
                "day", T<string> + T<int>
                // `d` is omitted: it would collide with `D` as a .NET property
                // name (the first letter is capitalized), and `D` is the
                // canonical date-of-month alias.

                "dates", T<string> + T<int>
                "date", T<string> + T<int>
                "D", T<string> + T<int>

                "hours", T<string> + T<int>
                "hour", T<string> + T<int>
                "h", T<string> + T<int>

                "minutes", T<string> + T<int>
                "minute", T<string> + T<int>
                // `m` is omitted: it would collide with `M` as a .NET property
                // name (the first letter is capitalized), and `M` is the
                // canonical month alias.

                "seconds", T<string> + T<int>
                "second", T<string> + T<int>
                "s", T<string> + T<int>

                "milliseconds", T<string> + T<int>
                "millisecond", T<string> + T<int>
                "ms", T<string> + T<int>
            ]
        }

    /// The values day.js accepts as a date, extended by the `arraySupport`,
    /// `objectSupport` and `bigIntSupport` plugins.
    let ConfigTypeMap =
        Pattern.Config "ConfigTypeMap" {
            Required = []
            Optional = [
                "default", T<string> + T<float> + T<Date> + Dayjs + T<unit>
                "arraySupport", !| (!?T<int>)
                "bigIntSupport", T<BigInt>
                "objectSupport", ObjectSupport.Type + T<obj>
            ]
        }

    let ConfigType = T<string> + T<float> + T<Date> + Dayjs + T<unit>

    /// The `format` argument of the parse functions: a format string, a list of
    /// formats, or an object selecting the format/locale/UTC mode.
    let FormatObject =
        Pattern.Config "FormatObject" {
            Required = []
            Optional = [
                "locale", T<string>
                "format", T<string>
                "utc", T<bool>
            ]
        }

    let OptionType = FormatObject + T<string> + !|T<string>

    /// The localized-format tokens replaced by the `localizedFormat` plugin.
    let FormatKeys =
        Pattern.Config "FormatKeys" {
            Required = []
            Optional = [
                "LT", T<string>
                "LTS", T<string>
                "L", T<string>
                "LL", T<string>
                "LLL", T<string>
                "LLLL", T<string>
            ]
        }

    /// The relative-time keys used by the `relativeTime` plugin.
    /// Note: `m`/`mm` are exposed as `Minute`/`Minutes` on the .NET side (their
    /// JS keys are still `m`/`mm`) because the first letter of a WIG property
    /// is capitalized, which would collide with `M`/`MM`.
    let RelativeTimeKeys =
        Pattern.Config "RelativeTimeKeys" {
            Required = []
            Optional = [
                "future", T<string>
                "past", T<string>
                "s", T<string>
                "h", T<string>
                "hh", T<string>
                "d", T<string>
                "dd", T<string>
                "M", T<string>
                "MM", T<string>
                "y", T<string>
                "yy", T<string>
            ]
        }
        |+> Instance [
            "m" =@ T<string> |> WithSourceName "Minute"
            "mm" =@ T<string> |> WithSourceName "Minutes"
        ]

    /// A (partial) locale definition, as passed to `dayjs.locale` and
    /// `dayjs.updateLocale`. All fields are optional so a single type covers
    /// both the full locale object and the partial overrides.
    let Locale =

        Pattern.Config "Locale" {
            Required = []
            Optional = [
                "name", T<string>
                "weekdays", !| T<string>
                "months", !| T<string>
                "weekStart", T<int>
                "weekdaysShort", !| T<string>
                "monthsShort", !| T<string>
                "weekdaysMin", !| T<string>
                "ordinal", T<int>?n ^-> (T<int> + T<string>)
                "formats", FormatKeys.Type
                "relativeTime", RelativeTimeKeys.Type
            ]
        }

    /// The `dayjs.Ls` locale registry, indexed by locale name.
    let LocaleDict =
        Class "LocaleDict"
        |+> Instance [
            "" =@ Locale |> Indexed T<string>
        ]

    let PluginFunc = T<obj>

    /// Options of the `customParseFormat` plugin.
    let PluginOptions =
        Pattern.Config "PluginOptions" {
            Required = []
            Optional = [
                ("parseTwoDigitYear", T<string>?yearString ^-> T<int>)
            ]
        }

    /// The object accepted by `dayjs.duration` (and `Duration.add`/`subtract`),
    /// from the `duration` plugin.
    let DurationUnitsObjectType =
        Pattern.Config "DurationUnitsObjectType" {
            Required = []
            Optional = [
                "milliseconds", T<int>
                "seconds", T<int>
                "minutes", T<int>
                "hours", T<int>
                "days", T<int>
                "weeks", T<int>
                "months", T<int>
                "years", T<int>
            ]
        }

    /// A Day.js duration, created with `Dayjs.Duration(...)` from the
    /// `duration` plugin.
    let Duration =
        Class "Duration"

    /// `[ 'Sunday', ... ]`-style day names returned by the `localeData` plugin.
    let WeekdayNames = !| T<string>

    /// `[ 'January', ... ]`-style month names returned by the `localeData` plugin.
    let MonthNames = !| T<string>

    let InstanceLocaleDataReturn =
        Pattern.Config "InstanceLocaleDataReturn" {
            Required = []
            Optional = [
                "firstDayOfWeek", T<unit> ^-> T<int>
                "weekdays", Dayjs?instance ^-> WeekdayNames
                "weekdaysShort", Dayjs?instance ^-> WeekdayNames
                "weekdaysMin", Dayjs?instance ^-> WeekdayNames
                "months", Dayjs?instance ^-> MonthNames
                "monthsShort", Dayjs?instance ^-> MonthNames
                "longDateFormat", T<string>?format ^-> T<string>
                "meridiem", T<int>?hour * T<int>?minute * T<bool>?isLower ^-> T<string>
                "ordinal", T<int>?n ^-> T<string>
            ]
        }

    let GlobalLocaleDataReturn =
        Pattern.Config "GlobalLocaleDataReturn" {
            Required = []
            Optional = [
                "firstDayOfWeek", T<unit> ^-> T<int>
                "weekdays", T<unit> ^-> WeekdayNames
                "weekdaysShort", T<unit> ^-> WeekdayNames
                "weekdaysMin", T<unit> ^-> WeekdayNames
                "months", T<unit> ^-> MonthNames
                "monthsShort", T<unit> ^-> MonthNames
                "longDateFormat", T<string>?format ^-> T<string>
                "meridiem", T<int>?hour * T<int>?minute * T<bool>?isLower ^-> T<string>
                "ordinal", T<int>?n ^-> T<string>
            ]
        }

    let RelativeTimeThreshold =
        Pattern.Config "RelativeTimeThreshold" {
            Required = []
            Optional = [
                "l", T<string>
                "r", T<int>
                "d", T<string>
            ]
        }

    /// Options of the `relativeTime` plugin.
    let RelativeTimeOptions =
        Pattern.Config "RelativeTimeOptions" {
            Required = []
            Optional = [
                "rounding", T<int>?num ^-> T<int>
                "thresholds", !| RelativeTimeThreshold.Type
            ]
        }

    /// The object returned by `Dayjs.ToObject()` (the `toObject` plugin).
    let DayjsObject =
        Pattern.Config "DayjsObject" {
            Required = [
                "years", T<int>
                "months", T<int>
                "date", T<int>
                "hours", T<int>
                "minutes", T<int>
                "seconds", T<int>
                "milliseconds", T<int>
            ]
            Optional = []
        }

    /// The formats object of the `calendar` plugin.
    let CalendarFormats =
        Pattern.Config "CalendarFormats" {
            Required = []
            Optional = [
                "sameDay", T<string>
                "nextDay", T<string>
                "nextWeek", T<string>
                "lastDay", T<string>
                "lastWeek", T<string>
                "sameElse", T<string>
            ]
        }

    /// The bundled locales (`dayjs/locale/<name>`). Importing a locale registers
    /// it with day.js, after which `Dayjs.LocaleStatic("<name>")` / `Locale("<name>")`
    /// switch to it. Only the locales you reference are bundled.
    let Locales =
        Class "Locales"
        |+> Static [
            "af" =? Locale
            |> ImportDefault "dayjs/locale/af"
            "am" =? Locale
            |> ImportDefault "dayjs/locale/am"
            "ar" =? Locale
            |> ImportDefault "dayjs/locale/ar"
            "ar-dz" =? Locale
            |> ImportDefault "dayjs/locale/ar-dz"
            "ar-iq" =? Locale
            |> ImportDefault "dayjs/locale/ar-iq"
            "ar-kw" =? Locale
            |> ImportDefault "dayjs/locale/ar-kw"
            "ar-ly" =? Locale
            |> ImportDefault "dayjs/locale/ar-ly"
            "ar-ma" =? Locale
            |> ImportDefault "dayjs/locale/ar-ma"
            "ar-sa" =? Locale
            |> ImportDefault "dayjs/locale/ar-sa"
            "ar-tn" =? Locale
            |> ImportDefault "dayjs/locale/ar-tn"
            "az" =? Locale
            |> ImportDefault "dayjs/locale/az"
            "be" =? Locale
            |> ImportDefault "dayjs/locale/be"
            "bg" =? Locale
            |> ImportDefault "dayjs/locale/bg"
            "bi" =? Locale
            |> ImportDefault "dayjs/locale/bi"
            "bm" =? Locale
            |> ImportDefault "dayjs/locale/bm"
            "bn" =? Locale
            |> ImportDefault "dayjs/locale/bn"
            "bn-bd" =? Locale
            |> ImportDefault "dayjs/locale/bn-bd"
            "bo" =? Locale
            |> ImportDefault "dayjs/locale/bo"
            "br" =? Locale
            |> ImportDefault "dayjs/locale/br"
            "bs" =? Locale
            |> ImportDefault "dayjs/locale/bs"
            "ca" =? Locale
            |> ImportDefault "dayjs/locale/ca"
            "cs" =? Locale
            |> ImportDefault "dayjs/locale/cs"
            "cv" =? Locale
            |> ImportDefault "dayjs/locale/cv"
            "cy" =? Locale
            |> ImportDefault "dayjs/locale/cy"
            "da" =? Locale
            |> ImportDefault "dayjs/locale/da"
            "de" =? Locale
            |> ImportDefault "dayjs/locale/de"
            "de-at" =? Locale
            |> ImportDefault "dayjs/locale/de-at"
            "de-ch" =? Locale
            |> ImportDefault "dayjs/locale/de-ch"
            "dv" =? Locale
            |> ImportDefault "dayjs/locale/dv"
            "el" =? Locale
            |> ImportDefault "dayjs/locale/el"
            "en" =? Locale
            |> ImportDefault "dayjs/locale/en"
            "en-au" =? Locale
            |> ImportDefault "dayjs/locale/en-au"
            "en-ca" =? Locale
            |> ImportDefault "dayjs/locale/en-ca"
            "en-gb" =? Locale
            |> ImportDefault "dayjs/locale/en-gb"
            "en-ie" =? Locale
            |> ImportDefault "dayjs/locale/en-ie"
            "en-il" =? Locale
            |> ImportDefault "dayjs/locale/en-il"
            "en-in" =? Locale
            |> ImportDefault "dayjs/locale/en-in"
            "en-nz" =? Locale
            |> ImportDefault "dayjs/locale/en-nz"
            "en-sg" =? Locale
            |> ImportDefault "dayjs/locale/en-sg"
            "en-tt" =? Locale
            |> ImportDefault "dayjs/locale/en-tt"
            "eo" =? Locale
            |> ImportDefault "dayjs/locale/eo"
            "es" =? Locale
            |> ImportDefault "dayjs/locale/es"
            "es-do" =? Locale
            |> ImportDefault "dayjs/locale/es-do"
            "es-mx" =? Locale
            |> ImportDefault "dayjs/locale/es-mx"
            "es-pr" =? Locale
            |> ImportDefault "dayjs/locale/es-pr"
            "es-us" =? Locale
            |> ImportDefault "dayjs/locale/es-us"
            "et" =? Locale
            |> ImportDefault "dayjs/locale/et"
            "eu" =? Locale
            |> ImportDefault "dayjs/locale/eu"
            "fa" =? Locale
            |> ImportDefault "dayjs/locale/fa"
            "fi" =? Locale
            |> ImportDefault "dayjs/locale/fi"
            "fo" =? Locale
            |> ImportDefault "dayjs/locale/fo"
            "fr" =? Locale
            |> ImportDefault "dayjs/locale/fr"
            "fr-ca" =? Locale
            |> ImportDefault "dayjs/locale/fr-ca"
            "fr-ch" =? Locale
            |> ImportDefault "dayjs/locale/fr-ch"
            "fy" =? Locale
            |> ImportDefault "dayjs/locale/fy"
            "ga" =? Locale
            |> ImportDefault "dayjs/locale/ga"
            "gd" =? Locale
            |> ImportDefault "dayjs/locale/gd"
            "gl" =? Locale
            |> ImportDefault "dayjs/locale/gl"
            "gom-latn" =? Locale
            |> ImportDefault "dayjs/locale/gom-latn"
            "gu" =? Locale
            |> ImportDefault "dayjs/locale/gu"
            "he" =? Locale
            |> ImportDefault "dayjs/locale/he"
            "hi" =? Locale
            |> ImportDefault "dayjs/locale/hi"
            "hr" =? Locale
            |> ImportDefault "dayjs/locale/hr"
            "ht" =? Locale
            |> ImportDefault "dayjs/locale/ht"
            "hu" =? Locale
            |> ImportDefault "dayjs/locale/hu"
            "hy-am" =? Locale
            |> ImportDefault "dayjs/locale/hy-am"
            "id" =? Locale
            |> ImportDefault "dayjs/locale/id"
            "is" =? Locale
            |> ImportDefault "dayjs/locale/is"
            "it" =? Locale
            |> ImportDefault "dayjs/locale/it"
            "it-ch" =? Locale
            |> ImportDefault "dayjs/locale/it-ch"
            "ja" =? Locale
            |> ImportDefault "dayjs/locale/ja"
            "jv" =? Locale
            |> ImportDefault "dayjs/locale/jv"
            "ka" =? Locale
            |> ImportDefault "dayjs/locale/ka"
            "kk" =? Locale
            |> ImportDefault "dayjs/locale/kk"
            "km" =? Locale
            |> ImportDefault "dayjs/locale/km"
            "kn" =? Locale
            |> ImportDefault "dayjs/locale/kn"
            "ko" =? Locale
            |> ImportDefault "dayjs/locale/ko"
            "ku" =? Locale
            |> ImportDefault "dayjs/locale/ku"
            "ky" =? Locale
            |> ImportDefault "dayjs/locale/ky"
            "lb" =? Locale
            |> ImportDefault "dayjs/locale/lb"
            "lo" =? Locale
            |> ImportDefault "dayjs/locale/lo"
            "lt" =? Locale
            |> ImportDefault "dayjs/locale/lt"
            "lv" =? Locale
            |> ImportDefault "dayjs/locale/lv"
            "me" =? Locale
            |> ImportDefault "dayjs/locale/me"
            "mi" =? Locale
            |> ImportDefault "dayjs/locale/mi"
            "mk" =? Locale
            |> ImportDefault "dayjs/locale/mk"
            "ml" =? Locale
            |> ImportDefault "dayjs/locale/ml"
            "mn" =? Locale
            |> ImportDefault "dayjs/locale/mn"
            "mr" =? Locale
            |> ImportDefault "dayjs/locale/mr"
            "ms" =? Locale
            |> ImportDefault "dayjs/locale/ms"
            "ms-my" =? Locale
            |> ImportDefault "dayjs/locale/ms-my"
            "mt" =? Locale
            |> ImportDefault "dayjs/locale/mt"
            "my" =? Locale
            |> ImportDefault "dayjs/locale/my"
            "nb" =? Locale
            |> ImportDefault "dayjs/locale/nb"
            "ne" =? Locale
            |> ImportDefault "dayjs/locale/ne"
            "nl" =? Locale
            |> ImportDefault "dayjs/locale/nl"
            "nl-be" =? Locale
            |> ImportDefault "dayjs/locale/nl-be"
            "nn" =? Locale
            |> ImportDefault "dayjs/locale/nn"
            "oc-lnc" =? Locale
            |> ImportDefault "dayjs/locale/oc-lnc"
            "pa-in" =? Locale
            |> ImportDefault "dayjs/locale/pa-in"
            "pl" =? Locale
            |> ImportDefault "dayjs/locale/pl"
            "pt" =? Locale
            |> ImportDefault "dayjs/locale/pt"
            "pt-br" =? Locale
            |> ImportDefault "dayjs/locale/pt-br"
            "rn" =? Locale
            |> ImportDefault "dayjs/locale/rn"
            "ro" =? Locale
            |> ImportDefault "dayjs/locale/ro"
            "ru" =? Locale
            |> ImportDefault "dayjs/locale/ru"
            "rw" =? Locale
            |> ImportDefault "dayjs/locale/rw"
            "sd" =? Locale
            |> ImportDefault "dayjs/locale/sd"
            "se" =? Locale
            |> ImportDefault "dayjs/locale/se"
            "si" =? Locale
            |> ImportDefault "dayjs/locale/si"
            "sk" =? Locale
            |> ImportDefault "dayjs/locale/sk"
            "sl" =? Locale
            |> ImportDefault "dayjs/locale/sl"
            "sq" =? Locale
            |> ImportDefault "dayjs/locale/sq"
            "sr" =? Locale
            |> ImportDefault "dayjs/locale/sr"
            "sr-cyrl" =? Locale
            |> ImportDefault "dayjs/locale/sr-cyrl"
            "ss" =? Locale
            |> ImportDefault "dayjs/locale/ss"
            "sv" =? Locale
            |> ImportDefault "dayjs/locale/sv"
            "sv-fi" =? Locale
            |> ImportDefault "dayjs/locale/sv-fi"
            "sw" =? Locale
            |> ImportDefault "dayjs/locale/sw"
            "ta" =? Locale
            |> ImportDefault "dayjs/locale/ta"
            "te" =? Locale
            |> ImportDefault "dayjs/locale/te"
            "tet" =? Locale
            |> ImportDefault "dayjs/locale/tet"
            "tg" =? Locale
            |> ImportDefault "dayjs/locale/tg"
            "th" =? Locale
            |> ImportDefault "dayjs/locale/th"
            "tk" =? Locale
            |> ImportDefault "dayjs/locale/tk"
            "tl-ph" =? Locale
            |> ImportDefault "dayjs/locale/tl-ph"
            "tlh" =? Locale
            |> ImportDefault "dayjs/locale/tlh"
            "tr" =? Locale
            |> ImportDefault "dayjs/locale/tr"
            "tzl" =? Locale
            |> ImportDefault "dayjs/locale/tzl"
            "tzm" =? Locale
            |> ImportDefault "dayjs/locale/tzm"
            "tzm-latn" =? Locale
            |> ImportDefault "dayjs/locale/tzm-latn"
            "ug-cn" =? Locale
            |> ImportDefault "dayjs/locale/ug-cn"
            "uk" =? Locale
            |> ImportDefault "dayjs/locale/uk"
            "ur" =? Locale
            |> ImportDefault "dayjs/locale/ur"
            "uz" =? Locale
            |> ImportDefault "dayjs/locale/uz"
            "uz-latn" =? Locale
            |> ImportDefault "dayjs/locale/uz-latn"
            "vi" =? Locale
            |> ImportDefault "dayjs/locale/vi"
            "x-pseudo" =? Locale
            |> ImportDefault "dayjs/locale/x-pseudo"
            "yo" =? Locale
            |> ImportDefault "dayjs/locale/yo"
            "zh" =? Locale
            |> ImportDefault "dayjs/locale/zh"
            "zh-cn" =? Locale
            |> ImportDefault "dayjs/locale/zh-cn"
            "zh-hk" =? Locale
            |> ImportDefault "dayjs/locale/zh-hk"
            "zh-tw" =? Locale
            |> ImportDefault "dayjs/locale/zh-tw"
        ]

    /// The Day.js plugins, each the default export of `dayjs/plugin/<name>`.
    /// Install one with `Dayjs.Extend(Plugins.Utc)` before using its API.
    let Plugins =
        Class "Plugins"
        |+> Static [
            "advancedFormat" =? PluginFunc
            |> ImportDefault "dayjs/plugin/advancedFormat"

            "arraySupport" =? PluginFunc
            |> ImportDefault "dayjs/plugin/arraySupport"

            "badMutable" =? PluginFunc
            |> ImportDefault "dayjs/plugin/badMutable"

            "bigIntSupport" =? PluginFunc
            |> ImportDefault "dayjs/plugin/bigIntSupport"

            "buddhistEra" =? PluginFunc
            |> ImportDefault "dayjs/plugin/buddhistEra"

            "calendar" =? PluginFunc
            |> ImportDefault "dayjs/plugin/calendar"

            "customParseFormat" =? PluginFunc
            |> ImportDefault "dayjs/plugin/customParseFormat"

            "dayOfYear" =? PluginFunc
            |> ImportDefault "dayjs/plugin/dayOfYear"

            "devHelper" =? PluginFunc
            |> ImportDefault "dayjs/plugin/devHelper"

            "duration" =? PluginFunc
            |> ImportDefault "dayjs/plugin/duration"

            "isBetween" =? PluginFunc
            |> ImportDefault "dayjs/plugin/isBetween"

            "isLeapYear" =? PluginFunc
            |> ImportDefault "dayjs/plugin/isLeapYear"

            "isMoment" =? PluginFunc
            |> ImportDefault "dayjs/plugin/isMoment"

            "isSameOrAfter" =? PluginFunc
            |> ImportDefault "dayjs/plugin/isSameOrAfter"

            "isSameOrBefore" =? PluginFunc
            |> ImportDefault "dayjs/plugin/isSameOrBefore"

            "isToday" =? PluginFunc
            |> ImportDefault "dayjs/plugin/isToday"

            "isTomorrow" =? PluginFunc
            |> ImportDefault "dayjs/plugin/isTomorrow"

            "isYesterday" =? PluginFunc
            |> ImportDefault "dayjs/plugin/isYesterday"

            "isoWeek" =? PluginFunc
            |> ImportDefault "dayjs/plugin/isoWeek"

            "isoWeeksInYear" =? PluginFunc
            |> ImportDefault "dayjs/plugin/isoWeeksInYear"

            "localeData" =? PluginFunc
            |> ImportDefault "dayjs/plugin/localeData"

            "localizedFormat" =? PluginFunc
            |> ImportDefault "dayjs/plugin/localizedFormat"

            "minMax" =? PluginFunc
            |> ImportDefault "dayjs/plugin/minMax"

            "negativeYear" =? PluginFunc
            |> ImportDefault "dayjs/plugin/negativeYear"

            "objectSupport" =? PluginFunc
            |> ImportDefault "dayjs/plugin/objectSupport"

            "pluralGetSet" =? PluginFunc
            |> ImportDefault "dayjs/plugin/pluralGetSet"

            "preParsePostFormat" =? PluginFunc
            |> ImportDefault "dayjs/plugin/preParsePostFormat"

            "quarterOfYear" =? PluginFunc
            |> ImportDefault "dayjs/plugin/quarterOfYear"

            "relativeTime" =? PluginFunc
            |> ImportDefault "dayjs/plugin/relativeTime"

            "timezone" =? PluginFunc
            |> ImportDefault "dayjs/plugin/timezone"

            "toArray" =? PluginFunc
            |> ImportDefault "dayjs/plugin/toArray"

            "toObject" =? PluginFunc
            |> ImportDefault "dayjs/plugin/toObject"

            "updateLocale" =? PluginFunc
            |> ImportDefault "dayjs/plugin/updateLocale"

            "utc" =? PluginFunc
            |> ImportDefault "dayjs/plugin/utc"

            "weekOfYear" =? PluginFunc
            |> ImportDefault "dayjs/plugin/weekOfYear"

            "weekYear" =? PluginFunc
            |> ImportDefault "dayjs/plugin/weekYear"

            "weekday" =? PluginFunc
            |> ImportDefault "dayjs/plugin/weekday"
        ]

    Duration
        |+> Static [
            Constructor ((T<string> + T<int> + DurationUnitsObjectType)?input * !?T<string>?unit * !?T<string>?locale)
            |> ObsoleteWithMessage "The Duration class is not exported by the duration plugin. Create durations with `Dayjs.Duration(...)` instead."
        ]
        |+> Instance [
            "clone" => T<unit> ^-> TSelf
            "humanize" => !?T<bool>?withSuffix ^-> T<string>
            "milliseconds" => T<unit> ^-> T<int>
            "asMilliseconds" => T<unit> ^-> T<float>
            "seconds" => T<unit> ^-> T<int>
            "asSeconds" => T<unit> ^-> T<float>
            "minutes" => T<unit> ^-> T<int>
            "asMinutes" => T<unit> ^-> T<float>
            "hours" => T<unit> ^-> T<int>
            "asHours" => T<unit> ^-> T<float>
            "days" => T<unit> ^-> T<int>
            "asDays" => T<unit> ^-> T<float>
            "weeks" => T<unit> ^-> T<int>
            "asWeeks" => T<unit> ^-> T<float>
            "months" => T<unit> ^-> T<int>
            "asMonths" => T<unit> ^-> T<float>
            "years" => T<unit> ^-> T<int>
            "asYears" => T<unit> ^-> T<float>

            "as" => DurationUnitType?unit ^-> T<float>
            "get" => DurationUnitType?unit ^-> T<int>

            "add" => DurationUnitsObjectType?units ^-> TSelf
            "add" => T<int>?time * !?DurationUnitType?unit ^-> TSelf
            "add" => T<string>?ISO_8601 ^-> TSelf
            "add" => Duration?duration ^-> TSelf

            "subtract" => DurationUnitsObjectType?units ^-> TSelf
            "subtract" => T<int>?time * !?DurationUnitType?unit ^-> TSelf
            "subtract" => T<string>?ISO_8601 ^-> TSelf
            "subtract" => Duration?duration ^-> TSelf

            "toJSON" => T<unit> ^-> T<string>
            "toISOString" => T<unit> ^-> T<string>
            "format" => !?T<string>?formatStr ^-> T<string>
            "locale" => T<string>?locale ^-> TSelf
        ]
        |> ignore

    Dayjs
        |+> Static [
            Constructor (
                !?ConfigType?config *
                !?OptionType?format *
                !?T<string>?locale *
                !?T<bool>?strict
            )
            |> WithComment "`dayjs(date?, format?, locale?, strict?)` — parse a date. Docs: https://day.js.org/docs/en/parse/parse"

            // `dayjs([year, month, day, hour, minute, second, millisecond])`
            // (the `arraySupport` plugin).
            Constructor ((!|T<int>)?dateArray)
            |> WithComment "Parse from an array of date parts. Add `arraySupport` plugin to use this overload"

            // `dayjs({ year, month, date: 1, hours, ... })` (the `objectSupport` plugin).
            Constructor (ObjectSupport?objectSupport)
            |> WithComment "Parse from an object of date parts. Add `objectSupport` plugin to use this overload"

            "extend" => PluginFunc?plugin * !? T<obj>?options ^-> TSelf
            |> WithComment "Install a plugin (one of `Plugins`) with optional plugin options. Docs: https://day.js.org/docs/en/plugin/plugin"

            "locale" => !?(T<string> + Locale)?preset * !?Locale?object * !?T<bool>?isLocale ^-> T<string>
            |> WithSourceName "LocaleStatic"
            |> WithComment "Get the global locale name, or set/define a locale. Docs: https://day.js.org/docs/en/i18n/i18n"

            "isDayjs" => T<obj>?d ^-> T<bool>
            |> WithComment "Check whether a value is a Day.js object."

            "unix" => (T<float> + T<BigInt>)?t ^-> TSelf
            |> WithComment "`dayjs.unix(t)` — create a Day.js object from a Unix timestamp (seconds). The `bigIntSupport` plugin adds the `BigInt` input. Docs: https://day.js.org/docs/en/display/unix-timestamp"

            "Ls" =? LocaleDict
            |> WithComment "The locale registry (`dayjs.Ls`), indexed by locale name."

            // duration plugin
            "duration" => DurationUnitsObjectType?units ^-> Duration
            |> WithComment "Create a `Duration` from a units object. Add `duration` plugin to use `duration` function. Docs: https://day.js.org/docs/en/durations/creating"
            "duration" => T<int>?time * !?DurationUnitType?unit ^-> Duration
            |> WithComment "Create a `Duration` from an amount and a unit. Add `duration` plugin to use `duration` function."
            "duration" => T<string>?ISO_8601 ^-> Duration
            |> WithComment "Create a `Duration` from an ISO 8601 string. Add `duration` plugin to use `duration` function."
            "isDuration" => T<obj>?d ^-> T<bool>
            |> WithComment "Add `duration` plugin to use `isDuration` function"
            // isMoment plugin
            "isMoment" => T<obj>?input ^-> T<bool>
            |> WithComment "Add `isMoment` plugin to use `isMoment` function"

            // localeData plugin
            "weekdays" => !?T<bool>?localOrder ^-> WeekdayNames
            |> WithComment "Add `localeData` plugin to use `weekdays` function"

            "weekdaysShort" => !?T<bool>?localOrder ^-> WeekdayNames
            |> WithComment "Add `localeData` plugin to use `weekdaysShort` function"

            "weekdaysMin" => !?T<bool>?localOrder ^-> WeekdayNames
            |> WithComment "Add `localeData` plugin to use `weekdaysMin` function"

            "monthsShort" => T<unit> ^-> MonthNames
            |> WithComment "Add `localeData` plugin to use `monthsShort` function"

            "months" => T<unit> ^-> MonthNames
            |> WithComment "Add `localeData` plugin to use `months` function"

            "localeData" => T<unit> ^-> GlobalLocaleDataReturn
            |> WithComment "Add `localeData` plugin to use `localeData` function"

            // minMax plugin
            "max" => (!|Dayjs)?maybeDates ^-> !? Dayjs
            |> WithComment "The latest of the given Day.js objects, or None when the list is empty. Add `minMax` plugin to use `max` function"
            "min" => (!|Dayjs)?maybeDates ^-> !? Dayjs
            |> WithComment "The earliest of the given Day.js objects, or None when the list is empty. Add `minMax` plugin to use `min` function"

            // timezone plugin
            "tz" => (!?ConfigType?date * !?T<string>?timezone) ^-> TSelf
            |> WithSourceName "TzStatic"
            |> WithComment "`dayjs.tz(date?, timezone?)`. Add `timezone` plugin to use `tz` function. Docs: https://day.js.org/docs/en/timezone/timezone"
            // Named TzWithFormat on the .NET side: the static and instance `tz`
            // members would otherwise generate the same overload signature.
            "tz" => (ConfigType?date * T<string>?format * !?T<string>?timezone) ^-> TSelf
            |> WithSourceName "TzWithFormat"
            |> WithComment "`dayjs.tz(date, format, timezone?)`. Add `timezone` plugin to use `tz` function"

            "tz.guess" => T<unit> ^-> T<string>
            |> WithSourceName "TzGuess"
            |> WithComment "`dayjs.tz.guess()` — the user's timezone. Add `timezone` plugin to use `tz.guess` function"
            "tz.setDefault" => !?T<string>?timezone ^-> T<unit>
            |> WithSourceName "TzSetDefault"
            |> WithComment "`dayjs.tz.setDefault(...)`. Add `timezone` plugin to use `tz.setDefault` function"

            // updateLocale plugin
            "updateLocale" => T<string>?localeName * Locale?customConfig ^-> Locale
            |> WithComment "Update a locale definition. Add `updateLocale` plugin to use `updateLocale` function. Docs: https://day.js.org/docs/en/i18n/updating-locale"
            // utc plugin
            "utc" => !?ConfigType?config * !?T<string>?format * !?T<bool>?strict ^-> TSelf
            |> WithSourceName "UtcStatic"
            |> WithComment "`dayjs.utc(...)` — parse in UTC mode. Add `utc` plugin to use `utc` function"
        ]
        |+> Instance [
            "clone" => T<unit> ^-> TSelf
            |> WithComment "Return a copy of the Day.js object. All Day.js objects are immutable; `clone()` returns an independent copy."

            "isValid" => T<unit> ^-> T<bool>
            |> WithComment "Whether the object contains a valid date."

            "year" => T<unit> ^-> T<int>
            "year" => T<int>?value ^-> TSelf

            "month" => T<unit> ^-> T<int>
            "month" => T<int>?value ^-> TSelf

            "date" => T<unit> ^-> T<int>
            "date" => T<int>?value ^-> TSelf

            "day" => T<unit> ^-> T<int>
            "day" => T<int>?value ^-> TSelf

            "hour" => T<unit> ^-> T<int>
            "hour" => T<int>?value ^-> TSelf

            "minute" => T<unit> ^-> T<int>
            "minute" => T<int>?value ^-> TSelf

            "second" => T<unit> ^-> T<int>
            "second" => T<int>?value ^-> TSelf

            "millisecond" => T<unit> ^-> T<int>
            "millisecond" => T<int>?value ^-> TSelf

            "set" => UnitType?unitName * T<int>?value ^-> TSelf
            |> WithComment "Generic setter: `set(unit, value)` is `dayjs()[unit](value)`."

            "get" => UnitType?unitName ^-> T<int>
            |> WithComment "Generic getter: `get(unit)` is `dayjs()[unit]()`."

            "add" => (T<float>?value * !?ManipulateType?unit) ^-> TSelf
            |> WithComment "A copy with the given amount of time added. Docs: https://day.js.org/docs/en/manipulate/add"
            "subtract" => (T<float>?value * !?ManipulateType?unit) ^-> TSelf
            |> WithComment "A copy with the given amount of time subtracted. Docs: https://day.js.org/docs/en/manipulate/subtract"
            "startOf" => (OpUnitType + ISOUnitType + QUnitType)?unit ^-> TSelf
            |> WithComment "A copy set to the start of the given unit."
            "endOf" => (OpUnitType + ISOUnitType + QUnitType)?unit ^-> TSelf
            |> WithComment "A copy set to the end of the given unit."

            "format" => !?T<string>?template ^-> T<string>
            |> WithComment "Format the date; without a template it returns the ISO 8601 representation. Docs: https://day.js.org/docs/en/display/format"

            "diff" => (!?ConfigType?date * !? (QUnitType + OpUnitType)?unit * !?T<bool>?float) ^-> T<float>
            |> WithComment "The difference to another date in the given unit (milliseconds by default); `float = true` keeps the fraction. Docs: https://day.js.org/docs/en/display/difference"

            "valueOf" => T<unit> ^-> T<int>
            |> WithComment "Milliseconds since the Unix epoch."
            "unix" => T<unit> ^-> T<int>
            |> WithComment "Seconds since the Unix epoch (floored)."
            "daysInMonth" => T<unit> ^-> T<int>
            "toDate" => T<unit> ^-> T<Date>
            "toJSON" => T<unit> ^-> T<string>
            "toISOString" => T<unit> ^-> T<string>
            "toString" => T<unit> ^-> T<string>

            "utcOffset" => T<unit> ^-> T<int>
            |> WithComment "The UTC offset in minutes."

            "isBefore" => !?ConfigType?date * !?(OpUnitType + ISOUnitType)?unit ^-> T<bool>
            "isSame" => !?ConfigType?date * !?(OpUnitType + ISOUnitType)?unit ^-> T<bool>
            "isAfter" => !?ConfigType?date * !?(OpUnitType + ISOUnitType)?unit ^-> T<bool>

            "locale" => T<unit> ^-> T<string>
            |> WithComment "The current locale name."
            "locale" => (T<string> + Locale)?preset * !?Locale?object ^-> TSelf
            |> WithComment "Switch to (or define) a locale."

            // calendar plugin
            "calendar" => !?ConfigType?referenceTime * !?CalendarFormats?formats ^-> T<string>
            |> WithComment "A calendar string (e.g. \"Today at 10:00\") relative to `referenceTime`. Add `calendar` plugin to use `calendar` function"

            // dayOfYear plugin
            "dayOfYear" => T<unit> ^-> T<int>
            |> WithComment "Add `dayOfYear` plugin to use `dayOfYear` function"
            "dayOfYear" => T<int>?value ^-> TSelf
            |> WithComment "Add `dayOfYear` plugin to use `dayOfYear` function"

            // duration plugin
            "add" => Duration?duration ^-> TSelf
            |> WithComment "Add `duration` plugin to use `add` function"
            "subtract" => Duration?duration ^-> TSelf
            |> WithComment "Add `duration` plugin to use `subtract` function"

            // isBetween plugin
            "isBetween" => ConfigType?a * ConfigType?b * !?OpUnitType?c * !?T<string>?d ^-> T<bool>
            |> WithComment "Add `isBetween` plugin to use `isBetween` function"

            // isLeapYear plugin
            "isLeapYear" => T<unit> ^-> T<bool>
            |> WithComment "Add `isLeapYear` plugin to use `isLeapYear` function"

            // isSameOrAfter plugin
            "isSameOrAfter" => !?ConfigType?date * !?OpUnitType?unit ^-> T<bool>
            |> WithComment "Add `isSameOrAfter` plugin to use `isSameOrAfter` function"

            // isSameOrBefore plugin
            "isSameOrBefore" => !?ConfigType?date * !?OpUnitType?unit ^-> T<bool>
            |> WithComment "Add `isSameOrBefore` plugin to use `isSameOrBefore` function"

            // isToday plugin
            "isToday" => T<unit> ^-> T<bool>
            |> WithComment "Add `isToday` plugin to use `isToday` function"

            //isTomorrow
            "isTomorrow" => T<unit> ^-> T<bool>
            |> WithComment "Add `isTomorrow` plugin to use `isTomorrow` function"

            // isYesterday plugin
            "isYesterday" => T<unit> ^-> T<bool>
            |> WithComment "Add `isYesterday` plugin to use `isYesterday` function"

            // isoWeek plugin
            "isoWeekYear" => T<unit> ^-> T<int>
            |> WithComment "Add `isoWeek` plugin to use `isoWeekYear` function"

            "isoWeek" => T<unit> ^-> T<int>
            |> WithComment "Add `isoWeek` plugin to use `isoWeek` function"

            "isoWeek" => T<int>?value ^-> TSelf
            |> WithComment "Add `isoWeek` plugin to use `isoWeek` function"

            "isoWeekday" => T<unit> ^-> T<int>
            |> WithComment "Add `isoWeek` plugin to use `isoWeekday` function"

            "isoWeekday" => T<int>?value ^-> TSelf
            |> WithComment "Add `isoWeek` plugin to use `isoWeekday` function"

            // isoWeeksInYear plugin
            "isoWeeksInYear" => T<unit> ^-> T<int>
            |> WithComment "Add `isoWeeksInYear` plugin to use `isoWeeksInYear` function"

            // localeData plugin
            "localeData" => T<unit> ^-> InstanceLocaleDataReturn
            |> WithComment "Add `localeData` plugin to use `localeData` function"

            // objectSupport methods
            "set" => ObjectSupport?argument ^-> TSelf
            |> WithComment "Add `objectSupport` plugin to use `set` function"

            "add" => ObjectSupport?argument ^-> TSelf
            |> WithComment "Add `objectSupport` plugin to use `add` function"

            "subtract" => ObjectSupport?argument ^-> TSelf
            |> WithComment "Add `objectSupport` plugin to use `subtract` function"

            // pluralGetSet plugin
            "years" => T<unit> ^-> T<int>
            |> WithComment "Add `pluralGetSet` plugin to use `years` function"
            "years" => T<int>?value ^-> TSelf
            |> WithComment "Add `pluralGetSet` plugin to use `years` function"

            "months" => T<unit> ^-> T<int>
            |> WithComment "Add `pluralGetSet` plugin to use `months` function"
            "months" => T<int>?value ^-> TSelf
            |> WithComment "Add `pluralGetSet` plugin to use `months` function"

            "dates" => T<unit> ^-> T<int>
            |> WithComment "Add `pluralGetSet` plugin to use `dates` function"
            "dates" => T<int>?value ^-> TSelf
            |> WithComment "Add `pluralGetSet` plugin to use `dates` function"

            "weeks" => T<unit> ^-> T<int>
            |> WithComment "Add `pluralGetSet` plugin to use `weeks` function"
            "weeks" => T<int>?value ^-> TSelf
            |> WithComment "Add `pluralGetSet` plugin to use `weeks` function"

            "days" => T<unit> ^-> T<int>
            |> WithComment "Add `pluralGetSet` plugin to use `days` function"
            "days" => T<int>?value ^-> TSelf
            |> WithComment "Add `pluralGetSet` plugin to use `days` function"

            "hours" => T<unit> ^-> T<int>
            |> WithComment "Add `pluralGetSet` plugin to use `hours` function"
            "hours" => T<int>?value ^-> TSelf
            |> WithComment "Add `pluralGetSet` plugin to use `hours` function"

            "minutes" => T<unit> ^-> T<int>
            |> WithComment "Add `pluralGetSet` plugin to use `minutes` function"
            "minutes" => T<int>?value ^-> TSelf
            |> WithComment "Add `pluralGetSet` plugin to use `minutes` function"

            "seconds" => T<unit> ^-> T<int>
            |> WithComment "Add `pluralGetSet` plugin to use `seconds` function"
            "seconds" => T<int>?value ^-> TSelf
            |> WithComment "Add `pluralGetSet` plugin to use `seconds` function"

            "milliseconds" => T<unit> ^-> T<int>
            |> WithComment "Add `pluralGetSet` plugin to use `milliseconds` function"
            "milliseconds" => T<int>?value ^-> TSelf
            |> WithComment "Add `pluralGetSet` plugin to use `milliseconds` function"

            // quarterOfYear plugin
            "quarter" => T<unit> ^-> T<int>
            |> WithComment "Add `quarterOfYear` plugin to support `quarter` function"
            "quarter" => T<int>?quarter ^-> TSelf
            |> WithComment "Add `quarterOfYear` plugin to support `quarter` function"

            "add" => T<int>?value * QUnitType?unit ^-> TSelf
            |> WithComment "Add `quarterOfYear` plugin to support `add` function"

            "subtract" => T<int>?value * QUnitType?unit ^-> TSelf
            |> WithComment "Add `quarterOfYear` plugin to support `subtract` function"

            // `isSame`/`isBefore`/`isAfter` accept quarters through their unit
            // argument once `quarterOfYear` is installed (unit types are strings)
            // — no separate overloads are needed.

            // relativeTime plugin
            "fromNow" => !?T<bool>?withoutSuffix ^-> T<string>
            |> WithComment "Add `relativeTime` plugin to use `fromNow` function"

            "from" => ConfigType?compared * !?T<bool>?withoutSuffix ^-> T<string>
            |> WithComment "Add `relativeTime` plugin to use `from` function"

            "toNow" => !?T<bool>?withoutSuffix ^-> T<string>
            |> WithComment "Add `relativeTime` plugin to use `toNow` function"

            "to" => ConfigType?compared * !?T<bool>?withoutSuffix ^-> T<string>
            |> WithComment "Add `relativeTime` plugin to use `to` function"

            // timezone plugin
            "tz" => !?T<string>?timezone * !?T<bool>?keepLocalTime ^-> TSelf
            |> WithComment "Add `timezone` plugin to use `tz` function"

            "offsetName" => !?T<string>?``type`` ^-> !? T<string>
            |> WithComment "Add `timezone` plugin to use `offsetName` function"

            // toArray plugin
            "toArray" => T<unit> ^-> T<int[]>
            |> WithComment "Add `toArray` plugin to use `toArray` function"

            // toObject plugin
            "toObject" => T<unit> ^-> DayjsObject
            |> WithComment "Add `toObject` plugin to use `toObject` function"

            // utc plugin
            "utc" => !?T<bool>?keepLocalTime ^-> TSelf
            |> WithComment "Add `utc` plugin to use `utc` function"

            "local" => T<unit> ^-> TSelf
            |> WithComment "Add `utc` plugin to use `local` function"

            "isUTC" => T<unit> ^-> T<bool>
            |> WithComment "Add `utc` plugin to use `isUTC` function"

            "utcOffset" => (T<int> + T<string>)?offset * !?T<bool>?keepLocalTime ^-> TSelf
            |> WithComment "Add `utc` plugin to use `utcOffset` function"

            // weekOfYear plugin
            "week" => T<unit> ^-> T<int>
            |> WithComment "Add `weekOfYear` plugin to use `week` getter function"
            "week" => T<int>?value ^-> TSelf
            |> WithComment "Add `weekOfYear` plugin to use `week` setter function"

            // weekYear plugin
            "weekYear" => T<unit> ^-> T<int>
            |> WithComment "Add `weekYear` plugin to use `weekYear` function"

            // weekDay plugin
            "weekday" => T<unit> ^-> T<int>
            |> WithComment "Add `weekDay` plugin to use `weekday` getter function"
            "weekday" => T<int>?value ^-> TSelf
            |> WithComment "Add `weekDay` plugin to use `weekday` setter function"
        ]
        |> ignore

    let Assembly =
        Assembly [
            Namespace "WebSharper.DayJs" [
                Plugins
                GlobalLocaleDataReturn
                InstanceLocaleDataReturn
                Duration
                DurationUnitsObjectType
                ObjectSupport
                PluginOptions
                Locale
                LocaleDict
                Locales
                FormatObject
                CalendarFormats
                ConfigTypeMap
                Dayjs
                RelativeTimeKeys
                FormatKeys
                DayjsObject
            ]
        ]

[<Sealed>]
type Extension() =
    interface IExtension with
        member ext.Assembly =
            Definition.Assembly

[<assembly: Extension(typeof<Extension>)>]
do ()