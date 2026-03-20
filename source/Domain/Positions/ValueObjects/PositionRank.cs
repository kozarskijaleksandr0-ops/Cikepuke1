using System;
using System.Collections.Generic;
using System.Text;

namespace DirectoryService.Domain.Positions.ValueObjects
{
	public sealed class PositionRank
	{
		public const int MinRank = 1;
		public const int MaxRank = 100;

		public int Value { get; }

		private PositionRank(int value)
		{
			Value = value;
		}

		public static PositionRank Create(int value)
		{
			if (value < MinRank)
			{
				throw new ArgumentException($"Ранг должен быть не меньше {MinRank}");
			}

			if (value > MaxRank)
			{
				throw new ArgumentException($"Ранг должен быть не больше {MaxRank}");
			}

			return new PositionRank(value);
		}

		public PositionRank Increase() // Повышение ранга (меньше число = выше ранг)
		{
			if (Value == MinRank)
			{
				throw new InvalidOperationException($"Нельзя повысить ранг выше {MinRank}");
			}
			return new PositionRank(Value - 1);
		}

		public PositionRank Decrease() // Понижение ранга (больше число = ниже ранг)
		{
			if (Value == MaxRank)
			{
				throw new InvalidOperationException($"Нельзя понизить ранг ниже {MaxRank}");
			}
			return new PositionRank(Value + 1);
		}
	}
}
