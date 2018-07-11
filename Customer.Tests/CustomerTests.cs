using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomerLib;
using System.Globalization;
using FormatProviderLib;

namespace Customer.Tests
{
    [TestFixture]
    public class CustomerTests
    {
        #region Customer.cs tests
        [TestCase("G", ExpectedResult = "Customer record: Jeffrey Richter, 1,234,567.00, +1 (425) 555-0100")]
        [TestCase("NRP", ExpectedResult = "Customer record: Jeffrey Richter, 1,234,567.00, +1 (425) 555-0100")]
        [TestCase("NR", ExpectedResult = "Customer record: Jeffrey Richter, 1,234,567.00")]
        [TestCase("NP", ExpectedResult = "Customer record: Jeffrey Richter, +1 (425) 555-0100")]
        [TestCase("RP", ExpectedResult = "Customer record: 1,234,567.00, +1 (425) 555-0100")]
        [TestCase("N", ExpectedResult = "Customer record: Jeffrey Richter")]
        [TestCase("R", ExpectedResult = "Customer record: 1,234,567.00")]
        [TestCase("P", ExpectedResult = "Customer record: +1 (425) 555-0100")]
        [TestCase(null, ExpectedResult = "Customer record: Jeffrey Richter, 1,234,567.00, +1 (425) 555-0100")]
        [TestCase("", ExpectedResult = "Customer record: Jeffrey Richter, 1,234,567.00, +1 (425) 555-0100")]
        public string ToString_IsCorrect(string format)
        {
            CustomerLib.Customer customer = new CustomerLib.Customer("Jeffrey Richter", 1234567, "+1 (425) 555-0100");
            return customer.ToString(format);
        }

        [TestCase("G", "ru-RU", ExpectedResult = "Customer record: Jeffrey Richter, 1 234 567,00, +1 (425) 555-0100")]
        [TestCase("NRP", "en-US", ExpectedResult = "Customer record: Jeffrey Richter, 1,234,567.00, +1 (425) 555-0100")]
        [TestCase("NR", "ru-RU", ExpectedResult = "Customer record: Jeffrey Richter, 1 234 567,00")]
        [TestCase("NP", "en-US", ExpectedResult = "Customer record: Jeffrey Richter, +1 (425) 555-0100")]
        [TestCase("RP", "ru-RU", ExpectedResult = "Customer record: 1 234 567,00, +1 (425) 555-0100")]
        [TestCase("N", "ru-RU", ExpectedResult = "Customer record: Jeffrey Richter")]
        [TestCase("R", "en-US", ExpectedResult = "Customer record: 1,234,567.00")]
        [TestCase("P", "ru-RU", ExpectedResult = "Customer record: +1 (425) 555-0100")]
        [TestCase(null, "ru-RU", ExpectedResult = "Customer record: Jeffrey Richter, 1 234 567,00, +1 (425) 555-0100")]
        [TestCase("", "en-US", ExpectedResult = "Customer record: Jeffrey Richter, 1,234,567.00, +1 (425) 555-0100")]
        public string ToString_IsCorrect_WithCulture(string format, string culture)
        {
            CultureInfo cultureInfo = new CultureInfo(culture);
            CustomerLib.Customer customer = new CustomerLib.Customer("Jeffrey Richter", 1234567, "+1 (425) 555-0100");
            return customer.ToString(format, cultureInfo);
        }


        [TestCase("GSDSD")]
        [TestCase("OG")]
        public void ToString_FormatIsNotSupported_FormatException(string format)
            => Assert.Throws<FormatException>(() => new CustomerLib.Customer().ToString(format));
        #endregion

        #region CustomerFormatProvider.cs tests
        [TestCase("GC", ExpectedResult = "Customer record: Jeffrey Richter, ¤1,234,567.00, +1 (425) 555-0100")]
        [TestCase("NRCP", ExpectedResult = "Customer record: Jeffrey Richter, ¤1,234,567.00, +1 (425) 555-0100")]
        [TestCase("RC", ExpectedResult = "Customer record: ¤1,234,567.00")]
        [TestCase("NRC", ExpectedResult = "Customer record: Jeffrey Richter, ¤1,234,567.00")]
        [TestCase("RCP", ExpectedResult = "Customer record: ¤1,234,567.00, +1 (425) 555-0100")]
        [TestCase("W", ExpectedResult = "Customer record: Jeffrey Richter, one two three four five six seven, one four two five five five five minus zero one zero zero")]
        [TestCase("RW", ExpectedResult = "Customer record: one two three four five six seven")]
        [TestCase("PW", ExpectedResult = "Customer record: one four two five five five five minus zero one zero zero")]
        [TestCase("NPW", ExpectedResult = "Customer record: Jeffrey Richter, one four two five five five five minus zero one zero zero")]
        [TestCase("NRW", ExpectedResult = "Customer record: Jeffrey Richter, one two three four five six seven")]
        public string Format_IsCorrect(string format)
        {
            CustomerFormatProvider provider = new CustomerFormatProvider();
            return provider.Format(format, new CustomerLib.Customer("Jeffrey Richter", 1234567, "+1 (425) 555-0100"), CultureInfo.InvariantCulture);
        }

        [TestCase("GC", "ru-RU", ExpectedResult = "Customer record: Jeffrey Richter, 1 234 567,00 ₽, +1 (425) 555-0100")]
        [TestCase("NRCP", "en-US", ExpectedResult = "Customer record: Jeffrey Richter, $1,234,567.00, +1 (425) 555-0100")]
        [TestCase("RC", "ru-RU", ExpectedResult = "Customer record: 1 234 567,00 ₽")]
        [TestCase("NRC", "en-US", ExpectedResult = "Customer record: Jeffrey Richter, $1,234,567.00")]
        [TestCase("RCP", "ru-RU", ExpectedResult = "Customer record: 1 234 567,00 ₽, +1 (425) 555-0100")]
        [TestCase("W", "ru-RU", ExpectedResult = "Customer record: Jeffrey Richter, one two three four five six seven, one four two five five five five minus zero one zero zero")]
        [TestCase("RW", "en-US", ExpectedResult = "Customer record: one two three four five six seven")]
        [TestCase("PW", "ru-RU", ExpectedResult = "Customer record: one four two five five five five minus zero one zero zero")]
        [TestCase("NPW", "ru-RU", ExpectedResult = "Customer record: Jeffrey Richter, one four two five five five five minus zero one zero zero")]
        [TestCase("NRW", "en-US", ExpectedResult = "Customer record: Jeffrey Richter, one two three four five six seven")]
        public string Format_IsCorrect_WithCulture(string format, string culture)
        {
            CustomerFormatProvider provider = new CustomerFormatProvider();
            return provider.Format(format, new CustomerLib.Customer("Jeffrey Richter", 1234567, "+1 (425) 555-0100"), new CultureInfo(culture));
        }

        [TestCase("DRERE")]
        [TestCase("HITHERE")]
        public void Format_FormatIsNotSupported_FormatException(string format)
            => Assert.Throws<FormatException>(() => new CustomerFormatProvider().Format(format, new CustomerLib.Customer("Jeffrey Richter", 1234567, "+1 (425) 555-0100"), CultureInfo.InvariantCulture));
        #endregion
    }
}
