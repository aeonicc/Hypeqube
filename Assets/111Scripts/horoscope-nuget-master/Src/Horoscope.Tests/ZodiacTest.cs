//using Xunit;
using System;
using UnityEngine.Assertions;

namespace Horoscope.Tests
{
    public class ZodiacTest
    {
        // [Theory]
        // [InlineData(2, 12, "Aquarius")]
        // [InlineData(3, 3, "Pisces")]
        // [InlineData(2, 28, "Pisces")]
        // [InlineData(10, 23, "Scorpio")]
        public void GetZodiacSignForDate_Test(int month, int day, string zodiacSign)
        {
            var symbol = Zodiac.GetZodiacSignForDate(new DateTime(1950, month, day));
            //Assert.Equal(zodiacSign, symbol.ZodiacName);
            Assert.AreEqual(zodiacSign, symbol.ZodiacName);
        }

        //[Fact]
        public void GetAllZodiacSigns_Test()
        {
            var symbols = Zodiac.GetAllZodiacSigns();
            //Assert.Equal(12, symbols.Count);
            Assert.AreEqual(12, symbols.Count);
        }

        //[Fact]
        public void GetZodiacSign_Pisces_Test()
        {
            var symbols = Zodiac.GetZodiacSign(ZodiacSigns.Pisces);
            // Assert.Equal("Pisces", symbols.ZodiacName);
            // Assert.Equal("The Fish", symbols.ZodiacEnglishTranslation);
            // Assert.Equal("February 19 to March 20", symbols.ZodiacDuration);
            Assert.AreEqual("Pisces", symbols.ZodiacName);
            Assert.AreEqual("The Fish", symbols.ZodiacEnglishTranslation);
            Assert.AreEqual("February 19 to March 20", symbols.ZodiacDuration);
        }

        //[Fact]
        public void GetZodiacSign_Capricorn_Test()
        {
            var symbols = Zodiac.GetZodiacSign(ZodiacSigns.Capricorn);
            Assert.AreEqual("Capricorn", symbols.ZodiacName);
            Assert.AreEqual("The Goat", symbols.ZodiacEnglishTranslation);
            Assert.AreEqual("December 22 to January 19", symbols.ZodiacDuration);
        }
    }
}
