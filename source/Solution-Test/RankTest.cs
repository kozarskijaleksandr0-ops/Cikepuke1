using System;
using System.Collections.Generic;
using System.Text;
using DirectoryService.Domain.DepartmentsContext;
using DirectoryService.Domain.DepartmentsContext.ValueObjects;
using DirectoryService.Domain.Positions;
using DirectoryService.Domain.Positions.ValueObjects;
using DirectoryService.Domain.Shared;
using Xunit;

namespace Solution_Test
{
	public class RankTest
	{
		[Theory]
		[InlineData(1)]
		[InlineData(50)]
		[InlineData(100)]
		public void Create_ValidRank_Success(int value)
		{			
			PositionRank rank = PositionRank.Create(value);
			Assert.Equal(value, rank.Value);
		}

		[Fact]
		public void Create_TooSmall_Throws()
		{
			Assert.Throws<ArgumentException>(() => PositionRank.Create(0));
		}

		[Fact]
		public void Create_TooLarge_Throws()
		{
			Assert.Throws<ArgumentException>(() => PositionRank.Create(101));
		}

		[Fact]
		public void Increase_RankGoesUp()
		{
			PositionRank rank = PositionRank.Create(10);
			PositionRank increased = rank.Increase();
			Assert.Equal(9, increased.Value);
		}

		[Fact]
		public void Increase_MaxRank_Throws()
		{
			PositionRank rank = PositionRank.Create(1);
			Assert.Throws<InvalidOperationException>(rank.Increase);
		}

		[Fact]
		public void Decrease_RankGoesDown()
		{
			PositionRank rank = PositionRank.Create(10);
			PositionRank decreased = rank.Decrease();
			Assert.Equal(11, decreased.Value);
		}

		[Fact]
		public void Decrease_MinRank_Throws()
		{
			PositionRank rank = PositionRank.Create(100);
			Assert.Throws<InvalidOperationException>(rank.Decrease);
		}
	}
}
