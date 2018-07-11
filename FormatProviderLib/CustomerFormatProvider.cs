using System;
using System.Globalization;
using System.Text;
using CustomerLib;

namespace FormatProviderLib
{
    /// <summary>
    /// Class that provides additional functionality to <see cref="Customer"/> class
    /// </summary>
    public class CustomerFormatProvider : ICustomFormatter, IFormatProvider
    {
        #region Constants
        private const string FORMAT_BY_DEFAULT = "GC";
        #endregion

        #region Private fields and Properties
        private IFormatProvider _provider;

        static readonly string[] _numberWords =
            "zero one two three four five six seven eight nine minus point".Split();

        static readonly string _numberString = "0123456789-.";
        #endregion

        #region Constructors
        /// <summary>
        /// Default <see cref="CustomerFormatProvider"/> constructor
        /// </summary>
        public CustomerFormatProvider()
        {
            _provider = CultureInfo.InvariantCulture;
        }

        /// <summary>
        /// <see cref="CustomerFormatProvider"/> constructor
        /// </summary>
        /// <param name="provider"><see cref="IFormatProvider"/> provider</param>
        public CustomerFormatProvider(IFormatProvider provider)
        {
            _provider = provider;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Present <paramref name="arg"/> string representation in specific format
        /// </summary>
        /// <param name="format">String format</param>
        /// <param name="arg">Object need to be represented</param>
        /// <param name="formatProvider">String format provider</param>        
        /// <exception cref="ArgumentNullException">Throws when arg is equal to null.</exception>
        /// <exception cref="ArgumentException">Throws when arg isn't Customer.</exception>
        /// <returns><paramref name="arg"/> string representation in specific format</returns>
        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (arg is null)
            {
                throw new ArgumentNullException($"{nameof(arg)} can't be equal to null!");
            }

            if (arg.GetType() != typeof(Customer))
            {
                throw new ArgumentException($"{nameof(arg)} should have {nameof(Customer)} type!");
            }

            if (string.IsNullOrEmpty(format))
            {
                format = FORMAT_BY_DEFAULT;
            }

            if (formatProvider is null)
            {
                formatProvider = _provider;
            }

            return GenerateStringByFormat(format, arg, formatProvider);
        }

        /// <summary>
        /// Return <see cref="Type"/> of this <paramref name="formatType"/>
        /// </summary>
        /// <param name="formatType">Format type</param>
        /// <returns><see cref="Type"/> of this <paramref name="formatType"/></returns>
        public object GetFormat(Type formatType)
            => (formatType == typeof(ICustomFormatter)) ? this : null;
        #endregion

        #region Private methods
        /// <summary>
        /// Present <paramref name="arg"/> string representation in specific format
        /// </summary>
        /// <param name="format">String format</param>
        /// <param name="arg">Object need to be represented</param>
        /// <param name="formatProvider">String format provider</param>
        /// <returns><paramref name="arg"/> string representation in specific format</returns>
        private string GenerateStringByFormat(string format, object arg, IFormatProvider formatProvider)
        {
            Customer customer = (Customer)arg;
            switch (format.Trim().ToUpperInvariant())
            {
                case "GC":
                case "NRCP":
                    return $"Customer record: {customer.Name}, {customer.Revenue.ToString("C", formatProvider)}, {customer.ContactPhone}";
                case "RC":
                    return $"Customer record: {customer.Revenue.ToString("C", formatProvider)}";
                case "NRC":
                    return $"Customer record: {customer.Name}, {customer.Revenue.ToString("C", formatProvider)}";
                case "RCP":
                    return $"Customer record: {customer.Revenue.ToString("C", formatProvider)}, {customer.ContactPhone}";
                case "W":
                    return $"Customer record: {customer.Name}, {StringToWords(customer.Revenue.ToString())}, {StringToWords(customer.ContactPhone)}";
                case "RW":
                    return $"Customer record: {StringToWords(customer.Revenue.ToString())}";
                case "PW":
                    return $"Customer record: {StringToWords(customer.ContactPhone)}";
                case "NPW":
                    return $"Customer record: {customer.Name}, {StringToWords(customer.ContactPhone)}";
                case "NRW":
                    return $"Customer record: {customer.Name}, {StringToWords(customer.Revenue.ToString())}";
                default:
                    throw new FormatException($"{nameof(format)} is not supported.");
            }
        }

        /// <summary>
        /// Represent <paramref name="input"/> string in words-form
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns><paramref name="input"/> string in words-form</returns>
        private string StringToWords(string input)
        {
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < input.Length; i++)
            {
                int index = _numberString.IndexOf(input[i]);

                if (index == -1)
                {
                    continue;
                }

                if (result.Length > 0)
                {
                    result.Append(' ');
                }

                result.Append(_numberWords[index]);
            }

            return result.ToString();
        }
        #endregion
    }
}
