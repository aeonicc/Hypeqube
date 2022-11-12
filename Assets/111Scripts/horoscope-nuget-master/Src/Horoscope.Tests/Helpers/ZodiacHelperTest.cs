using Horoscope.Helpers;
using UnityEngine.Assertions;

//using Xunit;


namespace Horoscope.Tests.Helpers
{
    public class ZodiacHelperTest
    {
        //[Fact]
        public void InitializeAndGetAllZodiacSigns_Count_Test()
        {
            var symbol = ZodiacHelper.InitializeAndGetAllZodiacSigns();
            Assert.AreEqual(12, symbol.Count);
        }

        //[Fact]
        public void InitializeAndGetAllZodiacSigns_ZodiacName_Test()
        {
            ZodiacHelper.InitializeAndGetAllZodiacSigns().TryGetValue(ZodiacSigns.Capricorn, out Model.ZodiacModel zodiacModel);
            Assert.AreEqual(ZodiacSigns.Capricorn.ToString(), zodiacModel.ZodiacName);
        }
    }
}
