using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

class SnakeGame
{
    const int Width = 30, Height = 20;   // tamaño del tablero (en celdas)
    const int Delay = 100;               // milisegundos por movimiento

    static List<(int X, int Y)> snake = new();
    static (int X, int Y) dir, food;
    static int score, best;
    static readonly Random rng = new();

    static void SetCursorSafe(bool visible)
    {
        try { Console.CursorVisible = visible; } catch { }
    }

    static void Main()
    {
        try { Console.OutputEncoding = Encoding.UTF8; } catch { }
        SetCursorSafe(false);

        do { Play(); } while (AskAgain());

        SetCursorSafe(true);
        try { Console.SetCursorPosition(0, Height + 4); } catch { }
    }

    static void Play()
    {
        Console.Clear();
        DrawBorder();

        snake = new() { (Width / 2, Height / 2), (Width / 2 - 1, Height / 2), (Width / 2 - 2, Height / 2) };
        dir = (1, 0);
        score = 0;

        foreach (var s in snake) DrawCell(s, "██", ConsoleColor.Green);
        PlaceFood();
        UpdateScore();

        var next = dir;
        while (true)
        {
            // Leer teclas (flechas o WASD) sin bloquear el juego
            while (Console.KeyAvailable)
            {
                var key = Console.ReadKey(true).Key;
                (int X, int Y) d = key switch
                {
                    ConsoleKey.UpArrow or ConsoleKey.W => (0, -1),
                    ConsoleKey.DownArrow or ConsoleKey.S => (0, 1),
                    ConsoleKey.LeftArrow or ConsoleKey.A => (-1, 0),
                    ConsoleKey.RightArrow or ConsoleKey.D => (1, 0),
                    _ => next
                };

                // No permitir girar 180° sobre sí misma
                if (d != (-dir.X, -dir.Y)) next = d;
            }
            dir = next;

            var head = (X: snake[0].X + dir.X, Y: snake[0].Y + dir.Y);
            bool eat = head == food;

            // Si no come, la cola se mueve, así que esa celda queda libre
            var body = eat ? snake : snake.GetRange(0, snake.Count - 1);
            bool hitWall = head.X < 0 || head.Y < 0 || head.X >= Width || head.Y >= Height;
            if (hitWall || body.Contains(head)) break;

            if (!eat)
            {
                DrawCell(snake[^1], "  ", ConsoleColor.Black);
                snake.RemoveAt(snake.Count - 1);
            }

            snake.Insert(0, head);
            DrawCell(head, "██", ConsoleColor.Green);

            if (eat)
            {
                score++;
                best = Math.Max(best, score);
                PlaceFood();
                UpdateScore();
            }

            Thread.Sleep(Delay);
        }

        Console.ResetColor();
        try { Console.SetCursorPosition(Math.Max(0, Width - 4), Height / 2); } catch { }
        Console.Write(" GAME OVER ");
    }

    static void PlaceFood()
    {
        do
        {
            food = (rng.Next(Width), rng.Next(Height));
        } while (snake.Contains(food));

        DrawCell(food, "● ", ConsoleColor.Red);
    }

    static void DrawCell((int X, int Y) p, string text, ConsoleColor color)
    {
        try
        {
            Console.SetCursorPosition(p.X * 2 + 1, p.Y + 1);
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }
        catch { }
    }

    static void DrawBorder()
    {
        try
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            string line = new string('#', Width * 2 + 2);
            Console.SetCursorPosition(0, 0);
            Console.Write(line);
            Console.SetCursorPosition(0, Height + 1);
            Console.Write(line);
            for (int y = 1; y <= Height; y++)
            {
                Console.SetCursorPosition(0, y);
                Console.Write('#');
                Console.SetCursorPosition(Width * 2 + 1, y);
                Console.Write('#');
            }
            Console.ResetColor();
        }
        catch { }
    }

    static void UpdateScore()
    {
        try
        {
            Console.SetCursorPosition(0, Height + 2);
            Console.Write($"Puntos: {score}   Récord: {best}   (flechas/WASD para mover)   ");
        }
        catch { }
    }

    static bool AskAgain()
    {
        try
        {
            Console.SetCursorPosition(0, Height + 3);
            Console.Write("¿Jugar de nuevo? (S/N): ");
        }
        catch { }

        while (Console.KeyAvailable) Console.ReadKey(true);
        while (true)
        {
            var key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.S) return true;
            if (key == ConsoleKey.N) return false;
        }
    }
}