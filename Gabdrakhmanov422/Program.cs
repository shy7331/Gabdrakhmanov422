using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gabdrakhmanov422
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.Start();
        }
    }

    class Game
    {
        private Player player;
        private Room[] dungeonMap;
        private Random random;

        public Game()
        {
            random = new Random();
            player = new Player();
            InitializeDungeon();
        }

        private void InitializeDungeon()
        {
            dungeonMap = new Room[10];
            for (int i = 0; i < 9; i++)
            {
                dungeonMap[i] = new Room(random.Next(5));
            }
            dungeonMap[9] = new Room(5);
        }

        public void Start()
        {
            Console.WriteLine("Ты вошел в подземелье!");
            for (int i = 0; i < dungeonMap.Length; i++)
            {
                Console.WriteLine($"\nТы вошел в следующую комнату под номером {i + 1}");

            
                OfferPotion(player);

                dungeonMap[i].Enter(player);
                if (player.Health <= 0)
                {
                    Console.WriteLine("Ты проиграл!");
                    break;
                }
                if (i == 9 && player.Health > 0)
                {
                    Console.WriteLine("Поздравляем вы победили босса и прошли 10 уровней подземелья!");
                }
            }

            Console.WriteLine("\nНажмите Продолжить, чтобы закрыть игру ");
            Console.ReadLine();
        }

        private void OfferPotion(Player player)
        {
            if (player.Potions > 0)
            {
                Console.WriteLine("Хотите выпить зелье перед входом в комнату? (1 - Да, 2 - Нет)");
                string choice = Console.ReadLine();
                if (choice == "1")
                {
                    player.UsePotion();
                }
            }
        }

        class Player
        {
            public int Health { get; set; } = 100;
            public int Potions { get; set; } = 3;
            public int Gold { get; set; } = 0;
            public int Arrows { get; set; } = 5;
            public string[] Inventory { get; set; } = new string[5];

            public void UsePotion()
            {
                if (Potions > 0)
                {
                    Health += 30;
                    if (Health > 100)
                    {
                        Health = 100;
                    }
                    Potions--;
                    Console.WriteLine($"Вы выпили зелье. Ваше здоровье теперь {Health}. Осталось зелий: {Potions}");
                }
                else
                {
                    Console.WriteLine("У вас нет зелий!");
                }
            }
        }

        class Room
        {
            private int type;
            private Random random;

            public Room(int type)
            {
                this.type = type;
                random = new Random();
            }

            public void Enter(Player player)
            {

                switch (type)
                {
                    case 0:
                        Console.WriteLine("Вы попали в комнату с монстром");
                        FightMonster(player);
                        break;
                    case 1:
                        Console.WriteLine("Вы попали в комнату с ловушкой");
                        TriggerTrap(player);
                        break;
                    case 2:
                        Console.WriteLine("Вы попали в комнату с сундуком, решите задачку, чтобы получить награду!");
                        OpenChest(player);
                        break;
                    case 3:
                        Console.WriteLine("Вы попали в пустую комнату");
                        break;
                    case 4:
                        Console.WriteLine("Вы попали в команту с торговцем");
                        MeetTrader(player);
                        break;
                    case 5:
                        Console.WriteLine("Вы попали в комнату с боссом!");
                        FightBoss(player);
                        break;

                }
            }


            private void FightMonster(Player player)
            {
                int monsterHealth = random.Next(20, 51);
                Console.WriteLine($"Монстр появился с {monsterHealth} ХП");
                while (monsterHealth > 0 && player.Health > 0)
                {
                    Console.WriteLine("Выберите оружие для атаки: (1) Меч (2) Лук");
                    string choice = Console.ReadLine();
                    if (choice == "1")
                    {
                        int damage = random.Next(10, 21);
                        monsterHealth -= damage;
                        Console.WriteLine($"Урон нанесенный монстру {damage} ");
                    }
                    else if (choice == "2" && player.Arrows > 0)
                    {
                        int damage = random.Next(5, 16);
                        monsterHealth -= damage;
                        player.Arrows--;
                        Console.WriteLine($"Урон нанесенный по монстру {damage}. Стрел осталось: {player.Arrows}");
                    }
                    else
                    {
                        Console.WriteLine("У вас нет стрел. Вы не можете использовать лук");
                        continue;
                    }

                    if (monsterHealth > 0)
                    {
                        int monsterDamage = random.Next(5, 16);
                        player.Health -= monsterDamage;
                        Console.WriteLine($"Монстр нанес вам {monsterDamage} урона. Ваше здоровье: {player.Health}");
                    }
                }
                if (player.Health > 0)
                {
                    Console.WriteLine("Вы победили монстра!");
                }
            }

            private void TriggerTrap(Player player)
            {
                int damage = random.Next(10, 21);
                player.Health -= damage;
                Console.WriteLine($"Вы настпуили в ловушку и потерли  {damage} здоровья. Ваше здоровье: {player.Health}");
            }

            private void OpenChest(Player player)
            {
                Console.WriteLine("Решите задачку для, того чтобы открыть сундук. Чему равно 2+2?");
                string answer = Console.ReadLine();
                if (answer == "4")
                {
                    Console.WriteLine("Правильно вы нашли 10 золотых, 3 стрелы и 1 зелье ");
                    player.Potions++;
                    player.Gold += 10;
                    player.Arrows += 3;

                }
                else
                {
                    Console.WriteLine("Неправильный ответ");
                }
            }

            private void FightBoss(Player player)
            {
                Console.WriteLine("Финальный босс появился");
                FightMonster(player);
            }

            private void MeetTrader(Player player)
            {
                Console.WriteLine("Вы попали в комнату с торговцем. Он вам прдлагает купить зелье за 30 золотых. (1 - Да, 2 - Нет)");
                string choice = Console.ReadLine();
                if (choice.ToLower() == "1" && player.Gold >= 30)
                {
                    player.Gold -= 30;
                    player.Potions++;
                    Console.WriteLine("Вы купили зелье");
                }
                else
                {
                    Console.WriteLine("У вас недостаточно золота, либо вы ответили нет. До свидания!");
                }
            }
        }
    }
}