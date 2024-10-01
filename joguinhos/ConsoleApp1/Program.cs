using System;
using System.ComponentModel;
using System.Timers;

class Quiz
{
    private static sbyte temporizador = 10;

    static void Main()
    {
        //Inicialização de variaveis
        Random rng = new();
        string input;
        sbyte jogoSelecionado = 0; //1 - 9

        //loop principal do programa
        while (true)
        {

            //loop do menu
            while (jogoSelecionado == 0)
            {
                Console.ResetColor();
                Console.CursorVisible = true;
                Console.Clear();
                Console.WriteLine("Bem vindo à o jogo dos joguinhos!");
                Console.WriteLine("Alt + F4 para sair.");
                Console.WriteLine("\nSelecione o jogo que deseja jogar:");
                Console.WriteLine("\n1 - Jogo de memorização\n2 - Jogo de adivinhação de números\n3 - Jogo de pedra, papel e tesoura\n" +
                    "4 - Jogo de Forca\n5 - Jogo de númeor pares e ímpares\n6 - Simulador de lançamento de dados\n7 - Jogo de adivinhação de animais\n" +
                    "8 - Jogo de adivinhação de senhas\n9 - Casino");
                input = (Console.ReadLine() ?? string.Empty).Trim();

                //checando resposta
                if (input.Length == 1)
                {
                    try
                    {
                        jogoSelecionado = Convert.ToSByte(input);
                    }
                    catch (Exception)
                    {
                        jogoSelecionado = 0;
                        Console.WriteLine("Resposta Inválida");
                        Thread.Sleep(800);
                        Console.Clear();
                    }
                }
                else
                {
                    jogoSelecionado = 0;
                    Console.WriteLine("Resposta Inválida");
                    Thread.Sleep(800);
                    Console.Clear();
                }
                while (Console.KeyAvailable) { Console.ReadKey(); }
            }

            switch (jogoSelecionado)
            {
                case 1:
                    jogoSelecionado = JogoMemorizacao(rng);
                    break;
                case 2:
                    jogoSelecionado = JogoAdivinhacaoNumero(rng);
                    break;
                case 3:
                    jogoSelecionado = JogoPedraPapelTesoura(rng);
                    break;
                case 4:
                    jogoSelecionado = JogoForca(rng);
                    break;
                case 5:
                    jogoSelecionado = JogoParImpar(rng);
                    break;
                case 6:
                    jogoSelecionado = SimuladorDados(rng);
                    break;
                case 7:
                    jogoSelecionado = JogoAdivinhacaoAnimal(rng);
                    break;
                case 8:
                    jogoSelecionado = JogoAdivinhacaoSenha(rng);
                    break;
                case 9:
                    jogoSelecionado = Casino(rng);
                    break;
                default:
                    jogoSelecionado = 0;
                    break;
            }            

        }
    }
    private static void TimerEvent(Object source, ElapsedEventArgs e)
    {
        temporizador--;
        Console.Write("Tempo: " + temporizador + "        ");
    }
    static bool Introducao(string nomeDoJogo, string breveTutorial, string placar = "")
    {
        Console.Clear();
        Console.WriteLine("Bem-vindo ao " + nomeDoJogo);
        Console.WriteLine(breveTutorial);
        if (placar != "") { Console.WriteLine(placar); }
        Console.WriteLine("escreva 'menu' para para voltar ao menu");
        Console.WriteLine("\nPressione 'Enter' para continuar...");
        if ((Console.ReadLine() ?? string.Empty).ToLower().Trim() == "menu") { return true; }
        Console.Clear();
        return false;
    }

    static sbyte JogoMemorizacao(Random rng)
    {
        int pontuacaoMax = 5;
        string pontuacaoMaxNome = "Melbi";
        int tempoMemorisacao = 3;
        string ultimoNumero = "";
        string dificuldade;
        string sequencia;

        while (true)
        {
            //Tela Incial do jogo
            sbyte vidas = 2;
            int quantidadeCaracteres = 1;
            int pontuacao = 0;
            if (Introducao
                ("Jogo de Memorizacao de Numeros!",
                "Neste jogo você deve memorizar a sequencia de números que aparecerão na tela por um curto período de tempo!",
                "Pontuação Máxima atual: " + pontuacaoMax + " - Jogador: " + pontuacaoMaxNome
                )) { return 0; }

            //Selecionar a dificuldade
            Console.WriteLine("Selecione a difculdade:");
            Console.WriteLine("1 - Normal");
            Console.WriteLine("2 - Dificíl (a sequencia jamais irá conter dois número iguais lado à lado)");
            dificuldade = (Console.ReadLine() ?? string.Empty).Trim();

            if (dificuldade == "1")
            {
                dificuldade = "1";
            }
            else if (dificuldade == "2")
            {
                dificuldade = "2";
            }
            else
            {
                Console.WriteLine("Resposta Inválida. Dificuldade será 'Normal'");
                dificuldade = "1";
                Thread.Sleep(2000);
            }
            while (Console.KeyAvailable) { Console.ReadKey(); }
            Console.Clear();


            //Loop do jogo
            while (vidas > 0)
            {
                //Cria e Atualiza a sequência
                sequencia = "";
                if (dificuldade == "2")
                {
                    for (int i = 0; i < quantidadeCaracteres; i++)
                    {
                        string novoNumero = rng.Next(10).ToString();
                        while (novoNumero == ultimoNumero) { novoNumero = rng.Next(10).ToString(); }
                        sequencia += novoNumero;
                        ultimoNumero = novoNumero;
                    }
                }
                else
                {
                    for (int i = 0; i < quantidadeCaracteres; i++)
                    {
                        sequencia += rng.Next(10).ToString();
                    }
                }

                //Escreve a sequência na tela
                Console.WriteLine("Memorize essa sequência.");
                Console.WriteLine(" " + sequencia);

                //Timer para memorizar a sequencia
                Thread.Sleep(200);
                for (int i = tempoMemorisacao; i > 0; i--)
                {
                    Console.Write(i);
                    Thread.Sleep(300);
                    Console.Write(".");
                    Thread.Sleep(300);
                    Console.Write(".");
                    Thread.Sleep(300);
                    Console.Write(".");
                }
                Thread.Sleep(300);
                Console.Clear();
                while (Console.KeyAvailable) { Console.ReadKey(); }
                Console.Clear();

                //processamento da resposta do jogador
                Console.Write("Digite a sequência: ");

                if ((Console.ReadLine() ?? string.Empty).Trim() == sequencia)
                {
                    pontuacao += sequencia.Length * Convert.ToInt32(dificuldade);
                    quantidadeCaracteres++;
                    Console.Clear();
                    Console.WriteLine("Parabéns, Você Acertou!");
                    Console.WriteLine("Pontuação Atual: " + pontuacao);
                    Console.WriteLine("\nPressione 'Enter' para continuar...");
                    Console.ReadLine();
                    Console.Clear();
                }
                else
                {
                    vidas--;
                    if (vidas <= 0) { break; }
                    Console.WriteLine("Você Errou!");
                    Console.WriteLine("A sequência correta era: " + sequencia);
                    Console.WriteLine("Vidas restantes: " + vidas);
                    Console.WriteLine("\nPressione 'Enter' para continuar...");
                    Console.ReadLine();
                    Console.Clear();
                }

            }
            Console.Clear();
            Console.WriteLine("Fim de Jogo!");

            //Atualiza Pontuação máxima
            if (pontuacao > pontuacaoMax)
            {
                pontuacaoMax = pontuacao;
                Console.WriteLine("Parabéns Você Quebrou o Recorde de " + pontuacaoMaxNome + "!");
                Console.Write("\nDigite Seu Nome: ");
                pontuacaoMaxNome = Console.ReadLine() ?? string.Empty;
                if (pontuacaoMaxNome == null) { pontuacaoMaxNome = "Melbi"; }
            }
            else
            {
                Console.WriteLine("Pontuação Final: " + pontuacao);
                Console.WriteLine("\nPressione 'Enter' para continuar...");
                Console.ReadLine();
            }
        }
    }

    static sbyte JogoAdivinhacaoNumero(Random rng)
    {
        sbyte vidas;
        string input;

        while (true)
        {
            vidas = 7;
            string resposta = Convert.ToString(rng.Next(0, 101));

            if (Introducao
                ("Jogo de Adivinhacao de Numero!",
                "Nesse jogo você terá 7 tentativas para adivinhar o número escolhido"
                )) { return 0; }

            Console.WriteLine("Um número de 1 à 100 foi escolhido, tente adivinhá-lo.");

            //loop principal do jogo
            while (vidas > 0)
            {
                input = (Console.ReadLine() ?? string.Empty).Trim();

                if (input == resposta) { break; }

                //dicas
                try
                {
                    if (Convert.ToSByte(input) < Convert.ToSByte(resposta))
                    {
                        if (Convert.ToSByte(input) < Convert.ToSByte(resposta) - 10)
                        {
                            Console.WriteLine("Muito baixo!");
                        }
                        else
                        {
                            Console.WriteLine("Um pouco baixo!");
                        }
                    }
                    else
                    {
                        if (Convert.ToSByte(input) > Convert.ToSByte(resposta) + 10)
                        {
                            Console.WriteLine("Muito alto!");
                        }
                        else
                        {
                            Console.WriteLine("Um pouco alto!");
                        }
                    }
                    vidas--;
                }
                catch (Exception)
                {
                    Console.WriteLine("Tem algo de errado com sua resposta, mas não se preocupe, você não perderá uma tentativa desta vez!");
                }
                Console.WriteLine("Tentativas restantes: " + vidas);
            }

            //Fim de jogo + resultado
            if (vidas > 0)
            {
                Console.WriteLine("\nParabéns, você acertou o número!");
                Console.WriteLine("O número era: " + resposta);
                Console.WriteLine("\nPressione 'enter' para continuar...");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("\nAcabaram suas tentativas!");
                Console.WriteLine("O número era: " + resposta);
                Console.WriteLine("\nPressione 'enter' para continuar...");
                Console.ReadLine();
            }

        }
    }

    static sbyte JogoPedraPapelTesoura(Random rng)
    {
        string input;
        string respostaMaquina;
        string[] opcoes = ["pedra", "papel", "tesoura"];
        sbyte[] placar = [0, 0];

        //loop do jogo
        while (true)
        {
            placar[0] = 0;
            placar[1] = 0;

            if (Introducao
                ("Jogo de Pedra Papel e Tesoura!",
                "Nesse jogo você deve escrever 'PEDRA' 'PAPEL' ou 'TESOURA'",
                "Cada partida é decidida em u melhor de três"
                )) { return 0; }

            //loop das partidas
            while (true)
            {
                //escolha da máquina e a do jogador
                respostaMaquina = opcoes[rng.Next(0, 3)];
                Console.Clear();
                Console.WriteLine("Placar:\nMáquina: " + placar[0] + " | " + "Player: " + placar[1]);
                Console.WriteLine("\nDigite sua escoha(Pedra / Papel / Tesoura).");
                input = (Console.ReadLine() ?? string.Empty).ToLower().Trim();

                //escrevendo resultado
                Console.Clear();
                Console.WriteLine("Pedra...Papel...Tesoura!");
                Console.WriteLine("\nSua escolha: " + input);
                Console.WriteLine("Escolha da máquina: " + respostaMaquina + "\n");

                //decidindo quem ganhou
                if (input == respostaMaquina) //em caso de empate
                {
                    Console.WriteLine("Empate!");
                }
                else
                {
                    switch (input)
                    {
                        case "pedra":
                            if (respostaMaquina == "papel")
                            {
                                Console.WriteLine("Máquina Ganhou!");
                                placar[0]++;

                            }
                            else if (respostaMaquina == "tesoura")
                            {
                                Console.WriteLine("Você Ganhou!");
                                placar[1]++;
                            }
                            break;
                        case "papel":
                            if (respostaMaquina == "tesoura")
                            {
                                Console.WriteLine("Máquina Ganhou!");
                                placar[0]++;
                            }
                            else if (respostaMaquina == "pedra")
                            {
                                Console.WriteLine("Você Ganhou!");
                                placar[1]++;
                            }
                            break;
                        case "tesoura":
                            if (respostaMaquina == "pedra")
                            {
                                Console.WriteLine("Máquina Ganhou!");
                                placar[0]++;
                            }
                            else if (respostaMaquina == "papel")
                            {
                                Console.WriteLine("Você Ganhou!");
                                placar[1]++;
                            }
                            break;
                        default:
                            Console.WriteLine("Máquina Ganhou!");
                            placar[0]++;
                            break;
                    }
                }
                Console.WriteLine("\nPressione 'Enter' para continuar...");
                Console.ReadLine();

                //fim de jogo / jogar denovo
                if (placar[0] >= 2)
                {
                    Console.Clear();
                    Console.WriteLine("Fim de jogo");
                    Console.WriteLine("A máquina ganhou!");
                    Console.Write("Deseja jogar de novo?(S|N): ");
                    if ((Console.ReadLine() ?? string.Empty).ToLower().Trim() == "s") { placar = [0, 0]; } else { break; }
                }
                else if (placar[1] >= 2)
                {
                    Console.Clear();
                    Console.WriteLine("Fim de jogo");
                    Console.WriteLine("Você ganhou!");
                    Console.Write("Deseja jogar de novo?(S|N): ");
                    if ((Console.ReadLine() ?? string.Empty).ToLower().Trim() == "s") { placar = [0, 0]; } else { break; }
                }

            }

        }
    }

    static sbyte JogoForca(Random rng)
    {
        List<string> palavras = [];
        List<char> letrasUsadas = [];
        List<char> palavraForca = [];
        char input;
        bool letraExiste;

        //loop principal
        while (true)
        {
            char[] boneco = ['/', '\\', '/', '\\', 'O'];
            //lendo arquivo com as palavras
            try
            {
                using (StreamReader file = new StreamReader("palavras.txt"))
                {
                    palavras.Clear();
                    while (!file.EndOfStream)
                    {
                        string palavra = file.ReadLine() ?? string.Empty;
                        if (palavra != string.Empty) { palavras.Add(palavra); }
                    }
                    if (palavras.Count == 0) { palavras.Add("palavra"); }
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("O arquivo contendo as palavras não pode ser encontrado.");
                Console.WriteLine("Precione 'Enter' para voltar ao menu...");
                Console.ReadLine();
                return 0;
            }

            //resetando variaves e escolhendo a palavra secreta
            char[] palavraSecreta = palavras[rng.Next(0, palavras.Count)].ToCharArray();
            letrasUsadas.Clear();
            palavraForca.Clear();

            for (int i = 0; i < palavraSecreta.Length; i++) { palavraForca.Add('_'); }

            if (Introducao
                ("Jogo de Forca!",
                "Nesse jogo você terá um total de 5 tentativas para acertar uma palavra aleatória."
                )) { return 0; }

            //loop do jogo
            for (int i = 0; i < 6; i++)
            {
                //desenhando o boneco
                Console.Clear();
                Console.WriteLine(" " + boneco[4]);
                Console.WriteLine(boneco[2] + "|" + boneco[3]);
                Console.WriteLine(boneco[0] + " " + boneco[1] + "\n");

                //mostrando a palavra e as letras usadas
                Console.Write(string.Join(' ', palavraForca));

                Console.Write("\nLetras Usadas: ");
                Console.Write(string.Join(' ', letrasUsadas));

                //estou checando se o jogador ganhou antes da resposta para que quando o jogo acabar o jogador poder ver o boneco e as letras usadas
                if (!palavraForca.Contains('_'))
                {
                    Console.WriteLine("\nParabéns, Você Ganhou!");
                    Console.WriteLine("\nPressione 'enter' para continuar...");
                    Console.ReadLine();
                    i = 6;
                }
                else if (boneco[4] == ' ')
                {
                    Console.WriteLine("\n Você Perdeu!");
                    Console.WriteLine("A palavra era: " + string.Concat(palavraSecreta));
                    Console.WriteLine("\nPressione 'enter' para continuar...");
                    Console.ReadLine();
                    i = 6;
                }
                else
                {
                    //obtendo resposta do jogador
                    Console.Write("\nSua escolha: ");
                    string line = Console.ReadLine().Trim().ToLower() ?? string.Empty;
                    input = line != string.Empty ? line.First() : ' ';

                    if (letrasUsadas.Contains(input))
                    {
                        Console.WriteLine("Letra já usada!");
                    }
                    else
                    {
                        if (input == ' ') { Console.WriteLine(i); boneco[i] = ' '; continue; }

                        letrasUsadas.Add(input);
                        letraExiste = false;
                        for (int j = 0; j < palavraSecreta.Count(); j++)
                        {
                            if (palavraSecreta[j] == input)
                            {
                                palavraForca[j] = input;
                                letraExiste = true;
                            }
                        }
                        if (letraExiste) { i--; } else { Console.WriteLine(i); boneco[i] = ' '; } //apagando o boneco
                        
                    }
                }

            }

            Console.Clear();
            Console.WriteLine("Deseja adicionar uma palavra para o jogo de forca?(S/N)");
            string readLine = Console.ReadLine().Trim().ToLower() ?? string.Empty;
            if (readLine != string.Empty && readLine.First() == 's')
            {
                Console.Clear();
                Console.WriteLine("Digite sua palavra: ");
                string palavraNova = (Console.ReadLine() ?? string.Empty).Trim().ToLower();
                using (StreamWriter file = new StreamWriter("palavras.txt", true))
                {
                    while (palavras.Contains(palavraNova))
                    {
                        Console.Clear();
                        Console.WriteLine("Sua palavra já existe, Digite uma nova: ");
                        palavraNova = (Console.ReadLine() ?? string.Empty).Trim().ToLower();
                    }
                    file.WriteLine(palavraNova);
                }
            }
            readLine = string.Empty;
        }
    }

    static sbyte JogoParImpar(Random rng)
    {
        sbyte[] placar = new sbyte[2];

        //loop principal
        while (true)
        {
            placar = [0, 0];
            bool fimDeJogo = false;
            temporizador = 10;

            if (Introducao
                ("Jogo de Par e Ímpar",
                "Nesse jogo você deve adivinhar se o número escolhido é par ou ímpar",
                "Pressiona 'seta esquerda' para par e 'seta direita' para ímpar. 'seta para baixo' termina o jogo."
                )) { return 0; }

            //Configurando o temporizador
            System.Timers.Timer timer = new System.Timers.Timer(1000);
            timer.AutoReset = true;
            timer.Enabled = true;
            timer.Elapsed += TimerEvent;
            timer.Start();

            //loop do jogo
            while (!fimDeJogo)
            {
                int numeroEscolhido = rng.Next(1, 10000);
                Console.Clear();
                Console.WriteLine("Acertos: " + placar[0] + " | Erros: " + placar[1]);
                Console.WriteLine("\nNúmero: " + numeroEscolhido);
                Console.WriteLine("\n Par   <    >   Ímpar\n         \\/\n        Sair");
                Console.Write("Tempo: " + temporizador);
                while (true)
                {
                    Console.SetCursorPosition(0, 7);

                    //checando a tecla
                    if (Console.KeyAvailable && temporizador > 0)
                    {
                        ConsoleKeyInfo tecla = Console.ReadKey(true);
                        switch (tecla.Key)
                        {
                            case ConsoleKey.DownArrow:
                                fimDeJogo = true;
                                goto exitLoop;
                            case ConsoleKey.LeftArrow:
                                if (numeroEscolhido % 2 == 0)
                                {
                                    placar[0]++;
                                }
                                else
                                {
                                    placar[1]++;
                                }
                                goto exitLoop;
                            case ConsoleKey.RightArrow:
                                if (numeroEscolhido % 2 == 1)
                                {
                                    placar[0]++;
                                }
                                else
                                {
                                    placar[1]++;
                                }
                                goto exitLoop;
                            default:
                                break;
                        }
                    }
                    if (temporizador <= 0) { fimDeJogo = true; break; }
                }

            exitLoop:;
            }
            timer.Dispose();
            Console.Clear();
            Console.WriteLine("Fim de jogo!");
            Console.WriteLine($"Placar Final:\nAcertos: {placar[0]} | Erros: {placar[1]}");
            Console.WriteLine("\nPressione 'Enter' para continuar...");
            Console.ReadLine();

        }
    }

    static sbyte SimuladorDados(Random rng)
    {
        sbyte[] dados = [4, 6, 8, 10, 12, 14, 16, 20, 100];
        sbyte dadoSelecionado = 0;
        sbyte numeroDado;
        sbyte input;

        //loop principal
        while (true)
        {
            if (Introducao
                ("Simulador de Dados!",
                "Nesse jogo você deve tentar adivinhar o número que será lançad pelo dado."
                )) { return 0; }

            while (true)
            {
                Console.WriteLine("Selecione o seu dado.");
                Console.WriteLine("1 - d4\n2 - d6\n3 - d8\n4 - d10\n5 - d12\n6 - d14\n7 - d16\n8 - d20\n9 - d100");
                try
                {
                    dadoSelecionado = (sbyte)(Console.ReadLine().Trim().First() - '0');
                    dadoSelecionado--;
                    if (dadoSelecionado >= 0 && dadoSelecionado < 10) { break; }
                    Console.Clear();
                }
                catch (Exception) { Console.Clear(); }
            }
            while (true)
            {
                numeroDado = ((sbyte)rng.Next(1, dados[dadoSelecionado]));
                Console.Clear();
                Console.WriteLine("Qual número você acha que caiu no dado d" + dados[dadoSelecionado] + "?");

                //resposta do jogador
                while (true)
                {
                    try
                    {
                        input = Convert.ToSByte(Console.ReadLine().Trim());
                        if (input == numeroDado)
                        {
                            Console.WriteLine("\nParabéns! Você acertou!");
                        }
                        else
                        {
                            Console.WriteLine("\nQue pena! Você errou!");
                        }
                        Console.WriteLine("O número do dado era " + numeroDado);
                        Console.WriteLine("\nPressione 'Enter' para continuar...");
                        Console.ReadLine();
                        break;
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Número Inválido, tente novamente.");
                    }
                }

                Console.Clear();
                Console.WriteLine("Deseja jogar novamente?(S/N)");
                if (Console.ReadLine().Trim().ToLower() != "s") { break; }
            }
        }
    }

    static sbyte JogoAdivinhacaoAnimal(Random rng)
    {
        string[] animal = new string[5];
        List<string> animais = [];

        //loop principal
        while (true)
        {
            sbyte pontuacao = 0;
            sbyte pontos = 10;
            bool fimDeJogo = false;

            //lendo o arquivo
            animais.Clear();
            using (StreamReader file = new StreamReader("animais.txt"))
            {
                while (!file.EndOfStream)
                {
                    string line = (file.ReadLine() ?? string.Empty);
                    if (line != string.Empty) { animais.Add(line); }
                }
            }

            if (Introducao
                ("Jogo de Adivinhação de Animal!",
                "Nesse jogo você terá 3 tentativas para adivinhar qual é cada animal.",
                "Caso você erre, dicas irão aparecer, após o 3º erro você perde."
                )) { return 0; }

            //loop do jogo
            while (!fimDeJogo)
            {
                animal = animais[rng.Next(animais.Count)].Split(new[] { ", " }, StringSplitOptions.None);
                pontos = 10;
                Console.WriteLine("Pontuação: " + pontuacao);
                for (int i = 1; i <= 4; i++)
                {                                             //esse espaço é para apagar o texto 'Qual o Animal?' caso a dica seja muito pequena
                    Console.WriteLine("Dica#" + i + ": " + animal[i] + "                  ");
                    Console.WriteLine("Qual o Animal?");

                    //Checando se o jogador acertou
                    if (Console.ReadLine().Trim().ToLower() == animal[0].ToLower())
                    {
                        Console.Clear();
                        Console.WriteLine("Parabéns, Voce Acertou!");
                        Console.WriteLine("O animal era: " + animal[0]);
                        Console.WriteLine("Gostaria de continuar jogando?(S/N):");
                        string line = Console.ReadLine().Trim().ToLower() ?? string.Empty;
                        if (line != string.Empty && line.First() != 's') { fimDeJogo = true; }
                        break;
                    }

                    //apagar a linha em que o jogador escreveu
                    Console.SetCursorPosition(0, i + 2);
                    Console.Write(new string(' ', Console.WindowWidth));
                    Console.SetCursorPosition(0, i + 1);
                    pontos -= (sbyte)i;
                }
                //caso o jogador perca
                if (pontos <= 0) { fimDeJogo = true; } else { pontuacao += pontos; }
            }
            //Fim do jogo
            Console.Clear();
            Console.WriteLine("Fim de Jogo!");
            Console.WriteLine("Pontuação Final: " + pontuacao);
            Console.WriteLine("Gostaria de adicionar um animal?(S/N):");

            //Adicionando novo animal
            string input = Console.ReadLine().Trim().ToLower() ?? string.Empty;
            if (input != string.Empty && input.First() == 's')
            {
                Console.Clear();
                Console.WriteLine("Escreva primeiro o animal, e então as dicas 1 - 4. Separe cada elemento usando ', '");
                Console.WriteLine("Exemplo: 'Baleia, mamífero, grande, azul, aquatico'");

                using (StreamWriter file = new StreamWriter("animais.txt", true))
                {
                    string novoAnimal = (Console.ReadLine() ?? string.Empty);
                    string[] virgulas = novoAnimal.Split(new[] { ", " }, StringSplitOptions.None);
                    if (novoAnimal != string.Empty && virgulas.Length == 4)
                    {
                        Console.WriteLine(novoAnimal);
                    }
                }
            }

        }
    }

    static sbyte JogoAdivinhacaoSenha(Random rng)
    {
        List<char> caracteresDaSenha = [];
        List<char> resposta = [];
        string readLine;

        //loop principal
        while (true)
        {
            sbyte dificuldade = -1;
            if (Introducao
                ("Jogo de Adivinhação de Senha!",
                "Nesse jogo você deve adivinhar a senha. Você terá 10 tentativas",
                "Toda vez que você tentar adivihar, o jogo dirá quantos digitos estão corretos."
                )) { return 0; }

            // Seletor de dificuldade
            Console.WriteLine("Selecione a dificuldade.");
            Console.WriteLine("  1  --  2  --  3  --  4");
            Console.Write("  ^");
            sbyte arrowPosition = 3;            
            while (dificuldade < 0)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    switch (key.Key)
                    {
                        case ConsoleKey.LeftArrow:
                            if (arrowPosition > 3)
                            {
                                arrowPosition -= 7;
                                Console.SetCursorPosition(0, 2);
                                Console.Write(new string(' ', Console.WindowWidth));
                                Console.SetCursorPosition(arrowPosition - 3, 2);
                                Console.Write("  ^");
                                break;
                            }
                            break;
                        case ConsoleKey.RightArrow:
                            if (arrowPosition < 24)
                            {
                                arrowPosition += 7;
                                Console.SetCursorPosition(0, 2);
                                Console.Write(new string(' ', Console.WindowWidth));
                                Console.SetCursorPosition(arrowPosition - 3, 2);
                                Console.Write("  ^");
                                break;
                            }
                            break;
                        case ConsoleKey.Enter:
                            dificuldade = (sbyte)(arrowPosition - 3);
                            break;
                        default:
                            break;
                    }
                }
            }
            switch (dificuldade)
            {
                case 0:
                    dificuldade = 5;
                    break;
                default:
                    dificuldade = (sbyte)(((double)dificuldade) * 1.5);
                    break;
            }

            // Loop do jogo
            while (true)
            {
                sbyte tentativas = 10;
                caracteresDaSenha.Clear();
                resposta.Clear();
                resposta.AddRange(new string('_', dificuldade));

                for (int i = 0; i < dificuldade; i++)
                {
                    switch (Math.Round(rng.NextSingle()))
                    {
                        case 0:
                            caracteresDaSenha.Add((char)rng.Next(48, 58));
                            break;
                        case 1:
                            caracteresDaSenha.Add((char)rng.Next(65, 91));
                            break;
                    }
                }

                Console.Clear();
                for (sbyte i = 0; i < tentativas; i++)
                {
                    Console.WriteLine(string.Join(' ', resposta));
                }
                Console.WriteLine("\n\nQual a senha?");

                //loop da senha
                while (tentativas > 0)
                {

                    List<char> caracteresDaSenhaCopia = new List<char>(caracteresDaSenha);

                    Console.SetCursorPosition(0, 11);
                    Console.Write("Tentativas restantes: " + tentativas + " ");
                    Console.SetCursorPosition(13, 12);
                    Console.Write(new string(' ', Console.WindowWidth));
                    Console.SetCursorPosition(0, 13);
                    Console.Write(new string(' ', Console.WindowWidth));
                    Console.SetCursorPosition(0, 13);

                    readLine = Console.ReadLine().Trim().ToUpper();

                    if (readLine.Length < caracteresDaSenha.Count)
                    {
                        if (readLine.Length != 0)
                        {
                            resposta = readLine.ToList();
                        }
                    }
                    else
                    {
                        resposta = readLine.Substring(0, caracteresDaSenha.Count).ToList();
                    }

                    while(resposta.Count < caracteresDaSenha.Count)
                    {
                        resposta.Add('_');
                    }

                    //escrever a resposta
                    Console.SetCursorPosition(0, Math.Abs(tentativas - 10));
                    for (int i = 0; i < dificuldade;i++)
                    {
                        if (caracteresDaSenhaCopia[i] == resposta[i])
                        {
                            Console.BackgroundColor = ConsoleColor.Green;
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            caracteresDaSenhaCopia[i] = ' ';
                        }
                        else if (caracteresDaSenhaCopia.Contains(resposta[i]))
                        {
                            int index = 0;
                            while (true)
                            {
                                index = caracteresDaSenhaCopia.FindIndex(index, c => c == resposta[i]);

                                if (index != -1)
                                {
                                    if (resposta[index] != caracteresDaSenhaCopia[index])
                                    {
                                        Console.BackgroundColor = ConsoleColor.Yellow;
                                        Console.ForegroundColor = ConsoleColor.DarkGray;
                                        break;
                                    }
                                    else
                                    {
                                        index++;
                                    }
                                }
                                else
                                {
                                    Console.ResetColor();
                                    break;
                                }
                            }
                        }
                        else
                        {
                            Console.ResetColor();
                        }

                        Console.Write(resposta[i] + " ");
                        Console.ResetColor();
                    }

                    if (!resposta.SequenceEqual(caracteresDaSenha))
                    {
                        tentativas--;
                    }
                    else
                    {
                        break;
                    }

                }

                Console.SetCursorPosition(0, 11);
                Console.Write("Fim de Jogo!" + new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, 12);
                if (tentativas == 0)
                {
                    Console.WriteLine("Você Perdeu!" + new string(' ', Console.WindowWidth));
                    Console.Write("A senha era: ");
                }
                else
                {
                    Console.WriteLine("Você Ganhou!" + new string(' ', Console.WindowWidth));
                    Console.Write("A senha era: ");
                }

                Console.WriteLine(string.Concat(caracteresDaSenha));

                {
                    Console.WriteLine("\nDeseja jogar de novo?(S/N)");
                    string input = Console.ReadLine().Trim().ToLower() ?? string.Empty;
                    if (input != string.Empty || input.First() != 's')
                    {
                        break;
                    }
                }
            }
        }
    }

    static sbyte Casino(Random rng)
    {
        string recorde = string.Empty;
        Dictionary<byte, short> repeticao = new Dictionary<byte, short>();

        // loop principal
        while (true)
        {
            try
            {
                using (StreamReader file = new StreamReader("casino.txt"))
                {
                    recorde = (file.ReadLine() ?? new string("Melbi - R$2"));
                }
            }
            catch (Exception)
            {
                Console.Clear();
                Console.WriteLine("Erro ao carregar o arquivo 'casino.txt'. Pressione enter para voltar ao menu.");
                Console.ReadLine();
                return 0;
            }

            short dinheiro = 1000;
            bool jogoAtivo = true;
            List<(string[] simbolo, ConsoleColor cor, float valor)> simbolos = [];
            simbolos.Add(([" _ ", "| |", "|_|"], ConsoleColor.White, 1));
            simbolos.Add(([" _ ", "_|_", " | "], ConsoleColor.Blue, 1.5f));
            simbolos.Add((["\\ /", " O ", "/ \\"], ConsoleColor.Yellow, 2));
            simbolos.Add(([" O ", "/|\\", "/ \\"], ConsoleColor.Green, 3));
            simbolos.Add((["___", "  /", " / "], ConsoleColor.Red, 10));
            simbolos.Add(([" 0 ", "0X0", " 0 "], ConsoleColor.Magenta, 1.5f));

            byte[] slots = [0,0,0];

            if (Introducao
                ("Bem-Vindo ao Casino!",
                "Aqui você irá começar com R$1000,00 e deve tentar sair do casino com a maior quantidade de dinheiro.",
                "Recorde do Casino: " + recorde
                )) { return 0; }

            DesenharCacaNiquel();
            Console.WriteLine("Dinheiro: R$" + dinheiro);
            Console.WriteLine("Pressione 'enter' para jogar ou 'backspace' para sair do casino com seu dinheiro.");
            Console.CursorVisible = false;

            // loop do casino
            while (jogoAtivo && dinheiro > 0)
            {

                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo tecla = Console.ReadKey(true);

                    switch (tecla.Key)
                    {
                        case ConsoleKey.Enter:
                            dinheiro -= 100;
                            Console.SetCursorPosition(0, 11);
                            Console.Write("Dinheiro: R$" + dinheiro + new string(' ', Console.WindowWidth / 2));

                            AtivarCacaNiquel(simbolos, ref slots, rng);

                            Console.SetCursorPosition(0, 11);

                            foreach (var slot in slots)
                            {
                                if (repeticao.ContainsKey(slot))
                                {
                                    repeticao[slot]++;
                                }
                                else
                                {
                                    repeticao[slot] = 1;
                                }
                            }

                            if (repeticao.ContainsValue(2))
                            {
                                foreach (var key in repeticao.Keys)
                                {
                                    if (repeticao[key] == 2)
                                    {
                                        dinheiro += (short)(20 * simbolos[(int)key].valor);
                                        Console.Write("Dinheiro: R$" + dinheiro + $" + R${(short)(20 * simbolos[(int)key].valor)}" + "    ");
                                    }
                                }
                            }
                            else if (repeticao.ContainsValue(3))
                            {
                                dinheiro += (short)(100 * simbolos[slots[0]].valor);
                                Console.Write("Dinheiro: R$" + dinheiro + $" + R${(short)(100 * simbolos[slots[0]].valor)}" + "    ");
                            }
                            else
                            {
                                Console.Write("Dinheiro: R$" + dinheiro + " + R$0       ");
                            }

                            repeticao.Clear();
                            break;
                        case ConsoleKey.Backspace:
                            jogoAtivo = false;
                            break;
                        default:
                            break;
                    }
                }
            }
            while(Console.KeyAvailable) { Console.ReadKey(); }

            Console.SetCursorPosition(0, 15);
            Console.CursorVisible = true;

            if (dinheiro > Convert.ToInt32(recorde.Substring(recorde.IndexOf("$") + 1)))
            {
                Console.WriteLine("Parabéns. Você Bateu o recorde do Casino! Digite seu nome.");
                string input = Console.ReadLine().Trim();
                if (input.Length > 0)
                {
                    try
                    {
                        using (StreamWriter file = new StreamWriter("casino.txt"))
                        {
                            file.WriteLine(input + " - R$" + dinheiro);
                        }
                    }
                    catch (Exception)
                    {
                        Console.Clear();
                        Console.WriteLine("Erro ao carregar o arquivo 'casino.txt'. Pressione enter para voltar ao menu.");
                        Console.ReadLine();
                        return 0;
                    }
                }
            }
            else
            {
                Console.WriteLine("Fim de jogo! Você Faliu!");
                Console.WriteLine("\n Pressione 'Enter' para continuar...");
                Console.ReadLine();
            }

        }
    }

    static void DesenharCacaNiquel()
    {
        //eu desenhei isso num documento txt e depois copiei aqui, se quiser mudar algo boa sorte
        Console.WriteLine("\t ___________________\r\n\t/\t\t    \\  ___\r\n\t" +
            "|    Caça-Níqueis   | |   |\r\n  \t|  ___\t ___   ___  | |___|\r\n\t" +
            "| |   |\t|   | |\t  | |\t|\r\n\t| |   |\t|   | |\t  | |\t|\r\n\t| |   |\t" +
            "|   | |\t  | |==<|\r\n\t| |___|\t|___| |___| |\r\n\t|     \t\t    |\r\n\t" +
            "|  Preço: R$100,00  |\r\n\tZZZZZZZZZZZZZZZZZZZZZ");
    }

    static void AtivarCacaNiquel(List<(string[] simbolo, ConsoleColor cor, float valor)> simbolos, ref byte[] slots, Random rng)
    {
        int[] posCaixas = [11, 17, 23];

        for (int i = 0; i < 3; i++)
        {
            Console.SetCursorPosition(posCaixas[i], 4);
            int giros = rng.Next(10, 25);
            byte index = slots[i];

            for (int j = 0; j < giros; j++)
            {
                Console.ForegroundColor = simbolos[index].cor;
                Console.Write(simbolos[index].simbolo[2]);

                Console.SetCursorPosition(posCaixas[i], 5);

                if (index == 0)
                {
                    Console.ForegroundColor = simbolos[5].cor;
                    Console.SetCursorPosition(posCaixas[i], 5);

                    Console.Write(simbolos[5].simbolo[0]);
                    Console.SetCursorPosition(posCaixas[i], 6);

                    Console.Write(simbolos[5].simbolo[1]);
                }
                else
                {
                    Console.ForegroundColor = simbolos[index - 1].cor;
                    Console.SetCursorPosition(posCaixas[i], 5);

                    Console.Write(simbolos[index - 1].simbolo[0]);
                    Console.SetCursorPosition(posCaixas[i], 6);

                    Console.Write(simbolos[index - 1].simbolo[1]);
                }

                Thread.Sleep(25);
                Console.SetCursorPosition(posCaixas[i], 4);


                Console.ForegroundColor = simbolos[index].cor;
                Console.Write(simbolos[index].simbolo[1]);
                Console.SetCursorPosition(posCaixas[i], 5);

                Console.Write(simbolos[index].simbolo[2]);
                Console.SetCursorPosition(posCaixas[i], 6);

                if (index == 0)
                {
                    Console.ForegroundColor = simbolos[5].cor;
                    Console.Write(simbolos[5].simbolo[0]);
                }
                else
                {
                    Console.ForegroundColor = simbolos[index - 1].cor;
                    Console.Write(simbolos[index - 1].simbolo[0]);
                }

                Thread.Sleep(25);
                Console.SetCursorPosition(posCaixas[i], 4);

                Console.ForegroundColor = simbolos[index].cor;
                Console.Write(simbolos[index].simbolo[0]);
                Console.SetCursorPosition(posCaixas[i], 5);

                Console.Write(simbolos[index].simbolo[1]);
                Console.SetCursorPosition(posCaixas[i], 6);

                Console.Write(simbolos[index].simbolo[2]);

                Thread.Sleep(25);
                Console.SetCursorPosition(posCaixas[i], 4);


                if (index + 1 > 5) { index = 0; } else { index++; }
            }

            slots[i] = index == 0 ? (byte)5 :(byte)(index - (byte)1);
        }

        Console.ResetColor();
    }
}