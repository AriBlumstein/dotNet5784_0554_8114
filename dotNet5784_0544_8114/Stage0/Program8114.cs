namespace Stage0
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Welcome0554();
            Welcome8114();
            Console.ReadKey();
        }

        static partial void Welcome0554();
        private static void Welcome8114()
        {
            Console.Write("Enter your name: ");
            string name = Console.ReadLine();
            Console.WriteLine("{0} welcome to my first console application", name);
        }
    }
}