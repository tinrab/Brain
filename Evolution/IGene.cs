using System;

namespace Brain.Evolution
{
	public interface IGene : IEquatable<IGene>
	{
		object Value { get; set; }
	}
}