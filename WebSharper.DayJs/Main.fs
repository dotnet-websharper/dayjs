namespace WebSharper.DayJs

open WebSharper
open WebSharper.JavaScript
open WebSharper.InterfaceGenerator

module Definition =

    let Dayjs = 
        Class "Dayjs"
        |> ImportDefault "dayjs"

    let UnitType = T<string>

    let OpUnitType = T<string>

    let QUnitType = T<string>

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
                //"d", T<string> + T<int>

                "dates", T<string> + T<int>
                "date", T<string> + T<int>
                "D", T<string> + T<int>

                "hours", T<string> + T<int>
                "hour", T<string> + T<int>
                "h", T<string> + T<int>

                "minutes", T<string> + T<int>
                "minute", T<string> + T<int>
                "m", T<string> + T<int>

                "seconds", T<string> + T<int>
                "second", T<string> + T<int>
                "s", T<string> + T<int>

                "milliseconds", T<string> + T<int>
                "millisecond", T<string> + T<int>
                "ms", T<string> + T<int>
            ]
        }


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
                // "M" is missing here
                // "MM" is missing here
                "y", T<string>
                "yy", T<string>
            ]
        }

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
                "relativeTime", RelativeTimeKeys.Type + T<obj>
            ]
        }
        |> Import "Locale" "dayjs"

    let ManipulateType = T<string>

    let PluginFunc = T<obj>

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

    let DurationUnitType = T<string>

    let Duration =
        Class "Duration" 

    let CreateDurationType = 
        (DurationUnitsObjectType?units ^-> Duration) +
        (T<int>?time * DurationUnitType?unit ^-> Duration) +
        (T<string>?ISO_8601 ^-> Duration)

    let AddDurationType = 
        CreateDurationType +
        (Duration?duration ^-> Duration)

    let ISOUnitType = T<string>

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

    let RelativeTimeThreshold =
        Pattern.Config "RelativeTimeThreshold" {
            Required = []
            Optional = [
                "l", T<string>
                "r", T<int>
                "d", T<string>
            ]
        }

    let RelativeTimeOptions =
        Pattern.Config "RelativeTimeOptions" {
            Required = []
            Optional = [
                "rounding", T<int>?num ^-> T<int>
                "thresholds", !| RelativeTimeThreshold.Type
            ]
        }

    let DayjsTimezone =
        Class "DayjsTimezone"
        |+> Static [
            Constructor (!?ConfigType?date * !?T<string>?format * !?T<string>?timezone ^-> Dayjs)
        ]
        |+> Pattern.OptionalFields [            
            "guess", T<unit> ^-> T<string>
            "setDefault", T<string>?timezone ^-> T<unit>
        ]

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

            // minMax plugin
            "max" => (!| (!|Dayjs))?dayjs ^-> Dayjs
            |> WithComment "Add `minMax` plugin to use `max` function"
            "max" => (!| T<obj>)?noDates ^-> T<unit>
            |> WithComment "Add `minMax` plugin to use `max` function"
            "max" =>  (!|Dayjs)?maybeDates ^-> Dayjs
            |> WithComment "Add `minMax` plugin to use `max` function"

            "min" => (!| (!|Dayjs))?dayjs ^-> Dayjs
            |> WithComment "Add `minMax` plugin to use `min` function"
            "min" => (!| T<obj>)?noDates ^-> T<unit>
            |> WithComment "Add `minMax` plugin to use `min` function"
            "min" =>  (!|Dayjs)?maybeDates ^-> Dayjs
            |> WithComment "Add `minMax` plugin to use `min` function"

            // timezone plugin
            "tz" =@ DayjsTimezone
            |> WithComment "Add `timezone` plugin to use `tz` function"

            // updateLocale plugin
            "updateLocale" => T<string>?localeName * T<obj>?customConfig ^-> T<obj>
            |> WithComment "Add `updateLocale` plugin to use `updateLocale` function"
            // utc plugin
            "utc" => !?ConfigType?config * !?T<string>?format * !?T<bool>?strict ^-> TSelf
            |> WithSourceName "UtcStatic"
            |> WithComment "Add `utc` plugin to use `utc` function"
            // need fix the static and instance name crashes
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
            "startOf" => (OpUnitType + ISOUnitType + QUnitType)?unit ^-> TSelf
            "endOf" => (OpUnitType + ISOUnitType + QUnitType)?unit ^-> TSelf

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

            "isBefore" => (!?ConfigType + T<obj>)?date * !?(OpUnitType + ISOUnitType)?unit ^-> T<bool>
            "isSame" => (!?ConfigType + T<obj>)?date * !?(OpUnitType + ISOUnitType)?unit ^-> T<bool>
            "isAfter" => (!?ConfigType + T<obj>)?date * !?(OpUnitType + ISOUnitType)?unit ^-> T<bool>

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

            // isoWeeksInYear plugin
            "isoWeeksInYear" => T<unit> ^-> T<int>
            |> WithComment "Add `isoWeeksInYear` plugin to use `isoWeeksInYear` function"

            // localeData plugin
            "localeData" => T<unit> ^-> InstanceLocaleDataReturn
            |> WithComment "Add `localeData` plugin to use `localeData` function"

            // objectSupport methods
            "set" => T<obj>?argument ^-> Dayjs
            |> WithComment "Add `objectSupport` plugin to use `set` function"

            "add" => T<obj>?argument ^-> Dayjs
            |> WithComment "Add `objectSupport` plugin to use `add` function"

            "subtract" => T<obj>?argument ^-> Dayjs
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

            "isSame" => ConfigType?date * QUnitType?unit ^-> T<bool>
            |> WithComment "Add `quarterOfYear` plugin to support `isSame` function"

            "isBefore" => ConfigType?date * QUnitType?unit ^-> T<bool>
            |> WithComment "Add `quarterOfYear` plugin to support `isBefore` function"

            "isAfter" => ConfigType?date * QUnitType?unit ^-> T<bool>
            |> WithComment "Add `quarterOfYear` plugin to support `isAfter` function"

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

            "offsetName" => !?T<string>?``type`` ^-> T<string>
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
                PluginOptions
                Locale
                FormatObject
                ConfigTypeMap
                Dayjs
                RelativeTimeKeys
                FormatKeys
                DayjsTimezone
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
