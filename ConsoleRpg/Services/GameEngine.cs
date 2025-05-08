using ConsoleRpg.Helpers;
using ConsoleRpgEntities.Data;
using ConsoleRpgEntities.Models.Attributes;
using ConsoleRpgEntities.Models.Characters;
using ConsoleRpgEntities.Models.Characters.Monsters;

namespace ConsoleRpg.Services
{
    public class GameEngine
    {
        private readonly GameContext _context;
        private readonly MenuManager _menuManager;
        private readonly OutputManager _outputManager;

        private IPlayer? _player; // Marked as nullable
        private IMonster? _goblin; // Marked as nullable

        public GameEngine(GameContext context, MenuManager menuManager, OutputManager outputManager)
        {
            _menuManager = menuManager;
            _outputManager = outputManager;
            _context = context;
        }

        public void Run()
        {
            if (_menuManager.ShowMainMenu())
            {
                SetupGame();
            }
        }

        private void GameLoop()
        {
            _outputManager.Clear();

            while (true)
            {
                _outputManager.WriteLine("Choose an action:", ConsoleColor.Cyan);
                _outputManager.WriteLine("1. Attack");
                _outputManager.WriteLine("2. Quit");

                _outputManager.Display();

                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        AttackCharacter();
                        break;
                    case "2":
                        _outputManager.WriteLine("Exiting game...", ConsoleColor.Red);
                        _outputManager.Display();
                        Environment.Exit(0);
                        break;
                    default:
                        _outputManager.WriteLine("Invalid selection. Please choose 1.", ConsoleColor.Red);
                        break;
                }
            }
        }

        private void AttackCharacter()
        {
            if (_goblin is Goblin goblin) // Cast to Goblin to access Health
            {
                // Player attacks Goblin
                if (_player is Player player) // Cast to Player to access Health
                {
                    int playerAttackValue = GetWeaponAttackPower(player.Equipment?.Weapon);
                    int goblinDefenseValue = GetArmorDefenseValue(null); // Goblin has no Equipment property

                    int damageToGoblin = Math.Max(playerAttackValue - goblinDefenseValue, 0);
                    goblin.Health -= damageToGoblin;

                    _outputManager.WriteLine($"{player.Name} attacks {goblin.Name} with {(player.Equipment?.Weapon ?? "bare hands")}, dealing {damageToGoblin} damage!", ConsoleColor.Yellow);

                    // Goblin retaliates and attacks the player
                    int goblinAttackValue = 10; // Default goblin attack power
                    int playerDefenseValue = GetArmorDefenseValue(player.Equipment?.Armor);

                    int damageToPlayer = Math.Max(goblinAttackValue - playerDefenseValue, 0);
                    player.Health -= damageToPlayer;

                    _outputManager.WriteLine($"{goblin.Name} retaliates and attacks {player.Name}, dealing {damageToPlayer} damage!", ConsoleColor.Red);
                }
                else
                {
                    _outputManager.WriteLine("Player is not initialized and cannot attack!", ConsoleColor.Red);
                }
            }
            else
            {
                _outputManager.WriteLine("No goblin to attack!", ConsoleColor.Red);
            }
        }

        private int GetWeaponAttackPower(string? weaponName)
        {
            // Simulate weapon attack power based on weapon name
            return weaponName switch
            {
                "Sword" => 15,
                "Axe" => 20,
                _ => 5 // Default attack power for bare hands or unknown weapons
            };
        }

        private int GetArmorDefenseValue(string? armorName)
        {
            // Simulate armor defense value based on armor name
            return armorName switch
            {
                "Shield" => 10,
                "Leather Armor" => 5,
                _ => 0 // Default defense value for no armor or unknown armor
            };
        }

        private void SetupGame()
        {
            _player = _context.Players.OfType<Player>().FirstOrDefault();
            if (_player == null)
            {
                throw new InvalidOperationException("No player found in the database.");
            }
            _outputManager.WriteLine($"{_player.Name} has entered the game.", ConsoleColor.Green);

            LoadMonsters();
            Thread.Sleep(500);
            GameLoop();
        }

        private void LoadMonsters()
        {
            _goblin = _context.Monsters.OfType<Goblin>().FirstOrDefault();
            if (_goblin == null)
            {
                throw new InvalidOperationException("No goblin found in the database.");
            }
        }
    }
}

