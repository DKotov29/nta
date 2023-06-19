using System;
using System.Collections.Generic;
using System.Diagnostics;

public class Program
{
    static void Main()
    {
        Console.Write("Введiть число, яке потрiбно розкласти на множники: ");
        long n = 61333127792637;//Convert.ToInt64(Console.ReadLine());

        bool isПросте = ПеревіркаНаПростотуМіллера_Рабіна(n, 10);
        Console.WriteLine("Перевiрка числа на простоту Мiллера-Рoбiна:");
        Console.WriteLine("Число " + n + " " + (isПросте ? "є простим" : "є складеним"));

        var множники = РозкластиНаМножники(n);

        Console.WriteLine("\nМножники: ");
        foreach (var множник in множники)
        {
            Console.WriteLine("Дiльник " + множник.Key + " знайдено за допомогою методу " + множник.Value.Item1);
            TimeSpan часВиконання = TimeSpan.FromTicks(множник.Value.Item2);
            Console.WriteLine("Час виконання: " + часВиконання);
        }

        Console.WriteLine("\nДiльники: ");
        Console.WriteLine("[" + string.Join(", ", ОтриматиДільники(множники)) + "]");
    }

    static bool ПеревіркаНаПростотуМіллера_Рабіна(long n, int k)
    {
        if (n < 2) return false;
        if (n != 2 && n % 2 == 0) return false;

        long s = n - 1;
        while (s % 2 == 0) s >>= 1;

        Random r = new Random();
        for (int i = 0; i < k; i++)
        {
            long a = r.Next(Math.Abs((int)n) - 1) + 1;
            long temp = s;
            Stopwatch таймер = Stopwatch.StartNew();
            long mod = ВозвестиВСтепіньПоМодулю(a, temp, n);
            таймер.Stop();
            while (temp != n - 1 && mod != 1 && mod != n - 1)
            {
                mod = (mod * mod) % n;
                temp *= 2;
            }
            if (mod != n - 1 && temp % 2 == 0)
            {
                return false;
            }
        }
        return true;
    }

    static List<KeyValuePair<long, Tuple<string, long>>> РозкластиНаМножники(long n)
    {
        List<KeyValuePair<long, Tuple<string, long>>> множники = new List<KeyValuePair<long, Tuple<string, long>>>();
        Stopwatch таймер = new Stopwatch();

        if (!ПеревіркаНаПростотуМіллера_Рабіна(n, 10))
        {
            таймер.Start();
            while (true)
            {
                long дільник = ДільникиМетодомПробноїДілення(n);
                таймер.Stop();
                if (дільник == 0)
                {
                    break;
                }
                else
                {
                    множники.Add(new KeyValuePair<long, Tuple<string, long>>(дільник, new Tuple<string, long>("Пробних подiлок", таймер.ElapsedTicks)));
                    n /= дільник;
                    таймер.Restart();
                }
            }
        }

        if (n == 1)
        {
            return множники;
        }

        таймер.Restart();
        while (true)
        {
            long дільник = ДільникиМетодомПолларда_Роу(n);
            таймер.Stop();
            if (дільник == n)
            {
                множники.Add(new KeyValuePair<long, Tuple<string, long>>(n, new Tuple<string, long>("Поллард-роу", таймер.ElapsedTicks)));
                break;
            }
            else
            {
                множники.Add(new KeyValuePair<long, Tuple<string, long>>(дільник, new Tuple<string, long>("Поллард-роу", таймер.ElapsedTicks)));
                n /= дільник;
                таймер.Restart();
            }
        }

        return множники;
    }

    static long ДільникиМетодомПробноїДілення(long n)
    {
        for (long i = 2; i * i <= n; i++)
        {
            if (n % i == 0)
            {
                return i;
            }
        }

        return 0;
    }

    static long ДільникиМетодомПолларда_Роу(long n)
    {
        long x = 2;
        long y = 2;
        long d = 1;
        Func<long, long> f = z => (z * z + 1) % n;

        while (d == 1)
        {
            x = f(x);
            y = f(f(y));
            d = НСД(Math.Abs(x - y), n);
        }

        return d;
    }

    static long ВозвестиВСтепіньПоМодулю(long основа, long показник, long модуль)
    {
        long результат = 1;
        while (показник > 0)
        {
            if (показник % 2 == 1) результат = (результат * основа) % модуль;
            основа = (основа * основа) % модуль;
            показник >>= 1;
        }
        return результат;
    }

    static long НСД(long a, long b)
    {
        while (b != 0)
        {
            long тимчасове = b;
            b = a % b;
            a = тимчасове;
        }
        return a;
    }

    static List<long> ОтриматиДільники(List<KeyValuePair<long, Tuple<string, long>>> множники)
    {
        List<long> дільники = new List<long>();
        foreach (var множник in множники)
        {
            дільники.Add(множник.Key);
        }
        return дільники;
    }
}
