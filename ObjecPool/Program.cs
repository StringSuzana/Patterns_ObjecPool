using System;
using System.Collections.Generic;

namespace ObjecPool
{
    /**
    • Consider an example in which we must create instances of class Object but no more than 10
    • instances should be created.
    • Write an implementation of that problem
    • How can you enable reusing existing 10 objects if needed?
    • Did you use any design patterns? Which patterns did you use and why?
    */
    public class Program
    {
        static void Main(string[] args)
        {
            MonsterPool pool = MonsterPool.GetPoolInstance(pool_size: 3);
            var monster_1 = pool.GetMonster("wolf 1");

            Console.WriteLine(monster_1);
            Console.WriteLine(pool.GetMonster("dwarf 1"));
            Console.WriteLine(pool.GetMonster("bigfoot 1"));
            Console.WriteLine(pool.GetMonster("dwarf 2"));

            pool.ReturnMonster(monster_1);

            Console.WriteLine(pool.GetMonster("dwarf 3"));


        }
    }
    public class MonsterPool
    {
        /**Singleton pool**/
        private static MonsterPool pool_instance = null;
        private static int _MAX_INSTANCES;
        private Queue<Monster> _pool;
        #region SINGLETON
        private MonsterPool(int pool_size)
        {
            if (pool_size <= 0)
                throw new ArgumentOutOfRangeException("pool_size", "Pool size must be positive int");
            _MAX_INSTANCES = pool_size;

            _pool = new Queue<Monster>(_MAX_INSTANCES);

            for (int i = 0; i < _MAX_INSTANCES; i++)
            {
                _pool.Enqueue(GetNewMonster("Placeholder"));
                Console.WriteLine($"Pool count {i} = {_pool.Count}");

            }

        }
        public static MonsterPool GetPoolInstance(int pool_size)
        {
            if (pool_instance == null)
            {
                pool_instance = new MonsterPool(pool_size);
            }
            return pool_instance;
        }
        #endregion
        public Monster GetMonster(string name)
        {
            Console.WriteLine("Is there any monsters free in the pool? ");
            if (_pool.Count > 0)
            {
                Console.WriteLine("Yes! Reconfigure the monster.");
                Monster monster0 = MonsterReconfiguring(_pool.Dequeue(), name);
                return monster0;
            }
            else
            {
                Console.WriteLine("No! Create new one.");
                Monster monster1 = GetNewMonster(name);
                _pool.Enqueue(monster1);
                return monster1;

            }
        }
        public Monster MonsterReconfiguring(Monster monster, string name)
        {
            Console.WriteLine($"Old name: {monster.name}");
            monster.HasSuperPowers = true;
            monster.name = name;
            return monster;
        }
        public void ReturnMonster(Monster monster)
        {
            Console.WriteLine($"Returning monster: {monster.name}, pool count {_pool.Count}");
            _pool.Enqueue(monster);
        }


        private Monster GetNewMonster(string monster)
        {

            return new Monster { name = monster, HasSuperPowers = false };
        }

    }
    public class Monster
    {
        public string name { get; set; }
        public bool HasSuperPowers { get; set; }
        public override string ToString()
        {
            return $"I am a monster and my name is: {name}. With superpowers? {HasSuperPowers}";
        }

    }
#if false
    public class MonsterFactory
    {
        public static Monster MakeMonster(string name)
        {
            switch (name.ToUpper())
            {
                case "WOLF":
                    var wolf = new Wolf { name = name, HasSuperPowers = false };
                    return wolf;
                case "DWARF":
                    var koyote = new Dwarf { name = name, HasSuperPowers = true };
                    return koyote;
                default:
                    return null;
            }
        }
    }
    public abstract class Monster
    {
        public string name { get; set; }
        public bool HasSuperPowers { get; set; }
        public override string ToString()
        {
            return $"I am a monster and my name is: {name}. With superpowers? {HasSuperPowers}";
        }
    }
    public class Wolf : Monster
    { }
    public class Dwarf : Monster
    { }
#endif

}
