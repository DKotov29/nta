internal class Program
{
    private static void Main(string[] args)
    {
        int result = SPH(322, 674, 13);
        Console.WriteLine($"Result: {result}");
        //1337228112
        //1337228111
    }
    static int Power(int a, int b, int c)
    {
        int buf = 1;
        int jx = 0;
        while (jx < c)
        {
            buf *= a;
            buf %= b;
            jx++;
        }
        return buf;
    }

    static Dictionary<int, int> Decomposition(int n)
    {
        Dictionary<int, int> nums = new Dictionary<int, int>();
        for (int i = 2; n > 1; i++)
        {
            while (n % i == 0)
            {
                if (!nums.ContainsKey(i))
                    nums[i] = 0;
                nums[i]++;
                n /= i;
            }
        }
        return nums;
    }

    static int XModP(int n, int b, int y, int p, int h)
    {
        int x = 0;
        int buf;
        Dictionary<int, int> r = new Dictionary<int, int>();
        for (int ix = 0; ix < p; ix++)
        {
            buf = Power(b, n, (n - 1) / p * ix);
            r[buf] = ix;
            Console.WriteLine($"r{p},{ix} = {buf}");
        }
        Console.WriteLine();

        Console.Write("x = ");
        for (int i = 0; i < h; i++)
            Console.Write($"{Math.Pow(p, i)}x{i} + ");
        Console.WriteLine();

        for (int ix = 0; ix < h; ix++)
        {
            buf = Power(y, n, (n - 1) / (int)Math.Pow(p, ix + 1));
            Console.WriteLine($"r(p,x) = {buf}");

            x += (int)Math.Pow(p, ix) * r[buf];
            Console.WriteLine($"x = {x}");

            buf = -r[buf];
            while (buf < 0)
                buf += n - 1;
            buf = Power(b, n, (int)Math.Pow(p, ix) * buf);
            y = y * buf % n;
            Console.WriteLine($"y{ix + 1} = {y}");
        }

        buf = (int)Math.Pow(p, h);
        return x % buf;
    }

    static int ExtEuclid(int a, int b, out int x, out int y)
    {
        int d;
        if (b == 0)
        {
            d = a;
            x = 1;
            y = 0;
            return d;
        }

        int q, r, x1 = 0, x2 = 1, y1 = 1, y2 = 0;
        while (b > 0)
        {
            q = a / b;
            r = a - q * b;
            x = x2 - q * x1;
            y = y2 - q * y1;
            a = b;
            b = r;
            x2 = x1;
            x1 = x;
            y2 = y1;
            y1 = y;
        }

        d = a;
        x = x2;
        y = y2;
        return d;
    }

    static int ChiResidueTheory(List<int> a, Dictionary<int, int> nums, int n)
    {
        int x = 0;
        int M = 1;
        List<int> b = new List<int>();
        foreach (var pair in nums)
            b.Add((int)Math.Pow(pair.Key, pair.Value));

        foreach (int num in b)
            M *= num;

        int q, z;
        for (int i = 0; i < a.Count; i++)
        {
            ExtEuclid(M / b[i], b[i], out q, out z);
            Console.WriteLine($"BACK element to {M / b[i]} is {q}");

            x += a[i] * M / b[i] * q;
            if (x > n - 1)
                x %= (n - 1);
            while (x < 0)
            {
                x += n - 1;
            }
        }
        return x;
    }

    static int SPH(int n, int b, int y)
    {
        Dictionary<int, int> nums = Decomposition(n - 1);
        List<int> x = new List<int>();
        Console.Write($"{n - 1} = ");
        foreach (var pair in nums)
            Console.Write($"{pair.Key}^{pair.Value} * ");
        Console.WriteLine();

        foreach (var pair in nums)
            x.Add(XModP(n, b, y, pair.Key, pair.Value));

        foreach (int num in x)
            Console.WriteLine(num);

        return ChiResidueTheory(x, nums, n);
    }

}