using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace cadastro_cliente
{
    class Program
    {
        static void Main(string[] args)
        {
            var clientes = new Dictionary<string, Cliente>();
            var logs = new Dictionary<string, DateTime>();

            int opcao = 1;
            while (opcao>0 && opcao<4)
            {
                 Console.WriteLine("\n\n");
                Console.WriteLine("______________________________________________________________________________");
                Console.WriteLine("\n                      CADASTRO DE CLIENTE");
                Console.WriteLine("\nDigite:");

                Console.WriteLine("1 - Cadastrar novo cliente");
                Console.WriteLine("2 - Listar clientes cadastrados");
                Console.WriteLine("3 - Exibir log");
                Console.WriteLine("ou qualquer tecla para Sair");

                if (Int32.TryParse(Console.ReadLine(), out opcao))
                {
                    switch (opcao)
                    {
                        case 1:
                            Program.cadastraCliente(clientes, logs);
                            break;
                        case 2:
                            Program.exibeRegistros(clientes);
                            break;
                        case 3:
                            Program.exibeRegistros(logs);
                            break;
                        default: break;
                    }
                }
            }
        }

        private static void cadastraCliente(Dictionary<string, Cliente> clientes, Dictionary<string, DateTime> logs)
        {
            var cliente = new Cliente();
            Console.WriteLine("\n* - campo de preenchimento obrigatorio:");

            Console.WriteLine("\nNome*:");
            cliente.Nome = Program.validaPreenchimento();

            Console.WriteLine("CPF*: ");
            cliente.CPF = Program.validaUnicidadeCPF(clientes);
            if (!cliente.CPF.Equals("0"))   //¬_¬ 
            {
                Console.WriteLine("Credito*: ");
                if (Decimal.TryParse(Program.validaPreenchimento(), out decimal creditoToParse))
                { cliente.Credito = creditoToParse; }

                bool respostaOk = false;
                while (!respostaOk)
                {
                    Console.WriteLine("Sexo* (Digite 1 para Masculino ou 2 para Feminino): ");
                    if (Int32.TryParse(Program.validaPreenchimento(), out int sexoToParse) && (sexoToParse == 1 || sexoToParse == 2))
                    {
                        cliente.Sexo = (Cliente.TipoSexo)sexoToParse;
                        respostaOk = true;
                        break;
                    }
                    Console.WriteLine("Valor invalido! Responda 1 ou 2");
                }

                Console.WriteLine("\n\nENDEREÇO");

                Console.WriteLine("\nRua*: ");
                cliente.EnderecoCliente.Rua = Program.validaPreenchimento();

                Console.WriteLine("Numero*: ");
                cliente.EnderecoCliente.Numero = Program.validaPreenchimento();

                Console.WriteLine("Cidade*: ");
                cliente.EnderecoCliente.Cidade = Program.validaPreenchimento();

                Console.WriteLine("Estado*: ");
                cliente.EnderecoCliente.Estado = Program.validaPreenchimento();

                Console.WriteLine("Complemento: ");
                cliente.EnderecoCliente.Cidade = Console.ReadLine();

                Console.WriteLine("\n\nDEPENDENTES");

                respostaOk = false;
                while (!respostaOk)
                {
                    Console.WriteLine("Digite a quantidade de dependentes que serão cadastrados (min=0 max=2)*");

                    if (Int32.TryParse(Console.ReadLine(), out int qndDepte))
                    {
                        if (qndDepte > 2 || qndDepte < 0)
                        {
                            Console.WriteLine("Quantidade inválida");
                            continue;
                        }

                        for (int i = 0; i < qndDepte; i++)
                        {
                            Console.WriteLine("Nome do parente* " + i + ":");
                            cliente.DependenteCliente.Nome = Program.validaPreenchimento();
                            bool valorValido = false;
                            while(!valorValido)
                            {
                                Console.WriteLine("Parentesco* (Digite 1 para conjuge e 2 para filho)");
                                
                                if (Int32.TryParse(Program.validaPreenchimento(), out int parentescoToParse) && (parentescoToParse > 0 && parentescoToParse < 3))
                                {
                                    cliente.DependenteCliente.parentesco = (Cliente.Dependente.Parentesco)parentescoToParse;
                                    valorValido = true;
                                }
                                else
                                {
                                    Console.WriteLine("\nValor inválido\n");
                                }
                            }
                        }
                        respostaOk = true;
                    }
                    else
                    {
                        Console.WriteLine("\nValor inválido\n");
                    }
                }

                cliente.DataCadastro = DateTime.Today.ToShortDateString();
                clientes.Add(cliente.CPF, cliente);
                logs.Add(cliente.CPF, DateTime.Now);

                Console.WriteLine("\nCADASTRO FINALIZADO COM SUCESSO!\n\n");
            }
        }
        private static void exibeRegistros<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
        {
            Console.WriteLine("\n\n");
            Console.WriteLine("______________________________________________________________________________");
            Console.WriteLine("\n                      LISTANDO REGISTROS");
            foreach (var pair in dictionary)
            {
                Console.WriteLine("{0} - {1}", pair.Key, pair.Value);
            }
            Console.WriteLine("\n\n");
        }

        private static string validaPreenchimento()
        {
            string valor;
            while (string.IsNullOrEmpty(valor = Console.ReadLine()))
                Console.WriteLine("Preenchimento do campo é obrigatorio! Insira um valor: ");

            return valor;
        }

        private static string validaUnicidadeCPF(Dictionary<string, Cliente> clientes)
        {
            var cliente = new Cliente();
            string cpf = Program.validaPreenchimento();
            if (clientes.TryGetValue(cpf, out cliente))
            {
                Console.WriteLine("CPF já cadastrado!");
                Console.WriteLine("Finalizando operação...");

                return "0";
            }
            return cpf;
        }

        class Cliente
        {
            public string Nome { get; set; }
            public string CPF { get; set; }
            public decimal Credito { get; set; }
            public TipoSexo Sexo { get; set; }
            public Endereco EnderecoCliente { get; set; }
            public Dependente DependenteCliente { get; set; }
            public string DataCadastro { get; set; }

            public Cliente()
            {
                this.EnderecoCliente = new Endereco();
                this.DependenteCliente = new Dependente();
            }


            public enum TipoSexo
            {
                Masculino = 1,
                Feminino = 2
            }

            public class Endereco
            {
                public string Rua { get; set; }
                public string Numero { get; set; }
                // public TipoEstado Estado { get; set; }
                public string Estado { get; set; }
                public string Cidade { get; set; }
                public string Complemento { get; set; }
            }

            public class Dependente
            {
                public string Nome { get; set; }
                public Parentesco parentesco { get; set; }
                public enum Parentesco
                {
                    conjuge, filho
                }
            }

            public override string ToString()
            {
                return JsonConvert.SerializeObject(this, Formatting.Indented);
            }
        }
    }
}
