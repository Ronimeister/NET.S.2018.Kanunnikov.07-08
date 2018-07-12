using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace CustomerLib
{
    /// <summary>
    /// Class that provides information about customer
    /// </summary>
    public class Customer : IFormattable
    {
        #region Constants
        private const string FORMAT_BY_DEFAULT = "G";
        #endregion

        #region Properties and fields

        private string _name;
        private string _contactPhone;
        private decimal _revenue;

        /// <summary>
        /// Public property that describe customer name
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (!IsLettersOnly(value))
                {
                    throw new FormatException($"{nameof(value)} is invalid");
                }

                _name = value;
            }
        }

        /// <summary>
        /// Public property that describe customer contact phone
        /// </summary>
        public string ContactPhone
        {
            get
            {
                return _contactPhone;
            }
            set
            {
                if (!IsDigitsOnly(value))
                {
                    throw new FormatException($"{nameof(value)} is invalid");
                }

                _contactPhone = value;
            }
        }


        /// <summary>
        /// Public property that describe customer revenue
        /// </summary>
        public decimal Revenue
        {
            get
            {
                return _revenue;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException($"{nameof(value)} is invalid");
                }

                _revenue = value;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Default <see cref="Customer"/> constructor
        /// </summary>
        public Customer()
        {
            Name = string.Empty;
            ContactPhone = string.Empty;
            Revenue = 0;
        }

        /// <summary>
        /// <see cref="Customer"/> constructor
        /// </summary>
        /// <param name="name">Describe customer name</param>
        /// <param name="revenue">Describe customer revenue</param>
        /// <param name="contactPhone">Describe customer phone</param>
        public Customer(string name, decimal revenue, string contactPhone)
        {
            Name = name;
            ContactPhone = contactPhone;
            Revenue = revenue;
        }
        #endregion

        #region ToString()
        /// <summary>
        /// Overrided <see cref="Object.ToString"/> method
        /// </summary>
        /// <returns>Default <see cref="Customer"/> string represantation</returns>
        public override string ToString()
            => $"Customer record: {Name}, {Revenue.ToString("N", CultureInfo.InvariantCulture)}, {ContactPhone}";

        /// <summary>
        /// <see cref="Customer"/> string represantation
        /// </summary>
        /// <param name="format">String format</param>
        /// <returns><see cref="Customer"/> string represantation based on <paramref name="format"/></returns>
        public string ToString(string format) => ToString(format, CultureInfo.InvariantCulture);

        /// <summary>
        /// <see cref="Customer"/> string represantation
        /// </summary>
        /// <param name="format">String format</param>
        /// <param name="formatProvider">String format provider</param>
        /// <exception cref="FormatException">Throws when <paramref name="format"/> is not supported</exception>
        /// <returns><see cref="Customer"/> string represantation based on <paramref name="format"/> and <paramref name="formatProvider"/></returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format))
            {
                format = FORMAT_BY_DEFAULT;
            }

            if (formatProvider is null)
            {
                formatProvider = CultureInfo.InvariantCulture;
            }

            return GenerateStringByFormat(format, formatProvider);
        }
        #endregion

        #region Private API
        /// <summary>
        /// Generate <see cref="Customer"/> string based on <paramref name="format"/> and <paramref name="formatProvider"/>
        /// </summary>
        /// <param name="format">String format</param>
        /// <param name="formatProvider">String format provider</param>
        /// <returns><see cref="Customer"/> string represantation based on <paramref name="format"/> and <paramref name="formatProvider"/></returns>
        private string GenerateStringByFormat(string format, IFormatProvider formatProvider)
        {
            switch (format.Trim().ToUpperInvariant())
            {
                case "G":
                case "NRP":
                    return $"Customer record: {_name}, {_revenue.ToString("N", formatProvider)}, {_contactPhone}";
                case "N":
                    return $"Customer record: {_name}";
                case "R":
                    return $"Customer record: {_revenue.ToString("N", formatProvider)}";
                case "P":
                    return $"Customer record: {_contactPhone}";
                case "NR":
                    return $"Customer record: {_name}, {_revenue.ToString("N", formatProvider)}";
                case "NP":
                    return $"Customer record: {_name}, {_contactPhone}";
                case "RP":
                    return $"Customer record: {_revenue.ToString("N", formatProvider)}, {_contactPhone}";
                default:
                    throw new FormatException($"{nameof(format)} is not supported.");
            }
        }

        private bool IsDigitsOnly(string str)
        {
            return Regex.IsMatch(str, @"^\+\d+\s(\(\d+\))\s\d{3}-\d{4}$");
        }

        private bool IsLettersOnly(string str)
        {
            return Regex.IsMatch(str, @"^([A-Z][a-z]*\s)*[A-Z][a-z]*$");
        }
        #endregion
    }
}
