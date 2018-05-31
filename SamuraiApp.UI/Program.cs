using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Domain;
using System;
using System.Linq;

namespace SamuraiApp.UI
{
    class Program
    {
        private static SamuraiContext _context = new SamuraiContext();

        static void Main(string[] args)
        {
            /**
             * MANY TO MANY RELATIONSHIPS
             */

            // PrePopulateSamuraisAndBattles();

            // JoinBattleAndSamurai();

            // EnlistSamuraiIntoABattle();

            // EnlistSamuraiIntoABattleUntracked();

            // AddNewSamuraiViaDisconnectedBattleObject();

            // GetSamuraiWithBattles(8);

            // RemoveJoinBetweenSamuraiAndBattleSimple();

            // RemoveBattleFromSamurai();

            Console.ReadLine();
        }

        // Remove battle from already in memory samurai
        private static void RemoveBattleFromSamurai()
        {
            // Remove join between Shichiroji (10) and Battle of Okehazama (5)
            var samurai = _context.Samurais
                .Include(s => s.SamuraiBattles)
                .ThenInclude(sb => sb.Battle)
                .FirstOrDefault(s => s.Id == 10);

            var sbToRemove = samurai.SamuraiBattles.SingleOrDefault(sb => sb.BattleId == 5);

            // This only works if the context is the same
            samurai.SamuraiBattles.Remove(sbToRemove);
            // If not, you must use this instead.
            // _context.Attach(sbToRemove);
            // _context.Remove(sbToRemove);

            _context.SaveChanges();
        }

        // Simple removal of join
        private static void RemoveJoinBetweenSamuraiAndBattleSimple()
        {
            var join = new SamuraiBattle { BattleId = 5, SamuraiId = 15 };
            _context.Remove(join);
            _context.SaveChanges();
        }
        
        // Query samurais with battles
        private static void GetSamuraiWithBattles(int id)
        {
            var samuraiWithBattles = _context.Samurais
                .Include(s => s.SamuraiBattles)
                .ThenInclude(sb => sb.Battle)
                .FirstOrDefault(s => s.Id == id);

            var battles = samuraiWithBattles.SamuraiBattles.Select(sb => sb.Battle).ToList();

            foreach (Battle battle in battles)
            {
                Console.WriteLine(battle.Name);
            }
        }

        // Add new samurai at the same time
        private static void AddNewSamuraiViaDisconnectedBattleObject()
        {
            Battle battle;
            using (var seperateOperation = new SamuraiContext())
            {
                battle = seperateOperation.Battles.Find(5);
            }
            var newSamurai = new Samurai { Name = "SampsonSan" };
            battle.SamuraiBattles.Add(new SamuraiBattle { Samurai = newSamurai });
            _context.Battles.Attach(battle);
            _context.SaveChanges();
        }

        // Add samurai battle with battle already added
        private static void EnlistSamuraiIntoABattleUntracked()
        {
            Battle battle;

            using (var separateOperation = new SamuraiContext())
            {
                battle = separateOperation.Battles.Find(5);
            }

            battle.SamuraiBattles.Add(new SamuraiBattle { SamuraiId = 9 });

            _context.Battles.Attach(battle);
            _context.SaveChanges();
        }

        // Add Samurai to battle
        private static void EnlistSamuraiIntoABattle()
        {
            var battle = _context.Battles.Find(5);

            battle.SamuraiBattles.Add(new SamuraiBattle { SamuraiId = 10 });

            _context.SaveChanges();
        }

        // Add already created samurai and battle
        private static void JoinBattleAndSamurai()
        {
            // Kikuchiyo id is 8, Siege of Osaka is 7
            var sbJoin = new SamuraiBattle { SamuraiId = 8, BattleId = 7 };
            _context.Add(sbJoin);
            _context.SaveChanges();
        }

        private static void PrePopulateSamuraisAndBattles()
        {
            _context.AddRange(
                new Samurai { Name = "Kikuchiyo" },
                new Samurai { Name = "Kambei Shimada" },
                new Samurai { Name = "Shichiroji" },
                new Samurai { Name = "Katsushiro Okamoto" },
                new Samurai { Name = "Heihachi Hayashida" },
                new Samurai { Name = "Kyuzo" },
                new Samurai { Name = "Gorobei Katayama" }
            );

            _context.Battles.AddRange(
                new Battle { Name = "Battle of Okehazama", StartDate = new DateTime(1560, 05, 01), EndDate = new DateTime(1560, 06, 15)},    
                new Battle { Name = "Battle of Shiroyama", StartDate = new DateTime(1877, 09, 02), EndDate = new DateTime(1877, 09, 22)},    
                new Battle { Name = "Siege of Osaka", StartDate = new DateTime(1614, 01, 01), EndDate = new DateTime(1615, 12, 31)},    
                new Battle { Name = "Boshin War", StartDate = new DateTime(1868, 01, 01), EndDate = new DateTime(1869, 01, 01)}
            );

            _context.SaveChanges();
        }
    }
}