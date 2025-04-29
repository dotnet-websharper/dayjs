namespace WebSharper.DayJs

open WebSharper
open WebSharper.JavaScript
open WebSharper.InterfaceGenerator

module Definition =

    let Dayjs = 
        Class "Dayjs"
        |> ImportDefault "dayjs"

    let UniTypeFields = [
        // UnitTypeLong
        "millisecond"; "second"; "minute"; "hour"; "day"; "month"; "year"; "date";
        // UnitTypeLongPlural
        "milliseconds"; "seconds"; "minutes"; "hours"; "days"; "months"; "years"; "dates";
        // UnitTypeShort
        "d"; "D"; "M"; "y"; "h"; "m"; "s"; "ms"
    ]

    let UnitType =
        Pattern.EnumStrings "UnitType" UniTypeFields
        |> Import "UnitType" "dayjs"

    let OpUnitType =
        Pattern.EnumStrings "OpUnitType" (
            [ 
                "week"; "weeks"; "w";
            ] @ UniTypeFields
        )
        |> Import "OpUnitType" "dayjs"

    let QUnitType =
        Pattern.EnumStrings "QUnitType" (
            [ 
                "quarter"; "quarters"; "Q" 
            ] @ UniTypeFields
        )
        |> Import "QUnitType" "dayjs"

    let ConfigTypeMap =
        Pattern.Config "FormatObject" {
            Required = []
            Optional = [
                "default", T<string> + T<float> + T<Date> + Dayjs + T<unit>
                "arraySupport", !| (!?T<int>)
                "bigIntSupport", T<BigInt>
            ]
        }

    let ConfigType = T<string>

    let FormatObject =
        Pattern.Config "FormatObject" {
            Required = []
            Optional = [
                "locale", T<string>
                "format", T<string>
                "utc", T<bool>
            ]
        }
        |> Import "FormatObject" "dayjs"

    let OptionType = FormatObject+ T<string> + !|T<string>

    let Locale =
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

        let RelativeTimeKeys =
            Pattern.Config "RelativeTimeKeys" {
                Required = []
                Optional = [
                    "future", T<string>
                    "past", T<string>
                    "s", T<string>
                    "m", T<string>
                    "mm", T<string>
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
        |> Import "Locale" "dayjs"

    let ManipulateType =
        Pattern.EnumStrings "ManipulateType" [
            "millisecond"; "second"; "minute"; "hour"; "day"; "month"; "year";
            "milliseconds"; "seconds"; "minutes"; "hours"; "days"; "months"; "years";
            "d"; "D"; "M"; "y"; "h"; "m"; "s"; "ms"
        ]
        |> Import "ManipulateType" "dayjs"

    let PluginFunc = T<obj>?option * !?T<obj>?c * !?T<obj>?d ^-> T<unit>

    let PluginOptions =
        Pattern.Config "PluginOptions" {
            Required = []
            Optional = [
                "parseTwoDigitYear", T<string>?yearString ^-> T<int>
            ]
        }

    let DurationUnitsObjectType = 
        Pattern.Config "DurationUnit" {
            Required = []
            Optional = [
                "milliseconds", T<int>
                "seconds", T<int> 
                "minutes", T<int> 
                "hours", T<int> 
                "days", T<int> 
                "months", T<int>
                "years", T<int>
            ]
        }

    let DurationUnitType = 
        Pattern.EnumStrings "DurationUnitType" [ 
            "week"; "weeks"; "w";
            "millisecond"; "second"; "minute"; "hour"; "day"; "month"; "year"; 
            "milliseconds"; "seconds"; "minutes"; "hours"; "days"; "months"; "years";
            "d"; "D"; "M"; "y"; "h"; "m"; "s"; "ms"
        ] 

    let Duration =
        Class "Duration" 

    let CreateDurationType = 
        (DurationUnitsObjectType?units ^-> Duration) +
        (T<int>?time * DurationUnitType?unit ^-> Duration) +
        (T<string>?ISO_8601 ^-> Duration)

    let AddDurationType = 
        CreateDurationType +
        (Duration?duration ^-> Duration)

    let ISOUnitType = 
        Pattern.EnumStrings "ISOUnitType" (
            [ 
                "week"; "weeks"; "w"; "isoWeek"
            ] @ UniTypeFields
        )

    let WeekdayNames = T<string>
    let MonthNames = T<string>

    let InstanceLocaleDataReturn =
        Pattern.Config "InstanceLocaleDataReturn" {
            Required = []
            Optional = [
                "firstDayOfWeek", T<unit> ^-> T<int>
                "weekdays", !?Dayjs?instance ^-> WeekdayNames
                "weekdaysShort", !?Dayjs?instance ^-> WeekdayNames
                "weekdaysMin", !?Dayjs?instance ^-> WeekdayNames
                "months", !?Dayjs?instance ^-> MonthNames
                "monthsShort", !?Dayjs?instance ^-> T<obj>
                "longDateFormat", T<string>?format ^-> T<string>
                "meridiem", !?T<int>?hour * !?T<int>?minute * !?T<bool>?isLower ^-> T<string>
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
                "meridiem", !?T<int>?hour * !?T<int>?minute * !?T<bool>?isLower ^-> T<string>
                "ordinal", T<int>?n ^-> T<string>
            ]
        }

    let Plugins = 
        Class "Plugins"
        |+> Instance [
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
            Constructor ((T<string> + T<int> + T<obj>)?input * !?T<string>?unit * !?T<string>?locale)
        ]
        |+> Pattern.OptionalFields [
            "clone", T<unit> ^-> TSelf
            "humanize", !?T<bool>?withSuffix ^-> T<string>
            "milliseconds", T<unit> ^-> T<int>
            "asMilliseconds", T<unit> ^-> T<float>
            "seconds", T<unit> ^-> T<int>
            "asSeconds", T<unit> ^-> T<float>
            "minutes", T<unit> ^-> T<int>
            "asMinutes", T<unit> ^-> T<float>
            "hours", T<unit> ^-> T<int>
            "asHours", T<unit> ^-> T<float>
            "days", T<unit> ^-> T<int>
            "asDays", T<unit> ^-> T<float>
            "weeks", T<unit> ^-> T<int>
            "asWeeks", T<unit> ^-> T<float>
            "months", T<unit> ^-> T<int>
            "asMonths", T<unit> ^-> T<float>
            "years", T<unit> ^-> T<int>
            "asYears", T<unit> ^-> T<float>

            "as", DurationUnitType?unit ^-> T<float>
            "get", DurationUnitType?unit ^-> T<int>

            "add", AddDurationType
            "subtract", AddDurationType

            "toJSON", T<unit> ^-> T<string>
            "toISOString", T<unit> ^-> T<string>
            "format", !?T<string>?formatStr ^-> T<string>
            "locale", T<string>?locale ^-> TSelf
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

            "extend" => PluginFunc?plugin * !? T<obj>?options ^-> TSelf
            "locale" => !?(T<string> + Locale)?preset * !?Locale?object * T<bool>?isLocale ^-> T<string>
            "isDayjs" => T<obj>?d ^-> TSelf
            "unix" => (T<float> + T<BigInt>)?t ^-> TSelf

            // duration plugin
            "duration" =? CreateDurationType
            |> WithComment "Add `duration` plugin to use `duration` function"
            "isDuration" => T<obj>?d ^-> Duration
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

        ]
        |+> Instance [
            "clone" => T<unit> ^-> TSelf
            "isValid" => T<unit> ^-> T<bool>

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
            "get" => UnitType?unitName ^-> T<int>

            "add" => (T<float>?value * !?ManipulateType?unit) ^-> TSelf
            "subtract" => (T<float>?value * !?UnitType?unit) ^-> TSelf
            "startOf" => OpUnitType?unit ^-> TSelf
            "endOf" => OpUnitType?unit ^-> TSelf

            "format" => !?T<string>?template ^-> T<string>

            "diff" => (!?ConfigType?date * !? (QUnitType + OpUnitType)?unit * !?T<bool>?float) ^-> T<float>

            "valueOf" => T<unit> ^-> T<float>
            "unix" => T<unit> ^-> T<float>
            "daysInMonth" => T<unit> ^-> T<int>
            "toDate" => T<unit> ^-> T<Date>
            "toJSON" => T<unit> ^-> T<string>
            "toISOString" => T<unit> ^-> T<string>
            "toString" => T<unit> ^-> T<string>

            "utcOffset" => T<unit> ^-> T<int>

            "isBefore" => (!?ConfigType?date * !?OpUnitType?unit) ^-> T<bool>
            "isSame" => (!?ConfigType?date * !?OpUnitType?unit) ^-> T<bool>
            "isAfter" => (!?ConfigType?date * !?OpUnitType?unit) ^-> T<bool>

            "locale" => T<unit> ^-> T<string>
            "locale" => (T<string> + Locale)?preset * !?Locale?object ^-> TSelf

            // calendar plugin
            "calendar" => !?ConfigType?referenceTime * T<obj>?formats ^-> T<string>
            |> WithComment "Add `calendar` plugin to use `calendar` function"

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

            "startOf" => ISOUnitType?unit ^-> TSelf
            |> WithComment "Add `isoWeek` plugin to use `startOf` function"

            "endOf" => ISOUnitType?unit ^-> TSelf
            |> WithComment "Add `isoWeek` plugin to use `endOf` function"

            "isSame" => !?T<obj>?date * !?ISOUnitType?unit ^-> T<bool>
            |> WithComment "Add `isoWeek` plugin to use `isSame` function"

            "isBefore" => !?T<obj>?date * !?ISOUnitType?unit ^-> T<bool>
            |> WithComment "Add `isoWeek` plugin to use `isBefore` function"

            "isAfter" => !?T<obj>?date * !?ISOUnitType?unit ^-> T<bool>
            |> WithComment "Add `isoWeek` plugin to use `isAfter` function"

            // isoWeeksInYear plugin
            "isoWeeksInYear" => T<unit> ^-> T<int>
            |> WithComment "Add `isoWeeksInYear` plugin to use `isoWeeksInYear` function"

            // localeData plugin
            "localeData" => T<unit> ^-> InstanceLocaleDataReturn
            |> WithComment "Add `localeData` plugin to use `localeData` function"
        ]
        |> ignore

    let Assembly =
        Assembly [
            Namespace "WebSharper.DayJs" [
            ]
        ]

[<Sealed>]
type Extension() =
    interface IExtension with
        member ext.Assembly =
            Definition.Assembly

[<assembly: Extension(typeof<Extension>)>]
do ()
