using HeroesApi;
using HeroesApi.interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace HeroesApiTest
{
    class HeroesServiceFake : IHeroesService
    {

        private static readonly string data_path = System.IO.Directory.GetCurrentDirectory() + "\\data\\heroes.json";
        private static List<Hero> heroes;

        public HeroesServiceFake()
        {
            heroes = new List<Hero>();
            load();
        }

        public Hero Add(Hero hero)
        {
            hero.Id = GenerateId();
            heroes.Add(hero);
            return hero;
        }

        public int GenerateId()
        {
            int max = 1;
            foreach (Hero hero in heroes)
            {
                int id = hero.Id;
                if (id > max)
                {
                    max = id;
                }
            }
            return max++;
        }

        public Hero GetById(int id)
        {
            foreach (Hero hero in heroes)
            {
                if (hero.Id == id)
                {
                    return hero;
                }
            }
            return null;
        }

        public IEnumerable<Hero> GetHeroes()
        {
            return heroes;
        }

        public void Remove(int id)
        {
            Hero hero = GetById(id);
            if (hero != null)
            {
                heroes.Remove(hero);
            }
        }

        public IEnumerable<Hero> SearchHeroes(string name)
        {
            List<Hero> filtered = new List<Hero>();
            name = name.ToLower();
            foreach (Hero hero in heroes)
            {
                string heroName = hero.Name;
                if (heroName.Contains(name))
                {
                    filtered.Add(hero);
                }
            }
            return filtered;
        }

        public Hero Update(Hero hero)
        {
            int x = -1;
            for (int i = 0; i < heroes.Count; i++)
            {
                if (heroes[i].Id == hero.Id)
                {
                    x = i;
                    break;
                }
            }
            if (x != -1)
            {
                heroes[x] = hero;
                Console.WriteLine("Updating Hero " + heroes[x].Power + " to " + hero.Power);
            }
            return heroes[x];
        }

        public static void save()
        {
            string json = JsonConvert.SerializeObject(heroes.ToArray());
            System.IO.File.WriteAllText(data_path, json);
        }

        private void load()
        {
            String data = System.IO.File.ReadAllText(data_path);
            heroes.AddRange(JsonConvert.DeserializeObject<IEnumerable<Hero>>(data));
        }

    }
}
