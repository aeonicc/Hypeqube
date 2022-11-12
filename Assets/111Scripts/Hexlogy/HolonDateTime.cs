using System;
//using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

namespace HEXLIBRIUM
{

    public class HolonDateTime : IComparable<HolonDateTime>, IComparable
    {
       

        public const int DAYS_IN_BAKTUN = 144000;
        public const int DAYS_IN_KATUN = 7200;
        public const int DAYS_IN_TUN = 360;
        public const int DAYS_IN_WINAL = 20;
        public const int NUMERALS_IN_TZOLKIN_CYCLE = 13;
        public const int DAYS_IN_TZOLKIN_CYCLE = 20;
        public const int DAYS_IN_HAAB_YEAR = 365;
        public const int DAYS_IN_HAAB_MONTH = 20;

        public static class MayanGregorianCalibration
        {
            public static readonly DateTime GregorianDate = new DateTime(1539, 11, 12);
            public static readonly HolonDateTime MayanDate = new HolonDateTime(11, 16, 0, 0, 0);
        }

        public static readonly HolonDateTime MaxValueDate = new HolonDateTime(19, 19, 19, 17, 19);

        public static readonly HolonDateTime MinValueDate = new HolonDateTime(0, 0, 0, 0, 0);

        public static readonly HolonDateTime NowDate = new HolonDateTime(DateTime.Now);

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

    
        public int Baktun { get; protected set; }
        public int Katun { get; protected set; }
        public int Tun { get; protected set; }
        public int Winal { get; protected set; }
        public int Kin { get; protected set; }
        public int TzolkinNumber { get; protected set; }
        public int TzolkinDay { get; protected set; }

        public TzolkinDayNames TzolkinDayName
        {
            get
            {
                return (TzolkinDayNames)TzolkinDay;
            }
        }

        public int HaabDay { get; protected set; }

        public int HaabMonth { get; protected set; }

        public HaabMonthNames HaabMonthName
        {
            get
            {
                return (HaabMonthNames)HaabMonth;
            }
        }

        public int DaysSinceCreation { get; protected set; }

        public static HolonDateTime ParseDate(string s)
        {
            HolonDateTime holonDateTime;
            if(TryParseDate(s, out holonDateTime))
            {
                return holonDateTime;
            }
            throw new FormatException("The input is not a valid Mayan date representation.");
        }
        public static bool TryParseDate(string s, out HolonDateTime holonDateTime)
        {
            holonDateTime = null;
            if (string.IsNullOrEmpty(s))
            {
                return false;
            }

            // Try to parse the input as a five-digit long count
            var longCountRegex = new Regex(@"(\d{1,2})\.(\d{1,2})\.(\d{1,2})\.(\d{1,2})\.(\d{1,2})");
            var match = longCountRegex.Match(s);
            if (match.Success)
            {
                holonDateTime = new HolonDateTime(int.Parse(match.Groups[1].Value),
                                                  int.Parse(match.Groups[2].Value),
                                                  int.Parse(match.Groups[3].Value),
                                                  int.Parse(match.Groups[4].Value),
                                                  int.Parse(match.Groups[5].Value));
                return true;
            }
            return false;
        }

        public HolonDateTime(int baktun, int katun, int tun, int winal, int kin)
        {
            Init(baktun, katun, tun, winal, kin);
        }

        public HolonDateTime(int daysSinceCreation)
        {
            Init(daysSinceCreation);
        }

        public HolonDateTime(DateTime dateTime)
        {
            //var diff = dateTime - MayanGregorianCalibration.GregorianDate;
            //var daysSinceCreation = (int)diff.TotalDays + MayanGregorianCalibration.MayanDate.DaysSinceCreation;

            //Init(daysSinceCreation);
        }

        /// <summary>
        /// Initializes the public properties.
        /// </summary>
        protected void Init(int daysSinceCreation)
        {
            if (daysSinceCreation < MinValueDate.DaysSinceCreation ||
                daysSinceCreation > MaxValueDate.DaysSinceCreation)
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

        protected void CalculateTzolkinDate(int daysSinceCreation)
        {
       
            TzolkinNumber = (4 + daysSinceCreation) % NUMERALS_IN_TZOLKIN_CYCLE;

            TzolkinDay = ((19 + (daysSinceCreation % DAYS_IN_TZOLKIN_CYCLE)) % DAYS_IN_TZOLKIN_CYCLE) + 1;
        }

        protected void CalculateHaabDate(int daysSinceCreation)
        {
            var daySinceNewYear = (DaysSinceCreation + DAYS_IN_HAAB_YEAR - 17) % DAYS_IN_HAAB_YEAR;
            HaabDay = (daySinceNewYear % DAYS_IN_HAAB_MONTH);
            HaabMonth = (daySinceNewYear / DAYS_IN_HAAB_MONTH) + 1;
        }

        public HolonDateTime AddDays(int days)
        {
            return new HolonDateTime(DaysSinceCreation + days);
        }

        public DateTime ToDateTime()
        {
            var daysSinceJan1st1CE = DaysSinceCreation - MayanGregorianCalibration.MayanDate.DaysSinceCreation;
            return MayanGregorianCalibration.GregorianDate.AddDays(daysSinceJan1st1CE);
        }

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

        public int CompareTo(HolonDateTime other)
        {
            if(other == null)
            {
                return 1;
            }
            return DaysSinceCreation.CompareTo(other.DaysSinceCreation);
        }

        public int CompareTo(object obj)
        {
            var other = obj as HolonDateTime;
            return CompareTo(other);
        }
    }
}
