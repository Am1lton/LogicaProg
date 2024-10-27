using Microsoft.VisualBasic.FileIO;
using System;
using System.Reflection.Emit;
using System.Runtime.ConstrainedExecution;
using System.Timers;


class Game
{
    static private int dificuldade = 0;
    static private float pontuacao = 0;
    private const int ALTURA_RUA = 10;
    private const int LARGURA_RUA = 5;
    static private char iconeBoneco = '@';
    static private char iconeCarro = '^';
    static private char[,] rua = new char[LARGURA_RUA + 2, ALTURA_RUA + 2];
    static private bool fimDeJogo = false;
    static private System.Timers.Timer timerMovimentoCarros;
    static private System.Timers.Timer timerGeracaoCarros;
    static private List<(int x, int y)> carros = [];
    static private Random rng = new Random();

    static void Main()
    {
        // Loop do menu

        while(true)
        {
            Console.Clear();
            Console.WriteLine("Bem-vindo ao jogo de desviar de carros!");
            Console.WriteLine("Selecione a dificuldade ( 1 | 2 | 3 )");
            string input = Console.ReadLine() ?? string.Empty;

            if (input != string.Empty)
            {
                char firstChar = input.Trim().First();

                switch (firstChar)
                {
                    case '1':
                        dificuldade = 700;
                        break;
                    case '2':
                        dificuldade = 500;
                        break;
                    case '3':
                        dificuldade = 300;
                        break;
                }

                if (dificuldade != 0)
                    break;
            }
        }
        Console.Clear();


        Boneco boneco = new Boneco();
        timerMovimentoCarros = new System.Timers.Timer(dificuldade);
        timerMovimentoCarros.Elapsed += MovimentarCarros;
        timerMovimentoCarros.AutoReset = true;

        timerGeracaoCarros = new System.Timers.Timer(dificuldade * 3 - 129);
        timerGeracaoCarros.Elapsed += GerarCarros;
        timerGeracaoCarros.AutoReset = true;

        Console.CursorVisible = false;

        InicializarRua();

        rua[4, 10] = iconeCarro;

        for (int i = 0; i < rua.GetLength(1); i++)
        {
            for (int j = 0; j < rua.GetLength(0); j++)
            {
                Console.Write(rua[j, i]);
            }
            Console.WriteLine();
        }

        carros.Add((4, 10));

        timerMovimentoCarros.Enabled = true;
        timerGeracaoCarros.Enabled = true;
        // Loop do jogo
        while (!fimDeJogo)
        {
            if (Console.KeyAvailable)
            {
                // Checando input do player
                ConsoleKeyInfo key = Console.ReadKey(true);

                // Movendo o player
                switch (key.Key)
                {
                    case ConsoleKey.LeftArrow:
                        boneco.MoverBoneco(true);
                        break;
                    case ConsoleKey.RightArrow:
                        boneco.MoverBoneco(false);
                        break;
                }
            }
        }
        Console.SetCursorPosition(0, ALTURA_RUA + 5);
        Thread.Sleep(100);
        Console.WriteLine();
    }

    public static void AtualizarPontuacao()
    {
        Console.SetCursorPosition(0, rua.GetLength(1) + 1);
        Console.Write("Pontuação: " + pontuacao + new string(' ', 10));
    }

    private static void GerarCarros(object source, ElapsedEventArgs e)
    {
        int xAleatorio = rng.Next(1, 6);
        carros.Add((xAleatorio, ALTURA_RUA));
        rua[xAleatorio, ALTURA_RUA] = iconeCarro;
        AtualizarObjeto(xAleatorio, ALTURA_RUA, xAleatorio, ALTURA_RUA, iconeCarro);
        pontuacao += 10.0f - (dificuldade / 100.0f);
        AtualizarPontuacao();
    }

    private static void MovimentarCarros(object source, ElapsedEventArgs e)
    {
        for(int i = 0; i < carros.Count; i++)
        {
            (int x, int y) a = carros[i];
            a.y--;
            if (a.y < 1)
            {
                AtualizarObjeto(carros[i].x, carros[i].y, carros[i].x, carros[i].y, ' ');
                carros.RemoveAt(i);
                i--;
            }
            else
            { 
                ChecarColisao(carros[i].x, carros[i].y - 1, iconeBoneco);
                AtualizarObjeto(carros[i].x, carros[i].y, carros[i].x, carros[i].y - 1, iconeCarro);
                rua[carros[i].x, carros[i].y] = ' ';
                carros[i] = a;
            }
        }
    }

    static public void InicializarRua()
    {
        for (int i = 0; i < rua.GetLength(1); i++)
        {
            for(int j = 0; j < rua.GetLength(0); j++)
            {
                if (i == 0 || j == 0 || i > ALTURA_RUA || j > LARGURA_RUA)
                {
                    rua[j, i] = '█';
                }
                else
                {
                    rua[j, i] = ' ';
                }
            }
        }

        rua[1, 1] = iconeBoneco;
    }

    static public void ChecarColisao(int posX, int posY, char objeto)
    {
        if (rua[posX, posY] == objeto)
        {
            fimDeJogo = true;
        }
    }

    static public void AtualizarObjeto(int posX, int posY, int novaPosX, int novaPosY, char objeto)
    {
        rua[posX, posY] = ' ';
        rua[novaPosX, novaPosY] = objeto;

        Console.SetCursorPosition(posX, posY);
        Console.Write(' ');
        Console.SetCursorPosition(novaPosX, novaPosY);
        Console.Write(objeto);
    }

    public class Boneco
    {
        private int posicao = 1;

        public void MoverBoneco(bool dir)
        {
            int novaPosicao = posicao - (dir ? 1 : -1);
            if (novaPosicao < 1 || novaPosicao > rua.GetLength(0) - 2)
            {
                return;
            }

            ChecarColisao(novaPosicao, 1, iconeCarro);

            AtualizarObjeto(posicao, 1, novaPosicao, 1, iconeBoneco);
            posicao = novaPosicao;
        }
    }
}