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
            if (_goblin is ITargetable targetableGoblin)
            {
                // Player attacks Goblin using weapon's attack power
                if (_player?.Equipment?.Weapon != null)
                {
                    _outputManager.WriteLine($"{_player.Name} attacks {_goblin.Name} with {_player.Equipment.Weapon}, dealing 10 damage!", ConsoleColor.Yellow);
                    _player.Attack(targetableGoblin);
                }
                else
                {
                    _outputManager.WriteLine($"{_player?.Name ?? "Unknown Player"} has no weapon equipped and cannot attack!", ConsoleColor.Red);
                }

                // Goblin retaliates and attacks the player
                _goblin.Attack((ITargetable)_player!); // Explicit cast to ITargetable
            }
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

