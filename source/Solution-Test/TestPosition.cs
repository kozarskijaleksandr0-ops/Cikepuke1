using DirectoryService.Domain.Positions;
using DirectoryService.Domain.Positions.ValueObjects;
using DirectoryService.Domain.Shared;

namespace Solution_Test
{
	public partial class TestDepartment
	{
        public class TestPosition
		{
			[Fact]
			public void CreatePosition_ValidData_PropertiesSetCorrectly()
			{
				Position position = new Position(
					PositionId.Create(),
					PositionName.Create("Developer"),
					PositionDescription.Create("Desc"),
					true,
					EntityLifeTime.Create(DateTime.UtcNow, DateTime.UtcNow)
				);

				Assert.Equal("Developer", position.Name.Value);
			}

			[Fact]
			public void ChangePositionName_ValidName_UpdatesSuccessfully()
			{
				Position position = new Position(
					PositionId.Create(),
					PositionName.Create("Dev"),
					PositionDescription.Create("Desc"),
					true,
					EntityLifeTime.Create(DateTime.UtcNow, DateTime.UtcNow)
				);

				position.ChangePositionName(PositionName.Create("Senior Dev"));
				Assert.Equal("Senior Dev", position.Name.Value);
			}

			[Theory]
			[InlineData("Менеджер")]
			[InlineData("Разработчик")]
			public void CreatePositionName_ValidNames_ReturnsCorrectValue(string name)
			{
				PositionName result = PositionName.Create(name);
				Assert.Equal(name, result.Value);
			}
		}
	}
}
