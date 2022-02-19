namespace Noise.Hash
{
    public readonly struct SmallXXHash
    {
        const uint primeA = 0b10011110001101110111100110110001;
        const uint primeB = 0b10000101111010111100101001110111;
        const uint primeC = 0b11000010101100101010111000111101;
        const uint primeD = 0b00100111110101001110101100101111;
        const uint primeE = 0b00010110010101100110011110110001;

        readonly private uint _accumulator;

        public static implicit operator uint(SmallXXHash hash)
        {
            uint avalanche = hash._accumulator;
            avalanche ^= avalanche >> 15;
            avalanche *= primeB;
            avalanche ^= avalanche >> 13;
            avalanche *= primeC;
            avalanche ^= avalanche >> 16;
            return avalanche;
        }

        static uint RotateLeft(uint data, int steps) => (data << steps) | (data >> 32 - steps);

        public static implicit operator SmallXXHash(uint accumulator) => new SmallXXHash(accumulator);

        public SmallXXHash Eat(int data) => RotateLeft(_accumulator + (uint)data * primeC, 17) * primeD;

        public SmallXXHash Eat(byte data) => RotateLeft(_accumulator + data * primeE, 11) * primeA;

        public static SmallXXHash Seed(int seed) => (uint)seed + primeE;
        
        public SmallXXHash(uint accumulator)
        {
            this._accumulator = accumulator;
        }


    }
}