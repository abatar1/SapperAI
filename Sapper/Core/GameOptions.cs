using System.ComponentModel;

namespace Sapper.Core
{
    public class GameOptions
    {
        [DisplayName("Ширина")]
        public int Width { get; set; } = 20;

        [DisplayName("Высота")]
        public int Height { get; set; } = 20;

        [DisplayName("Степень искажения")]
        public double GeneratorProbability { get; set; } = 0.1;

        [DisplayName("Число бомб")]
        public int NumberOfBombs { get; set; } = 50;

        [DisplayName("Задержка на ход в миллисекундах")]
        public int DelayInMilliseconds { get; set; } = 2000;

        [DisplayName("Радиус без бомб")]
        public int RaduisWithoutBombs { get; set; } = 5;

        [DisplayName("Путь к ИИ")]
        public string PlayerController { get; set; } = "SapperAI.dll";
    }
}
