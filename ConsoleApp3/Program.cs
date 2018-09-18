using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    class Program
    {
        class Phone
        {
            public string Name{ get; set; }
            public string Company{ get; set; }

        }
        class Player
        {
            public string Name{ get; set; }
            public string Team{ get; set; }
        }
        class Team
        {
            public string  Name{ get; set; }
            public string Country{ get; set; }
        }
        static void Main(string[] args)
        {
            //группировка
            List<Phone> phones = new List<Phone>
            {
                new Phone { Name = "Lumia 430",Company = "Microsoft"},
                new Phone { Name = "Mi 5",Company = "Xiaomi"},
                new Phone { Name = "G Pro",Company = "LG"},
                new Phone { Name = "iPhone 3GS",Company = "Apple"},
                new Phone { Name = "P 30",Company = "Huawei"},
                new Phone { Name = "M3",Company = "Meizu"},
                new Phone { Name = "XPeria Z",Company = "Sony"},
                new Phone { Name = "XPeria",Company = "Sony"},
                 new Phone { Name = "iPhone 6s",Company = "Apple"},
            };

            List<Team> teams = new List<Team>
            {
                new Team  { Name = "Бавария",Country = "Германия"},
                 new Team  { Name = "Барселона",Country = "Испания"},
                  new Team  { Name = "Динамо К.",Country = "Украина"},
                   new Team  { Name = "Ювентус",Country = "Италия"},
                    new Team  { Name = "ПСЖ",Country = "Франция"},
            };

            List<Player> players = new List<Player>
            {
                new Player{ Name = "Роналду",Team = "Ювентус"},
                 new Player{ Name = "Месси",Team = "Барселона"},
                  new Player{ Name = "Роббен",Team = "Бавария"},
                  new Player{ Name = "Зинченко",Team = "Динамо К."},
            };


            GroupingExample(phones);
            Console.WriteLine();
            Console.WriteLine();
            InnerQuery(phones);
            Console.WriteLine();
            Console.WriteLine();
            joinExample(teams,players);
            //Console.WriteLine();
            //Console.WriteLine();
            lazyExample(new String[] { "Бавария", "Ювентус", "ПСЖ", "Манчестер Юнайтед", "Реал Мадрид" });
        }

        private static void lazyExample(string[] teams)
        {
            //отложеный запрос
            var i = (from t in teams
                     where t.ToUpper().StartsWith("Б")
                     orderby t
                     select t);
            Console.WriteLine(i.Count());
            teams[1] = "ПСЖ";
            Console.WriteLine(i.Count());

            //немедленный запрос
            var selectTeams = teams.Where(t => t.ToUpper().StartsWith("Б")).OrderBy(t => t).ToList();
            
            foreach (var item in selectTeams)
            {
                Console.WriteLine(item);
            }
        }

        private static void joinExample(List<Team> teams, List<Player> players)
        {
            //запрос
            var result = from player in players
                         join team in teams on player.Team equals team.Name
                         select new { Name = player.Name, Team = player.Team,Country = team.Country };

            //метод
            result = players.Join(
                teams,//второй набор
                p =>p.Team,//свойство обьекта из первого набора
                t =>t.Country,//свойство обьекта из второго набора
                (p,t) => new { Name = p.Name, Team = p.Team, Country = t.Country }//результат
                );

            foreach (var item in result)
            {
                Console.WriteLine($"{item.Name},{item.Team},{item.Country}");
            }
        }

        private static void InnerQuery(List<Phone> phones)
        {
            //получение производителя,к-тва смартфонов и их моделей
            //запрос
            var phoneGroup2 = from phone in phones
                              group phone by phone.Company into g
                              select new {
                                  Name = g.Key,
                                  Count = g.Count(),
                                  Phones = from p in g select p
                              };
            //метод
            phoneGroup2 = phones.GroupBy(p => p.Company).
                Select(g => new
                {
                    Name = g.Key,
                    Count = g.Count(),
                    Phones = g.Select(p => p)
                });


            foreach (var group in phoneGroup2 )
            {
                Console.WriteLine($"{ group.Name}:{ group.Count}");
                foreach (Phone phone in group.Phones)
                {
                    Console.WriteLine($"{phone.Name}");
                }
                Console.WriteLine("_________");
            }
        }

        private static void GroupingExample(List<Phone> phones)
        {
            //запрос
            var phoneGroup = from phone in phones
                             group phone by phone.Company;
            //метод
            phoneGroup = phones.GroupBy(p => p.Company);
            //результат
            foreach (IGrouping<string, Phone> item in phoneGroup)
            {
                Console.WriteLine(item.Key);
                foreach (var t in item)
                {
                    Console.WriteLine(t.Name);

                }
                Console.WriteLine("__________");
            }

            //получение количества смартфонов определенного производителя
            //запрос
            var phoneGroup2 = from phone in phones
                              group phone by phone.Company into g
                              select new {Name = g.Key,Count = g.Count() };
            //метод
            phoneGroup2 = phones.GroupBy(p => p.Company).
                Select(g => new { Name = g.Key, Count = g.Count() });
            //результат
            foreach (var item in phoneGroup2)
            {
                Console.WriteLine($"{ item.Name}:{ item.Count}");
            }
            
        }
    }
}
