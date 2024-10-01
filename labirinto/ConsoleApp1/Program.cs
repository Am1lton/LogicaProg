using System;

namespace AventuraNoLabirinto
{
    class Program
    {
        // Definição dos símbolos usados no labirinto
        const char PAREDE = '█';
        const char CAMINHO = ' ';
        const char JOGADOR = '@';
        const char ITEM = '*';

        // Dimensões do labirinto
        const int ALTURA = 20;
        const int LARGURA = 40;
        static void Main(string[] args)
        {
            // Variáveis do jogo
            sbyte[,,] item = new sbyte[5,1,1];
            char[,] menu = new char[4, 13];
            char[,] labirinto = new char[ALTURA, LARGURA];
            int jogadorLinha = 2;
            int jogadorColuna = 3;
            int vidas = 3;
            int itensColetados = 0;
            bool jogoAtivo = true;
            sbyte dificuldade = 1;
            int pontuacao = 0;
            List<(sbyte, sbyte)> items = [];
            List<(sbyte, sbyte)> armadilhas = [];
            List<(sbyte, sbyte)> curas = [];

            InicializarMenu(menu);
            menu[jogadorLinha, jogadorColuna] = JOGADOR;
            DesenharLabirinto(menu);
            Console.SetCursorPosition(0, 5);
            Console.WriteLine("Selecione a dificuldade e pressione 'Enter' para comoeçar");

            // Menu do jogo
            while (jogoAtivo)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo tecla = Console.ReadKey(true);
                    int novaColuna = jogadorColuna;

                    // Atualiza a posição do jogador com base na tecla pressionada
                    switch (tecla.Key)
                    {
                        case ConsoleKey.LeftArrow:
                            novaColuna-= 3;
                            break;
                        case ConsoleKey.RightArrow:
                            novaColuna+= 3;
                            break;
                        case ConsoleKey.Enter:
                            dificuldade = (sbyte)(jogadorColuna / 3);
                            jogoAtivo = false;
                            break;
                        default:
                            continue; // Ignora outras teclas
                    }

                    // Verifica se a nova posição está dentro dos limites
                    if (novaColuna < 0 || novaColuna >= menu.GetLength(1))
                    {
                        // Bateu na borda do labirinto
                        continue;
                    }

                    char proximoPasso = menu[jogadorLinha, novaColuna];

                    // Verifica o conteúdo da próxima posição
                    if (proximoPasso == PAREDE)
                    {
                        // Bateu em uma parede
                        continue;
                    }
                    else
                    {
                        // Remove o jogador da posição atual
                        AtualizarPosicao(jogadorLinha, jogadorColuna, ' ');

                        jogadorColuna = novaColuna;
                    }

                    // Atualiza a posição do jogador no menu
                    menu[jogadorLinha, jogadorColuna] = JOGADOR;
                    AtualizarPosicao(jogadorLinha, jogadorColuna, JOGADOR);
                }
            }
            jogoAtivo = true;


            // Define a quantidade de armadilhas e items baseado na dificuldade
            switch (dificuldade)
            {
                case 1:
                    dificuldade = 24;
                    break;
                case 2:
                    dificuldade = 48;
                    break;
                case 3:
                    dificuldade = 72;
                    break;
                default:
                    break;
            }

            //Jogo
            while (jogoAtivo)
            {
                itensColetados = 0;
                items.Clear();
                armadilhas.Clear();
                // Inicialização do labirinto
                InicializarLabirinto(labirinto, dificuldade, ref items, ref armadilhas, ref curas);

                // Colocar o jogador na posição inicial
                jogadorLinha = 1;
                jogadorColuna = 1;
                labirinto[jogadorLinha, jogadorColuna] = JOGADOR;

                // Desenha o labirinto inicialmente
                Console.Clear();
                DesenharLabirinto(labirinto);
                ExibirInformacoes(vidas, itensColetados, (sbyte)(dificuldade / 24), pontuacao);

                while (true)
                {
                    // Captura a tecla pressionada pelo jogador sem bloquear a thread
                    if (Console.KeyAvailable)
                    {

                        ConsoleKeyInfo tecla = Console.ReadKey(true);
                        if (tecla.Key == ConsoleKey.R) { break; }

                        int novaLinha = jogadorLinha;
                        int novaColuna = jogadorColuna;

                        // Atualiza a posição do jogador com base na tecla pressionada
                        switch (tecla.Key)
                        {
                            case ConsoleKey.UpArrow:
                                novaLinha--;
                                break;
                            case ConsoleKey.DownArrow:
                                novaLinha++;
                                break;
                            case ConsoleKey.LeftArrow:
                                novaColuna--;
                                break;
                            case ConsoleKey.RightArrow:
                                novaColuna++;
                                break;
                            default:
                                continue; // Ignora outras teclas
                        }

                        // Verifica se a nova posição está dentro dos limites
                        if (novaLinha < 0 || novaLinha >= ALTURA || novaColuna < 0 || novaColuna >= LARGURA)
                        {
                            // Bateu na borda do labirinto
                            continue;
                        }

                        char proximoPasso = labirinto[novaLinha, novaColuna];

                        // Verifica o conteúdo da próxima posição
                        if (proximoPasso == PAREDE)
                        {
                            // Bateu em uma parede
                            continue;
                        }
                        else
                        {
                            // Remove o jogador da posição atual
                            AtualizarPosicao(jogadorLinha, jogadorColuna, ' ');

                            jogadorLinha = novaLinha;
                            jogadorColuna = novaColuna;

                            // Colisão com items
                            if (items.Contains(((sbyte)(novaLinha), (sbyte)(novaColuna))))
                            {
                                items.Remove(((sbyte)(novaLinha), (sbyte)(novaColuna)));
                                itensColetados++;
                                ExibirMensagem("Você coletou um item!");
                                pontuacao += dificuldade;
                                Console.Beep(5000, 3);
                                if (itensColetados >= dificuldade) { break; }
                            }
                            else if (armadilhas.Contains(((sbyte)(novaLinha), (sbyte)(novaColuna))))
                            {
                                armadilhas.Remove(((sbyte)(novaLinha), (sbyte)(novaColuna)));
                                vidas--;
                                ExibirMensagem("Você caiu em uma armadilha! Perdeu 1 vida.");
                                Console.Beep(1000, 3);
                                if (vidas <= 0)
                                {
                                    ExibirMensagem("Você perdeu todas as suas vidas! Game Over.");
                                    jogoAtivo = false;
                                    break;
                                }
                            }
                            else if (curas.Contains(((sbyte)(novaLinha), (sbyte)(novaColuna))))
                            {
                                curas.Remove(((sbyte)(novaLinha), (sbyte)(novaColuna)));
                                vidas++;
                                ExibirMensagem("Você achou uma cura e recuperou 1 de vida");
                                Console.Beep(5500, 3);
                            }

                            // Atualiza a posição do jogador no labirinto
                            labirinto[jogadorLinha, jogadorColuna] = JOGADOR;
                            AtualizarPosicao(jogadorLinha, jogadorColuna, JOGADOR);
                            ExibirInformacoes(vidas, itensColetados, (sbyte)(dificuldade / 24), pontuacao);
                        }
                    }
                }
            }
            Console.SetCursorPosition(0, ALTURA + 5);
            Console.WriteLine("Pressione qualquer tecla para sair...");
            Console.ReadKey();
        }

        static void InicializarMenu(char[,] menu)
        {
            for (int i = 1; i < menu.GetLength(0); i++)
            {
                for (int j = 0; j < menu.GetLength(1); j++)
                {
                    if (i == 1 || i == menu.GetLength(0) - 1 || j == 0 || j == menu.GetLength(1) - 1)
                    {
                        menu[i, j] = PAREDE; // Bordas do labirinto
                    }
                    else
                    {
                        menu[i, j] = CAMINHO; // Espaço vazio
                    }
                }
            }
            menu[0, 3] = '1';
            menu[0, 6] = '2';
            menu[0, 9] = '3';
        }

        // Método para inicializar o labirinto
        static void InicializarLabirinto(char[,] labirinto, sbyte dificuldade, ref List<(sbyte, sbyte)> items, ref List<(sbyte, sbyte)> armadilhas, ref List<(sbyte, sbyte)> curas)
        {
            Random rnd = new Random();

            // Preenche o labirinto com caminhos e paredes
            for (int i = 0; i < ALTURA; i++)
            {
                for (int j = 0; j < LARGURA; j++)
                {
                    if (i == 0 || i == ALTURA - 1 || j == 0 || j == LARGURA - 1)
                    {
                        labirinto[i, j] = PAREDE; // Bordas do labirinto
                    }
                    else
                    {
                        labirinto[i, j] = CAMINHO; // Espaço vazio
                    }
                }
            }

            // Adiciona paredes internas aleatoriamente
            for (int i = 0; i < (ALTURA * LARGURA) / 4; i++)
            {
                int linha = rnd.Next(1, ALTURA - 1);
                int coluna = rnd.Next(1, LARGURA - 1);
                labirinto[linha, coluna] = PAREDE;
            }

            // Coloca itens no labirinto
            for (int i = 0; i < dificuldade; i++)
            {
                int linha = rnd.Next(1, ALTURA - 1);
                int coluna = rnd.Next(1, LARGURA - 1);
                if (labirinto[linha, coluna] == CAMINHO)
                {
                    items.Add(((sbyte)(linha), (sbyte)(coluna)));
                    labirinto[linha, coluna] = ITEM;
                }
                else { i--; }
            }

            // Coloca armadilhas no labirinto
            for (int i = 0; i < dificuldade; i++)
            {
                int linha = rnd.Next(1, ALTURA - 1);
                int coluna = rnd.Next(1, LARGURA - 1);
                if (labirinto[linha, coluna] == CAMINHO)
                {
                    armadilhas.Add(((sbyte)(linha), (sbyte)(coluna)));
                    labirinto[linha, coluna] = ITEM;
                }
                else { i--; }
            }

            // Coloca curas no labirinto
            for (int i = 0; i < dificuldade / 2; i++)
            {
                int linha = rnd.Next(1, ALTURA - 1);
                int coluna = rnd.Next(1, LARGURA - 1);
                if (labirinto[linha, coluna] == CAMINHO)
                {
                    curas.Add(((sbyte)(linha), (sbyte)(coluna)));
                    labirinto[linha, coluna] = ITEM;
                }
                else { i--; }
            }
        }

        // Método para desenhar o labirinto no console
        static void DesenharLabirinto(char[,] labirinto)
        {
            for (int i = 0; i < labirinto.GetLength(0); i++)
            {
                for (int j = 0; j < labirinto.GetLength(1); j++)
                {
                    DesenharElemento(i, j, labirinto[i, j]);
                }
                Console.WriteLine();
            }
            Console.ResetColor();
        }

        // Método para desenhar um elemento específico na posição dada
        static void DesenharElemento(int linha, int coluna, char elemento)
        {
            Console.SetCursorPosition(coluna, linha);
            switch (elemento)
            {
                case PAREDE:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write(PAREDE);
                    break;
                case CAMINHO:
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(CAMINHO);
                    break;
                case JOGADOR:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(JOGADOR);
                    break;
                case ITEM:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(ITEM);
                    break;
                default:
                    Console.Write(elemento);
                    break;
            }
            Console.ResetColor();
        }

        // Método para atualizar a posição de um elemento no labirinto
        static void AtualizarPosicao(int linha, int coluna, char elemento)
        {
            Console.SetCursorPosition(coluna, linha);
            switch (elemento)
            {
                case CAMINHO:
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(CAMINHO);
                    break;
                case JOGADOR:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(JOGADOR);
                    break;
                default:
                    DesenharElemento(linha, coluna, elemento);
                    break;
            }
            Console.ResetColor();
        }

        // Método para exibir informações ao jogador
        static void ExibirInformacoes(int vidas, int itensColetados, sbyte dificuldade, int pontuacao)
        {
            Console.SetCursorPosition(0, ALTURA + 1);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, ALTURA + 1);
            Console.WriteLine($"Vidas: {vidas} | Itens coletados: {itensColetados} | Dificuldade {dificuldade} | Pontuação: {pontuacao}  (Pressione 'R' para reiniciar o labirinto)");
        }

        // Método para exibir mensagens ao jogador
        static void ExibirMensagem(string mensagem)
        {
            Console.SetCursorPosition(0, ALTURA + 2);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, ALTURA + 2);
            Console.WriteLine(mensagem);
        }
    }
}
