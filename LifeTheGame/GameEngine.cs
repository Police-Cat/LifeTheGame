using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifeTheGame
{
    /// <summary>
    /// Игровая логика
    /// </summary>
    public class GameEngine
    {
        #region Fields

        private bool[,] cells;
        private readonly int columns;
        private readonly int rows;

        #endregion

        #region Properties

        public int CurrentGeneration { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Инициализует новый экземляр класcа GameEngine с готовым паттерном "Conway's Game of Life"
        /// </summary>
        /// <param name="rows">Количество рядов клеток</param>
        /// <param name="columns">Количество колонок клеток</param>
        public GameEngine(int rows, int columns)
        {
            this.rows = rows;
            this.columns = columns;
            cells = new bool[columns, rows];

            // Генерирует паттерн игры "Conway's Game of Life"
            FirstGeneration();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Генерирует новое поколение клеток
        /// </summary>
        public void NextGeneration()
        { 
            bool[,] newCells = new bool[columns, rows];

            // Изменяет состояние каждой клетки в зависимости от количества соседей
            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    int neighboursCount = CountNeighbours(x, y);
                    bool isAlive = cells[x, y];

                    if (isAlive == false && neighboursCount == 3)
                    {
                        newCells[x, y] = true;
                    }
                    else if (isAlive && (neighboursCount < 2 || neighboursCount > 3))
                    {
                        newCells[x, y] = false;
                    }
                    else
                    {
                        newCells[x, y] = cells[x, y];
                    }
                }
            }
            cells = newCells;
            CurrentGeneration++;
        }

        /// <returns>Копия массива клеток</returns>
        public bool[,] GetCurrentGeneration()
        {
            bool[,] result = new bool[columns, rows];
            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    result[x, y] = cells[x, y];
                }
            }
            return result;
        }
    
        /// <summary>
        /// Добавляет клетку в ячейку
        /// </summary>
        /// <param name="x">Номер колонки</param>
        /// <param name="y">Номер ряда</param>
        public void AddCell(int x, int y)
        {
            UpdateCell(x, y, state: true);
        }

        /// <summary>
        /// Убирает клетку из ячейки
        /// </summary>
        /// <param name="x">Номер колонки</param>
        /// <param name="y">Номер ряда</param>
        public void RemoveCell(int x, int y)
        {
            UpdateCell(x, y, state: false);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Генерирует один из паттернов "Conway's Game of Life"
        /// </summary>
        private void FirstGeneration()
        {
            int startX = columns / 2;
            int startY = rows / 2;
;
            cells[startX, startY] = true;
            cells[startX, startY + 4] = true;
            cells[startX + 1, startY + 1] = true;
            cells[startX + 1, startY + 2] = true;
            cells[startX + 1, startY + 3] = true;
            cells[startX - 1, startY + 1] = true;
            cells[startX - 1, startY + 2] = true;
            cells[startX - 1, startY + 3] = true;
            cells[startX - 2, startY + 2] = true;
            cells[startX + 2, startY + 2] = true;
        }

        /// <summary>
        /// Считает соседние клетки
        /// </summary>
        /// <param name="x">Номер колонки</param>
        /// <param name="y">Номер ряда</param>
        /// <returns></returns>
        private int CountNeighbours(int x, int y)
        {
            int count = 0;

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    int column = (x + i + columns) % columns;
                    int row = (y + j + rows) % rows;

                    bool isSelfCheck = column == x && row == y;
                    bool isAlive = cells[column, row];

                    if (isAlive && isSelfCheck == false)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        /// <summary>
        /// Проверяет на выход за пределы экрана
        /// </summary>
        /// <param name="x">Номер колонки</param>
        /// <param name="y">Номер ряда</param>
        /// <returns></returns>
        private bool ValidateCellPosition(int x, int y)
        {
            return x >= 0 && y >= 0 && x < columns && y < rows;
        }

        /// <summary>
        /// Обновляет состояние в ячейке 
        /// </summary>
        /// <param name="x">Номер колонки</param>
        /// <param name="y">Номер ряда</param>
        /// <param name="state">Состояние</param>
        private void UpdateCell(int x, int y, bool state)
        {
            if (ValidateCellPosition(x, y))
            {
                cells[x, y] = state;
            }
        }

        #endregion
    }
}
