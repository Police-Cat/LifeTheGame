using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LifeTheGame
{
    /// <summary>
    /// Взаимодействие с пользователем
    /// </summary>
    public partial class MainForm : Form
    {
        #region Fields

        private Graphics graphics;
        private GameEngine gameEngine;

        private readonly int cellSize = 10;

        #endregion

        #region Constructor

        /// <summary>
        /// Инициализует новый экземляр класcа MainForm унаследованный от Form
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Начинает игру, запускает таймер 
        /// </summary>
        private void StartGame()
        {
            if (timer.Enabled)
                return;

            timer.Start();
        }

        /// <summary>
        /// Останавливает игру
        /// </summary>
        private void StopGame()
        {
            if (timer.Enabled == false)
                return;
            timer.Stop();
        }

        /// <summary>
        /// Рисует текущее поколение
        /// </summary>
        private void DrawCurrentGeneration()
        {
            graphics.Clear(Color.White);
            var cells = gameEngine.GetCurrentGeneration();

            for (int x = 0; x < cells.GetLength(0); x++)
            {
                for (int y = 0; y < cells.GetLength(1); y++)
                {
                    if (cells[x, y])
                    {
                        graphics.FillRectangle(Brushes.Aqua, x * cellSize, y * cellSize, cellSize - 1, cellSize - 1);
                    }
                }
            }
            GameField.Refresh();
        }

        /// <summary>
        /// Рисует следующее поколение
        /// </summary>
        private void DrawNextGeneration()
        {
            gameEngine.NextGeneration();
            DrawCurrentGeneration();
            GameField.Refresh();
            this.Text = $"Generation Number: {gameEngine.CurrentGeneration}";
        }
        
        /// <summary>
        /// Добавляет или убирает клетку с поля в зависимости от нажатой клавиши мыши
        /// </summary>
        /// <param name="e"></param>
        private void ProcessMousePress(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int x = e.Location.X / cellSize;
                int y = e.Location.Y / cellSize;
                gameEngine.AddCell(x, y);
                DrawCurrentGeneration();
            }

            if (e.Button == MouseButtons.Right)
            {
                int x = e.Location.X / cellSize;
                int y = e.Location.Y / cellSize;
                gameEngine.RemoveCell(x, y);
                DrawCurrentGeneration();
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Обрабатывает загрузку формы, устанавливает количество полей и колонок
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainFormLoad(object sender, EventArgs e)
        {
            if (timer.Enabled)
                return;

            gameEngine = new GameEngine(
                rows: GameField.Height / cellSize,
                columns: GameField.Width / cellSize
                );

            this.Text = $"Generation Number: {gameEngine.CurrentGeneration}";
            GameField.Image = new Bitmap(GameField.Width, GameField.Height);
            graphics = Graphics.FromImage(GameField.Image);

            // Отрисовывает первое поколение 
            DrawCurrentGeneration();
        }

        /// <summary>
        /// Отрисовывает следующее поколение, обрабатывает каждый тик таймера
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTimerTick(object sender, EventArgs e)
        {
            DrawNextGeneration();
        }

        /// <summary>
        /// Начинает игру, обрабатывает нажатие на кнопку старт
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnButtonStartClick(object sender, EventArgs e)
        {
            StartGame();
        }

        /// <summary>
        /// Останавливает игру, обрабатывает нажатие на кнопку стоп
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnButtonStopClick(object sender, EventArgs e)
        {
            StopGame();
        }

        /// <summary>
        /// Обрабатывает добавление и удаления клеток при одиночном нажатии на клавишу мыши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            if (timer.Enabled)
                return;

            ProcessMousePress(e);
        }

        /// <summary>
        /// Обрабатывает добавление и удаления клеток с зажатой клавишой мыши
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (timer.Enabled)
                return;

            ProcessMousePress(e);
        }

        #endregion
    }
}
