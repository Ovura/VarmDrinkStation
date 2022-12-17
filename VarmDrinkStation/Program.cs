namespace VarmDrinkStation
{
    public interface IWarmDrink
    {
        void Consume();
    }
    internal class Water : IWarmDrink
    {
        public void Consume()
        {
            Console.WriteLine("Warm water is served.");
        }
    }
    internal class Coffee : IWarmDrink
    {
        public void Consume()
        {
            Console.WriteLine("Your Coffee is served.");
        }
    }
    internal class Cappuccino : IWarmDrink
    {
        public void Consume()
        {
            Console.WriteLine("Delicious Cappuccino is served.");
        }
    }
    internal class HotChocolate : IWarmDrink
    {
        public void Consume()
        {
            Console.WriteLine("Warm Chocolate is served.");
        }
    }
    public interface IWarmDrinkFactory
    {
        IWarmDrink Prepare(int total);
    }
    internal class HotWaterFactory : IWarmDrinkFactory
    {
        public IWarmDrink Prepare(int total)
        {
            Console.WriteLine($"Pour {total} ml hot water in your cup");
            return new Water();
        }
    }
    internal class HotCoffee : IWarmDrinkFactory
    {
        public IWarmDrink Prepare(int total)
        {
            Console.WriteLine($"Pour {total} ml hot Coffee in your cup");
            return new Coffee();
        }
    }
    internal class HotCappuccino : IWarmDrinkFactory
    {
        public IWarmDrink Prepare(int total)
        {
            Console.WriteLine($"Pour {total} ml hot Cappuccino in your cup");
            return new Cappuccino();
        }
    }
    internal class Chocolate : IWarmDrinkFactory
    {
        public IWarmDrink Prepare(int total)
        {
            Console.WriteLine($"Pour {total} ml hot Chocolate in your cup");
            return new HotChocolate();
        }
    }

    public class WarmDrinkMachine
    {
        public enum AvailableDrink // violates open-closed
        {
            Coffee, Tea
        }
        private Dictionary<AvailableDrink, IWarmDrinkFactory> factories =
          new();

        private List<Tuple<string, IWarmDrinkFactory>> namedFactories =
          new();

        public List<Tuple<string, IWarmDrinkFactory>> NamedFactories { get => NamedFactories2; set => NamedFactories2 = value; }
        public List<Tuple<string, IWarmDrinkFactory>> NamedFactories1 { get => NamedFactories2; set => NamedFactories2 = value; }
        public Dictionary<AvailableDrink, IWarmDrinkFactory> Factories { get => factories; set => factories = value; }
        public List<Tuple<string, IWarmDrinkFactory>> NamedFactories2 { get => namedFactories; set => namedFactories = value; }

        public WarmDrinkMachine()
        {
            foreach (var t in typeof(WarmDrinkMachine).Assembly.GetTypes())
            {
                if (typeof(IWarmDrinkFactory).IsAssignableFrom(t) && !t.IsInterface)
                {
                    NamedFactories2.Add(Tuple.Create(
                      t.Name.Replace("Factory", string.Empty), (IWarmDrinkFactory)Activator.CreateInstance(t)));
                }
            }
        }
        public IWarmDrink MakeDrink()
        {
            Console.WriteLine("This is what we serve today:");
            for (var index = 0; index < NamedFactories2.Count; index++)
            {
                var tuple = NamedFactories2[index];
                Console.WriteLine($"{index}: {tuple.Item1}");
            }
            Console.WriteLine("Select a number to continue:");
            while (true)
            {
                string s;
                if ((s = Console.ReadLine()) != null
                    && int.TryParse(s, out int i) // c# 7
                    && i >= 0
                    && i < NamedFactories2.Count)
                {
                    Console.Write("How much: ");
                    s = Console.ReadLine();
                    if (s != null
                        && int.TryParse(s, out int total)
                        && total > 0)
                    {
                        return NamedFactories2[i].Item2.Prepare(total);
                    }
                }
                Console.WriteLine("Something went wrong with your input, try again.");
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var machine = new WarmDrinkMachine();
            IWarmDrink drink = machine.MakeDrink();
            drink.Consume();
        }
    }
}