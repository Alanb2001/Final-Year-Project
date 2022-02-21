using Unity.Mathematics;

using static Unity.mathematics.math;

namespace Noise.Hash
{
	public static partial class Noise
	{
		public interface Value : IGradient
		{
			float4 Evaluate(SmallXXHash4 hash, float4 x) => x;

			float4 Evaluate(SmallXXHash4 hash, float4 x, float4 y) => hash.Floats01A;

			float4 Evaluate(SmallXXHash4 hash, float4 x, float4 y, float4 z) => hash.Floats01As;
		}
	}
}