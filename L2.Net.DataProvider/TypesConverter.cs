using System;

namespace L2.Net.DataProvider
{
    /// <summary>
    /// Provides methods for object-to-type conversions, used on database operations.
    /// </summary>
    public static class TypesConverter
    {
        /// <summary>
        /// Converts <see cref="object"/> to <see cref="byte"/> value.
        /// </summary>
        /// <param name="obj"><see cref="object"/> to convert.</param>
        /// <returns><see cref="byte"/> value if <see cref="object"/> is convertible, otherwise 0.</returns>
        public static byte GetByte( object obj )
        {
            return GetByte(obj, 0);
        }

        /// <summary>
        /// Converts <see cref="object"/> to <see cref="byte"/> value.
        /// </summary>
        /// <param name="obj"><see cref="object"/> to convert.</param>
        /// <param name="def"><see cref="byte"/> value to return if provided <see cref="object"/> is not convertible.</param>
        /// <returns><see cref="byte"/> value if provided <see cref="object"/> is convertible, otherwise <paramref name="def" /> value.</returns>
        public static byte GetByte( object obj, byte def )
        {
            if ( !( obj == null || obj is DBNull ) )
                Byte.TryParse(obj.ToString(), out def);
            return def;
        }

        /// <summary>
        /// Converts <see cref="object"/> to <see cref="sbyte"/> value.
        /// </summary>
        /// <param name="obj"><see cref="object"/> to convert.</param>
        /// <returns><see cref="sbyte"/> value if provided <see cref="object"/> is convertible, otherwise 0.</returns>
        public static sbyte GetSByte( object obj )
        {
            return GetSByte(obj, 0);
        }

        /// <summary>
        /// Converts <see cref="object"/> to <see cref="sbyte"/> value.
        /// </summary>
        /// <param name="obj"><see cref="object"/> to convert.</param>
        /// <param name="def"><see cref="sbyte"/> value to return if provided <see cref="object"/> is not convertible.</param>
        /// <returns><see cref="sbyte"/> value if provided <see cref="object"/> is convertible, otherwise <paramref name="def"/> value.</returns>
        public static sbyte GetSByte( object obj, sbyte def )
        {
            if ( obj != null && !( obj is DBNull ) )
                SByte.TryParse(obj.ToString(), out def);
            return def;
        }

        /// <summary>
        /// Converts provided <see cref="object"/> to <see cref="short"/> value.
        /// </summary>
        /// <param name="obj"><see cref="object"/> to convert.</param>
        /// <returns><see cref="short"/> value if provided <see cref="object"/> is convertible, otherwise 0.</returns>
        public static short GetShort( object obj )
        {
            return GetShort(obj, 0);
        }

        /// <summary>
        /// Converts provided <see cref="object"/> to <see cref="short"/> value.
        /// </summary>
        /// <param name="obj"><see cref="object"/> to convert.</param>
        /// <param name="def">Default <see cref="short"/> value to return if provided <see cref="object"/> is not convertible.</param>
        /// <returns><see cref="short"/> value if provided <see cref="object"/> is convertible, otherwise <paramref name="def"/> value.</returns>
        public static short GetShort( object obj, short def )
        {
            if ( obj != null && !( obj is DBNull ) )
                short.TryParse(obj.ToString(), out def);
            return def;
        }

        /// <summary>
        /// Converts provided <see cref="object"/> to <see cref="ushort"/> value.
        /// </summary>
        /// <param name="obj"><see cref="object"/> to convert.</param>
        /// <returns><see cref="ushort"/> value if provided <see cref="object"/> is convertible, otherwise 0.</returns>
        public static ushort GetUShort( object obj )
        {
            return GetUShort(obj, 0);
        }

        /// <summary>
        /// Converts provided <see cref="object"/> to <see cref="ushort"/> value.
        /// </summary>
        /// <param name="obj"><see cref="object"/> to convert.</param>
        /// <param name="def"><see cref="ushort"/> value to return if provided <see cref="object"/> is not convertible.</param>
        /// <returns><see cref="ushort"/> value if provided <see cref="object"/> is convertible, otherwise <paramref name="def"/> value.</returns>
        public static ushort GetUShort( object obj, ushort def )
        {
            if ( obj != null && !( obj is DBNull ) )
                ushort.TryParse(obj.ToString(), out def);
            return def;
        }

        /// <summary>
        /// Converts <see cref="object"/> to <see cref="int"/> value.
        /// </summary>
        /// <param name="obj"><see cref="object"/> to convert.</param>
        /// <returns><see cref="int"/> value if provided <see cref="object"/> is convertible, otherwise 0.</returns>
        public static int GetInt( object obj )
        {
            return GetInt(obj, 0);
        }

        /// <summary>
        /// Converts provided <see cref="object"/> to <see cref="int"/> value.
        /// </summary>
        /// <param name="obj"><see cref="object"/> to convert.</param>
        /// <param name="def"><see cref="int"/> value to return if provided <see cref="object"/> is not convertible.</param>
        /// <returns><see cref="int"/> value if provided <see cref="object"/> is convertible, otherwise <paramref name="def"/> value.</returns>
        public static int GetInt( object obj, int def )
        {
            if ( obj != null && !( obj is DBNull ) )
                Int32.TryParse(obj.ToString(), out def);
            return def;
        }

        /// <summary>
        /// Converts <see cref="object"/> to <see cref="uint"/> value.
        /// </summary>
        /// <param name="obj"><see cref="object"/> to convert.</param>
        /// <returns><see cref="uint"/> value if provided <see cref="object"/> is convertible, otherwise 0.</returns>
        public static uint GetUInt( object obj )
        {
            return GetUInt(obj, 0);
        }

        /// <summary>
        /// Converts provided <see cref="object"/> to <see cref="uint"/> value.
        /// </summary>
        /// <param name="obj"><see cref="object"/> to convert.</param>
        /// <param name="def"><see cref="uint"/> value to return if provided <see cref="object"/> is not convertible.</param>
        /// <returns><see cref="uint"/> value if provided <see cref="object"/> is convertible, otherwise <paramref name="def"/> value.</returns>
        public static uint GetUInt( object obj, uint def )
        {
            if ( obj != null && !( obj is DBNull ) )
                uint.TryParse(obj.ToString(), out def);
            return def;
        }

        /// <summary>
        /// Converts provided <see cref="object"/> to <see cref="string"/> value.
        /// </summary>
        /// <param name="obj"><see cref="object"/> to convert.</param>
        /// <returns><see cref="string"/> value if provided <see cref="object"/> is convertible, otherwise <see cref="String.Empty"/>.</returns>
        public static string GetString( object obj )
        {
            return GetString(obj, String.Empty);
        }

        /// <summary>
        /// Converts provided <see cref="object"/> to <see cref="string"/> value.
        /// </summary>
        /// <param name="obj"><see cref="object"/> to convert.</param>
        /// <param name="def"><see cref="string"/>  value to return if provided <see cref="object"/> is not convertible.</param>
        /// <returns><see cref="string"/> value if provided <see cref="object"/> is convertible, otherwise <paramref name="def"/> value.</returns>
        public static string GetString( object obj, string def )
        {
            return obj != null && !( obj is DBNull ) ? def : obj.ToString();
        }

        /// <summary>
        /// Converts <see cref="object"/> to <see cref="long"/> value.
        /// </summary>
        /// <param name="obj"><see cref="object"/> to convert.</param>
        /// <returns><see cref="long"/> value if provided <see cref="object"/> is convertible, otherwise 0.</returns>
        public static long GetLong( object obj )
        {
            return GetLong(obj, 0);
        }

        /// <summary>
        /// Converts provided <see cref="object"/> to <see cref="long"/> value.
        /// </summary>
        /// <param name="obj"><see cref="object"/> to convert.</param>
        /// <param name="def"><see cref="long"/> value to return if provided <see cref="object"/> is not convertible.</param>
        /// <returns><see cref="long"/> value if provided <see cref="object"/> is convertible, otherwise <paramref name="def"/> value.</returns>
        public static long GetLong( object obj, long def )
        {
            if ( obj != null && !( obj is DBNull ) )
                long.TryParse(obj.ToString(), out def);

            return def;
        }

        /// <summary>
        /// Converts <see cref="object"/> to <see cref="ulong"/> value.
        /// </summary>
        /// <param name="obj"><see cref="object"/> to convert.</param>
        /// <returns><see cref="ulong"/> value if provided <see cref="object"/> is convertible, otherwise 0.</returns>
        public static ulong GetULong( object obj )
        {
            return GetULong(obj, 0);
        }

        /// <summary>
        /// Converts provided <see cref="object"/> to <see cref="ulong"/> value.
        /// </summary>
        /// <param name="obj"><see cref="object"/> to convert.</param>
        /// <param name="def"><see cref="ulong"/> value to return if provided <see cref="object"/> is not convertible.</param>
        /// <returns><see cref="ulong"/> value if provided <see cref="object"/> is convertible, otherwise <paramref name="def"/> value.</returns>
        public static ulong GetULong( object obj, ulong def )
        {
            if ( obj != null && !( obj is DBNull ) )
                ulong.TryParse(obj.ToString(), out def);
            return def;
        }

        /// <summary>
        /// Converts provided <see cref="object"/> to <see cref="double"/> value.
        /// </summary>
        /// <param name="obj"><see cref="object"/> to convert.</param>
        /// <returns><see cref="double"/> value if provided <see cref="object"/> is convertible, otherwise 0.</returns>
        public static double GetDouble( object obj )
        {
            return GetDouble(obj, 0d);
        }

        /// <summary>
        /// Converts provided <see cref="object"/> to <see cref="double"/> value.
        /// </summary>
        /// <param name="obj"><see cref="object"/> to convert.</param>
        /// <param name="def"><see cref="double"/> value to return if provided <see cref="object"/> is not convertible.</param>
        /// <returns><see cref="double"/> value if provided <see cref="object"/> is convertible, otherwise <paramref name="def"/> value.</returns>
        public static double GetDouble( object obj, double def )
        {
            if ( obj != null && !( obj is DBNull ) )
                double.TryParse(obj.ToString(), out def);
            return def;
        }

        /// <summary>
        /// Converts provided <see cref="object"/> to <see cref="DateTime"/> value.
        /// </summary>
        /// <param name="obj"><see cref="object"/> to convert.</param>
        /// <returns><see cref="DateTime"/> value if provided <see cref="object"/> is convertible, otherwise <see cref="DateTime.MinValue"/>.</returns>
        public static DateTime GetDateTime( object obj )
        {
            return GetDateTime(obj, DateTime.MinValue);
        }

        /// <summary>
        /// Converts provided <see cref="object"/> to <see cref="DateTime"/> value.
        /// </summary>
        /// <param name="obj"><see cref="object"/> to convert.</param>
        /// <param name="def">Default <see cref="DateTime"/> value to return if provided <see cref="object"/> is not convertible.</param>
        /// <returns><see cref="DateTime"/> value if provided <see cref="object"/> is convertible, otherwise <paramref name="def"/> value.</returns>
        public static DateTime GetDateTime( object obj, DateTime def )
        {
            if ( obj != null && ( obj is DBNull ) )
                DateTime.TryParse(obj.ToString(), out def);
            return def;
        }

        /// <summary>
        /// Converts provided <see cref="object"/> to <see cref="TimeSpan"/> value.
        /// </summary>
        /// <param name="obj"><see cref="object"/> to convert.</param>
        /// <returns><see cref="TimeSpan"/> value if provided <see cref="object"/> is convertible, otherwise <see cref="TimeSpan.Zero"/>.</returns>
        public static TimeSpan GetTimeSpan( object obj )
        {
            return GetTimeSpan(obj, TimeSpan.Zero);
        }

        /// <summary>
        /// Converts provided <see cref="object"/> to <see cref="TimeSpan"/> value.
        /// </summary>
        /// <param name="obj"><see cref="object"/> to convert.</param>
        /// <param name="def">Default <see cref="TimeSpan"/> value to return if provided <see cref="object"/> is not convertible.</param>
        /// <returns><see cref="TimeSpan"/> value if provided <see cref="object"/> is convertible, otherwise <paramref name="def"/> value.</returns>
        public static TimeSpan GetTimeSpan( object obj, TimeSpan def )
        {
            long v = GetLong(obj, def.Ticks);
            return v == def.Ticks ? def : TimeSpan.FromTicks(v);
        }

        /// <summary>
        /// Converts provided <see cref="object"/> to <see cref="bool"/> value.
        /// </summary>
        /// <param name="obj"><see cref="object"/> to convert.</param>
        /// <returns><see cref="bool"/> value if provided <see cref="object"/> is convertible, otherwise false.</returns>
        public static bool GetBoolean( object obj )
        {
            return GetBoolean(obj, false);
        }

        /// <summary>
        /// Converts provided <see cref="object"/> to <see cref="bool"/> value.
        /// </summary>
        /// <param name="obj"><see cref="object"/> to convert.</param>
        /// <param name="def">Default <see cref="bool"/> value to return if provided <see cref="object"/> is not convertible.</param>
        /// <returns><see cref="bool"/> value if provided <see cref="object"/> is convertible, otherwise <paramref name="def"/> value.</returns>
        public static bool GetBoolean( object obj, bool def )
        {
            if ( obj != null )
            {
                string s = obj.ToString();

                if ( s.Length > 0 )
                {
                    if ( s.Length == 1 )
                    {
                        switch ( s[0] )
                        {
                            case '1':
                                return true;
                            case '0':
                                return false;
                        }
                    }

                    if ( s.Equals(bool.TrueString, StringComparison.InvariantCultureIgnoreCase) )
                        return true;

                    if ( s.Equals(bool.FalseString, StringComparison.InvariantCultureIgnoreCase) )
                        return false;
                }
            }

            return def;
        }
    }
}