using System;
using System.Globalization;
using System.IO;
using DirectoryService.Domain.DepartmentsContext;
using DirectoryService.Domain.DepartmentsContext.ValueObjects;
using DirectoryService.Domain.LocationsContext;
using DirectoryService.Domain.LocationsContext.ValueObjects;
using DirectoryService.Domain.Shared;

namespace Solution_Test
{
	public partial class TestDepartment
	{
		[Theory]
		[InlineData("A")]
		[InlineData("1")]
		public void CreateDepartmentIdentifier(string input)
		{
			Assert.Throws<ArgumentException>(() => DepartmentIdentifier.Create(input));
		}

		[Theory]
		[InlineData("ИТ-РАЗРАБ")]
		[InlineData("HR-REC")]
		public void CreateDepartmentPath(string path)
		{
			DepartmentPath result = DepartmentPath.Create(path);
			Assert.Equal(path, result.Value);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(5)]
		[InlineData(10)]
		public void CreateDepartmentDepth_ValidDepth_ReturnsCorrectValu(short depth)
		{
			DepartmentDepth result = DepartmentDepth.Create(depth);
			Assert.Equal(depth, result.Value);
		}

		[Fact]
		public void CreateDepartmentDepth_Negative_ThrowsArgumentException()
		{
			Assert.Throws<ArgumentException>(() => DepartmentDepth.Create(-1));
		}

		[Fact]
		public void AddChild_WhenChildAlreadyHasParent_ThrowsInvalidOperationException()
		{
            Department department = new Department(
            DepartmentId.Create(),
            DepartmentName.Create("Главный офис"),
            DepartmentIdentifier.Create("hq"),
            EntityLifeTime.Create(DateTime.UtcNow, DateTime.UtcNow)			
        );			
			Department child = new Department(
            DepartmentId.Create(),
            DepartmentName.Create("Бухгалтерия"),
            DepartmentIdentifier.Create("acc"),
            EntityLifeTime.Create(DateTime.UtcNow, DateTime.UtcNow)				
        );
		    department.AddChild(child);
            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => 
            department.AddChild(child));
		    Assert.Equal("Подразделение не является дочерним.", exception.Message);
		}
		 
		[Fact]
		public void AddLocation_WhenLocationAlreadyExists_ThrowsInvalidOperationException()		
		{
            DepartmentId deparmentId = DepartmentId.Create();
            DepartmentName departmentName = DepartmentName.Create("Главный офис");
            DepartmentIdentifier departmentIdentifier = DepartmentIdentifier.Create("hq");
            EntityLifeTime departmentLifeTime = EntityLifeTime.Create(DateTime.UtcNow, DateTime.UtcNow);
			Department department = new Department(deparmentId, departmentName, departmentIdentifier, departmentLifeTime);

			// create location object
			LocationId locationId = LocationId.Create();
			LocationName locationName = LocationName.Create("Главный офис");
			LocationAddress locationAddress = LocationAddress.Create("Россия, Москва, улица Пушкина, 1");
			IanaTimeZone locationTimeZone = IanaTimeZone.Create("Europe/Moscow");
			EntityLifeTime locationLifeTime = EntityLifeTime.Create(DateTime.UtcNow, DateTime.UtcNow);
            Location location = new Location(locationId, locationAddress, locationName, locationTimeZone, locationLifeTime);

		    department.AddLocation(location);

            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() => 
            department.AddLocation(location));
        
            Assert.Equal("Локация уже присутствует в подразделении.", exception.Message);
		}	    
    } 
}