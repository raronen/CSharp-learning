namespace ChainOfResponsibility.MethodChain {
    public class Creature {
        public string Name;
        public int Attack, Defense;

        public Creature(string name, int attack, int defense) {
            Name = name;
            Attack = attack;
            Defense = defense;
        }

        public override string ToString() {
            return $"{nameof(Name)}: {Name}, {nameof(Attack)}: {Attack}, {nameof(Defense)}: {Defense}";
        }
    }

    public class CreatureModifier {
        protected Creature creature;
        protected CreatureModifier next; // linked list

        public CreatureModifier(Creature creature) {
            this.creature = creature;
        }

        public void Add(CreatureModifier cm) {
            if (next != null) next.Add(cm);
            else next = cm;
        }

        public virtual void Handle() => next?.Handle();
    }

    public class DoubleAttackModifier: CreatureModifier {
        public DoubleAttackModifier(Creature creature): base(creature) {}

        public override void Handle()
        {
            Console.WriteLine($"Doubling {creature.Name}'s attack");
            creature.Attack *= 2;
            base.Handle();
        }
    }

    public class IncreareDefenseModifier: CreatureModifier {
        public IncreareDefenseModifier(Creature creature): base(creature) {}

        public override void Handle()
        {
            Console.WriteLine($"Increasing {creature.Name}'s defense");
            creature.Defense += 3;
            base.Handle();
        }
    }

    public class NoBounsesModifer: CreatureModifier {
        public NoBounsesModifer(Creature creature): base(creature) {}

        public override void Handle()
        {
            Console.WriteLine("No bonuses for you!");
        }
    }

    public class Demo {
        public static void Run() {
            var goblin = new Creature("Goblin", 2, 2);

            Console.WriteLine(goblin);

            var root = new CreatureModifier(goblin);
            Console.WriteLine("Let's double goblin's attack");

            root.Add(new DoubleAttackModifier(goblin));
            root.Add(new NoBounsesModifer(goblin)); // Stop list!
            root.Add(new IncreareDefenseModifier(goblin));

            root.Handle();


        }
    }
}