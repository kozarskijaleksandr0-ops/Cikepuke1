using DirectoryService.Domain.LocationsContext;
using DirectoryService.Domain.LocationsContext.ValueObjects;
using DirectoryService.Domain.Shared;

namespace Solution_Test
{
	public partial class TestDepartment
	{
        public class TestLocation
		{
			[Theory]
			[InlineData("Asia/Tokyo")]
			[InlineData("Europe/Moscow")]
			public void CreateIanaTimeZone_ValidZone_ReturnsCorrectValue(string timeZone)
			{
				IanaTimeZone result = IanaTimeZone.Create(timeZone);
				Assert.Equal(timeZone, result.Value);
			}

			[Fact]
			public void CreateIanaTimeZone_EmptyString_ThrowsArgumentException()
			{
				Assert.Throws<ArgumentException>(() => IanaTimeZone.Create(""));
			}

			[Fact]
			public void CreateLocation_WithAllComponents_PropertiesSetCorrectly()
			{
				LocationName name = LocationName.Create("island Epstein");
				LocationId id = LocationId.Create();
				LocationAddress address = LocationAddress.Create("island Epstein");
				IanaTimeZone timeZone = IanaTimeZone.Create("Asia/Tokyo");
				EntityLifeTime lifeTime = EntityLifeTime.Create(DateTime.Now, DateTime.Now);
				Location location = new Location(id, address, name, timeZone, lifeTime);

				Assert.Equal("island Epstein", location.Name.Value);
				Assert.Equal("Asia/Tokyo", location.TimeZone.Value);
			}

			[Theory]
			[InlineData("")]
			[InlineData("   ")]
			public void CreateLocationName_EmptyOrWhitespace_ThrowsArgumentException(string input)
			{
				Assert.Throws<ArgumentException>(() => LocationName.Create(input));
			}

			[Theory]
			[InlineData("Tokyo, Japan")]
			[InlineData("Москва, Россия")]
			public void CreateLocationAddress_ValidAddress_ReturnsCorrectValue(string address)
			{
				LocationAddress result = LocationAddress.Create(address);
				Assert.Equal(address, result.Value);
			}

			[Theory]
			[InlineData("Главный офис")]
			[InlineData("Филиал")]
			public void CreateLocationName_ValidNames_ReturnsCorrectValue(string name)
			{
				LocationName result = LocationName.Create(name);
				Assert.Equal(name, result.Value);
			}

			[Fact]
			public void CreateLocationName_TooShort_ThrowsArgumentException()
			{
				Assert.Throws<ArgumentException>(() => LocationName.Create("АБ"));
			}
		}
	}
}
