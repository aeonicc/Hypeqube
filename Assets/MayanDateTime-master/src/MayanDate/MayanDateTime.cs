#region License
/**
 * Copyright (c) Steven Nichols
 * All rights reserved.
 *
 * MIT License
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this
 * software and associated documentation files (the "Software"), to deal in the Software
 * without restriction, including without limitation the rights to use, copy, modify, merge,
 * publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
 * to whom the Software is furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all copies or
 * substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED *AS IS*, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
 * PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
 * FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
 * OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
 * DEALINGS IN THE SOFTWARE.
 */
#endregion

using System;
//using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

namespace MayanDate
{
    /// <summary>
    /// Used to compute dates in the ancient Mayan calendar system. Dates can be tracked in the 5 digit
    /// Long Count system, the 260-day Tzolk'in cycle and/or the 365-day Haab' cycle.
    /// </summary>
    public class MayanDateTime : IComparable<MayanDateTime>, IComparable
    {
        #region Constants

        public const int DAYS_IN_BAKTUN = 144000;

        public const int DAYS_IN_KATUN = 7200;

        public const int DAYS_IN_TUN = 360;

        public const int DAYS_IN_WINAL = 20;

        public const int NUMERALS_IN_TZOLKIN_CYCLE = 13;

        public const int DAYS_IN_TZOLKIN_CYCLE = 20;

        public const int DAYS_IN_HAAB_YEAR = 365;

        public const int DAYS_IN_HAAB_MONTH = 20;

        #endregion

        #region Static Read-onlys

        /// <summary>
        /// The reference date used to calibrate the Mayan long count date to the Gregorian calendar.
        /// The standard correlation (GMT) assumes that 11.16.0.0.0 corresponds to Nov 12, 1539.
        /// </summary>
        public static class MayanGregorianCalibrationDate
        {
            public static readonly DateTime Gregorian = new DateTime(1539, 11, 12);
            public static readonly MayanDateTime Mayan = new MayanDateTime(11, 16, 0, 0, 0);
        }

        /// <summary>
        /// Represents the largest possible value of MayanDateTime.
        /// </summary>
        public static readonly MayanDateTime MaxValue = new MayanDateTime(19, 19, 19, 17, 19);

        /// <summary>
        /// Represents the smallest possible value of MayanDateTime.
        /// </summary>
        public static readonly MayanDateTime MinValue = new MayanDateTime(0, 0, 0, 0, 0);

        /// <summary>
        /// Represents the current date, expressed as a MayanDateTime instance.
        /// </summary>
        public static readonly MayanDateTime Now = new MayanDateTime(DateTime.Now);

        #endregion

        #region Enums

        /// <summary>
        /// The twenty named days of the Tzolk'in cycle.
        /// </summary>
        public enum TzolkinDayNames
        {
            Imix = 1,
            Ik,
            Akbal,
            Kan,
            Chikchan,
            Kimi,
            Manik,
            Lamat,
            Muluk,
            Ok,
            Chuwen,
            Eb,
            Ben,
            Ix,
            Men,
            Kib,
            Kaban,
            Etznab,
            Kawak,
            Ajaw
        };

        /// <summary>
        /// The 18 months of the Habb' cycle.
        /// </summary>
        public enum HaabMonthNames
        {
           
            Pop = 1,
            Wo,
            Sip,
            Sotz,
            Sek,
            Xul,
            Yaxkin,
            Mol,
            Chen,
            Yax,
            Sak,
            Keh,
            Mak,
            Kankin,
            Muwan,
            Pax,
            Kayab,
            Kumku,
            Wayeb
        };

        #endregion

        #region Public Properties

        /// <summary>
        /// The b'ak'tun is the first digit of the long count date.
        /// Range 1 to 20.
        /// </summary>
        public int Baktun { get; protected set; }

        /// <summary>
        /// The k'atun is the second digit of the long count date.
        /// Range 1 to 20.
        /// </summary>
        public int Katun { get; protected set; }

        /// <summary>
        /// The tun is the third digit of the long count date.
        /// Range 1 to 20.
        /// </summary>
        public int Tun { get; protected set; }

        /// <summary>
        /// The winal is the fourth digit of the long count date.
        /// Range 1 to 18.
        /// </summary>
        public int Winal { get; protected set; }

        /// <summary>
        /// The k'in is the fifth digit of the long count date.
        /// </summary>
        public int Kin { get; protected set; }


        /// <summary>
        /// The numerical portion of Tzolk'in date.
        /// Range 1 to 13.
        /// </summary>
        public int TzolkinNumber { get; protected set; }

        /// <summary>
        /// The sequence number of the Tzolk'in day.
        /// Range 1 to 20.
        /// </summary>
        public int TzolkinDay { get; protected set; }

        /// <summary>
        /// Returns the TzolkinDayName enum value corresponding to the Tzolk'in Day.
        /// </summary>
        public TzolkinDayNames TzolkinDayName
        {
            get
            {
                return (TzolkinDayNames)TzolkinDay;
            }
        }


        /// <summary>
        /// The sequence number of the Haab' day.
        /// Range 1 to 20.
        /// </summary>
        public int HaabDay { get; protected set; }

        /// <summary>
        /// The sequence number of the Haab' month.
        /// Range 1 to 19 (18 months + the 5 "nameless" Wayeb' days).
        /// </summary>
        public int HaabMonth { get; protected set; }

        /// <summary>
        /// The HaabMonthNames enum value corresponding to the Haab' month.
        /// </summary>
        public HaabMonthNames HaabMonthName
        {
            get
            {
                return (HaabMonthNames)HaabMonth;
            }
        }

        /// <summary>
        /// Returns the number of days since 0.0.0.0.0, 4 Ajaw 8 Kumk'u (August 11, 3114 BCE).
        /// </summary>
        public int DaysSinceCreation { get; protected set; }

        #endregion

        /// <summary>
        /// Converts the string representation of a date and time to its MayanDateTime equivalent.
        /// Valid formats are the five-digit long count, e.g., "7.14.2.9.3".
        /// </summary>
        /// <param name="s">A string that contains a date to convert.</param>
        /// <returns>The converted MayanDateTime.</returns>
        /// <exception cref="FormatException">Thrown if the input is not a valid Mayan date representation.</exception>
        public static MayanDateTime Parse(string s)
        {
            MayanDateTime mayanDateTime;
            if(TryParse(s, out mayanDateTime))
            {
                return mayanDateTime;
            }
            throw new FormatException("The input is not a valid Mayan date representation.");
        }

        /// <summary>
        /// Tries to converts the string representation of a date and time to its MayanDateTime equivalent.
        /// Valid formats are the five-digit long count, e.g., "7.14.2.9.3".
        /// </summary>
        /// <param name="s">A string that contains a date to convert.</param>
        /// <param name="mayanDateTime">The converted MayanDateTime.</param>
        /// <returns>A value indicating whether the input was parsed successfully.</returns>
        public static bool TryParse(string s, out MayanDateTime mayanDateTime)
        {
            mayanDateTime = null;
            if (string.IsNullOrEmpty(s))
            {
                return false;
            }

            // Try to parse the input as a five-digit long count
            var longCountRegex = new Regex(@"(\d{1,2})\.(\d{1,2})\.(\d{1,2})\.(\d{1,2})\.(\d{1,2})");
            var match = longCountRegex.Match(s);
            if (match.Success)
            {
                mayanDateTime = new MayanDateTime(int.Parse(match.Groups[1].Value),
                                                  int.Parse(match.Groups[2].Value),
                                                  int.Parse(match.Groups[3].Value),
                                                  int.Parse(match.Groups[4].Value),
                                                  int.Parse(match.Groups[5].Value));
                return true;
            }
            return false;
        }

        /// <summary>
        /// Initializes a new instance of the MayanDateTime structure to the
        /// specified Mayan Long Count date.
        /// </summary>
        /// <param name="baktun">The B'ak'tun (0 through 19)</param>
        /// <param name="katun">The K'atun (0 through 19)</param>
        /// <param name="tun">The Tun (0 through 19)</param>
        /// <param name="winal">The Winal (0 through 17)</param>
        /// <param name="kin">The Kin (0 through 19)</param>
        public MayanDateTime(int baktun, int katun, int tun, int winal, int kin)
        {
            Init(baktun, katun, tun, winal, kin);
        }

        /// <summary>
        /// Initializes a new instance of the MayanDateTime object to the
        /// date specified by the number of days since the mythological creation date.
        /// </summary>
        /// <param name="daysSinceCreation">The number of days since 0.0.0.0.0, 4 Ajaw 8 Kumk'u (August 11, 3114 BCE).</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Throws if daysSinceCreation is negative or greater than 2,879,999 (19.19.19.17.19).
        /// </exception>
        public MayanDateTime(int daysSinceCreation)
        {
            Init(daysSinceCreation);
        }

        /// <summary>
        /// Initializes a new instance of the MayanDateTime structure to the
        /// specified DateTime.
        /// </summary>
        public MayanDateTime(DateTime dateTime)
        {
            var diff = dateTime - MayanGregorianCalibrationDate.Gregorian;
            var daysSinceCreation = (int)diff.TotalDays + MayanGregorianCalibrationDate.Mayan.DaysSinceCreation;

            Init(daysSinceCreation);
        }

        /// <summary>
        /// Initializes the public properties.
        /// </summary>
        protected void Init(int daysSinceCreation)
        {
            if (daysSinceCreation < MinValue.DaysSinceCreation ||
                daysSinceCreation > MaxValue.DaysSinceCreation)
            {
                throw new ArgumentOutOfRangeException(nameof(daysSinceCreation));
            }

            var remainingDays = daysSinceCreation;

            var baktun = (remainingDays / DAYS_IN_BAKTUN);
            remainingDays %= DAYS_IN_BAKTUN;

            var katun = (remainingDays / DAYS_IN_KATUN);
            remainingDays %= DAYS_IN_KATUN;

            var tun = (remainingDays / DAYS_IN_TUN);
            remainingDays %= DAYS_IN_TUN;

            var winal = (remainingDays / DAYS_IN_WINAL);
            remainingDays %= DAYS_IN_WINAL;

            var kin = remainingDays;

            Baktun = baktun;
            Katun = katun;
            Tun = tun;
            Winal = winal;
            Kin = kin;

            DaysSinceCreation = daysSinceCreation;

            CalculateTzolkinDate(DaysSinceCreation);
            CalculateHaabDate(DaysSinceCreation);
        }

        /// <summary>
        /// Initializes the public properties.
        /// </summary>
        protected void Init(int baktun, int katun, int tun, int winal, int kin)
        {
            if (baktun < 0 || baktun > 19)
            {
                throw new ArgumentOutOfRangeException(nameof(baktun));
            }
            if (katun < 0 || katun > 19)
            {
                throw new ArgumentOutOfRangeException(nameof(katun));
            }
            if (tun < 0 || tun > 19)
            {
                throw new ArgumentOutOfRangeException(nameof(tun));
            }
            if (winal < 0 || winal > 17)
            {
                throw new ArgumentOutOfRangeException(nameof(winal));
            }
            if (kin < 0 || kin > 19)
            {
                throw new ArgumentOutOfRangeException(nameof(kin));
            }

            Baktun = baktun;
            Katun = katun;
            Tun = tun;
            Winal = winal;
            Kin = kin;

            DaysSinceCreation = Baktun * DAYS_IN_BAKTUN + Katun * DAYS_IN_KATUN + Tun * DAYS_IN_TUN + Winal * DAYS_IN_WINAL + Kin;

            CalculateTzolkinDate(DaysSinceCreation);
            CalculateHaabDate(DaysSinceCreation);
        }

        /// <summary>
        /// Computes the Tzolk'in date which is the combination of one of twenty named
        /// days and one of 13 numbers which produce 260 (20 x 13) unique days.
        /// </summary>
        protected void CalculateTzolkinDate(int daysSinceCreation)
        {
            //***
            // Tzolk'in date is counted forward from the day of creation which was 4 Ajaw.
            //***
            TzolkinNumber = (4 + daysSinceCreation) % NUMERALS_IN_TZOLKIN_CYCLE;

            //***
            // The calculation is performed using modulo arithmetic (0-19) which is then
            // incremented by 1 to change the range to 1-20. Counting starts from 4 Ajaw
            // which is the last named day of the cycle.
            //***
            TzolkinDay = ((19 + (daysSinceCreation % DAYS_IN_TZOLKIN_CYCLE)) % DAYS_IN_TZOLKIN_CYCLE) + 1;
        }

        /// <summary>
        /// Computes the Haab' date which consists of 18 months of 20 days each plus
        /// a period of 5 "nameless" days to produce a 365 day long year.
        /// </summary>
        /// <param name="daysSinceCreation">the number of days since 0.0.0.0.0 4 Ajaw 8 Kumk'u</param>
        protected void CalculateHaabDate(int daysSinceCreation)
        {
            //***
            // Haab' starts from 8 Kumk'u which is the 9th day of the 18th month.
            // There are 17 days to the start of the next year.
            //***
            var daySinceNewYear = (DaysSinceCreation + DAYS_IN_HAAB_YEAR - 17) % DAYS_IN_HAAB_YEAR;
            HaabDay = (daySinceNewYear % DAYS_IN_HAAB_MONTH);
            HaabMonth = (daySinceNewYear / DAYS_IN_HAAB_MONTH) + 1;
        }

        /// <summary>
        /// Returns a new MayanDateTime that adds the specified number of days to the value of this instance.
        /// </summary>
        /// <param name="days">Can be positive or negative</param>
        public MayanDateTime AddDays(int days)
        {
            return new MayanDateTime(DaysSinceCreation + days);
        }

        /// <summary>
        /// Converts this MayanDateTime to a DateTime object. DateTime doesn't support dates before 1 AD
        /// so it will throw an ArgumentOutOfRangeException for dates before 7.17.18.13.3.
        /// </summary>
        public DateTime ToDateTime()
        {
            var daysSinceJan1st1CE = DaysSinceCreation - MayanGregorianCalibrationDate.Mayan.DaysSinceCreation;
            return MayanGregorianCalibrationDate.Gregorian.AddDays(daysSinceJan1st1CE);
        }

        /// <summary>
        /// Returns a string representation of the long count, Tzolk'in and Haab' values.
        /// </summary>
        // public override string ToString()
        // {
        //     return string.Format("{0}.{1}.{2}.{3}.{4}, {5} {6} {7} {8}",
        //         Baktun, Katun, Tun, Winal, Kin,
        //         //TzolkinNumber, TzolkinDayName.GetAttribute<DisplayAttribute>()?.Name ?? TzolkinDayName.ToString(),
        //         //HaabDay, HaabMonthName.GetAttribute<DisplayAttribute>()?.Name ?? HaabMonthName.ToString());
        // }

        /// <summary>
        /// Returns a custom string representation specified by the format string parameter.
        /// </summary>
        public string ToString(string formatStr)
        {
            StringBuilder sb = new StringBuilder();

            if (formatStr == null)
            {
                return string.Empty;
            }

            for(int i = 0; i < formatStr.Length; i++)
            {
                char token = formatStr[i];
                if (token == '%' && ++i < formatStr.Length)
                {
                    switch (formatStr[i])
                    {
                        case '%':
                            // Escaped percent sign
                            sb.Append('%');
                            break;
                        case 'c':
                            // Long count days since creation
                            sb.Append(DaysSinceCreation);
                            break;
                        case 'L':
                            // Formatted long count
                            sb.AppendFormat("{0}.{1}.{2}.{3}.{4}", Baktun, Katun, Tun, Winal, Kin);
                            break;
                        case 'b':
                            // Baktun number
                            sb.Append(Baktun);
                            break;
                        case 'k':
                            // Katun number
                            sb.Append(Katun);
                            break;
                        case 'u':
                            // Tun number
                            sb.Append(Tun);
                            break;
                        case 'w':
                            // Winal number
                            sb.Append(Winal);
                            break;
                        case 'i':
                            // Kin number
                            sb.Append(Kin);
                            break;
                        case 't':
                            // Tzolkin number
                            sb.Append(TzolkinNumber);
                            break;
                        case 'T':
                            // Tzolkin day
                            sb.Append(TzolkinDay);
                            break;
                        case 'D':
                            // Tzolkin day name
                            //sb.Append(TzolkinDayName.GetAttribute<DisplayAttribute>().Name);
                            break;
                        case 'h':
                            // Haab day number
                            sb.Append(HaabDay);
                            break;
                        case 'H':
                            // Haab month
                            sb.Append(HaabMonth);
                            break;
                        case 'M':
                            // Haab month name
                            //sb.Append(HaabMonthName.GetAttribute<DisplayAttribute>().Name);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    sb.Append(token);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Compares the value of this instance to a specified MayanDateTime value and indicates whether
        /// this instance is earlier than, the same as, or later than the specified MayanDateTime value.
        /// </summary>
        public int CompareTo(MayanDateTime other)
        {
            if(other == null)
            {
                return 1;
            }
            return DaysSinceCreation.CompareTo(other.DaysSinceCreation);
        }

        /// <summary>
        /// Compares this instance to the specified object and returns an indication of their relative values.
        /// </summary>
        public int CompareTo(object obj)
        {
            var other = obj as MayanDateTime;
            return CompareTo(other);
        }
    }
}
