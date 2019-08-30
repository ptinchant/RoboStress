using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZurichAPI.StressTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string executions = string.Empty;
            long initialOrder = 0;
            long initialQuote = 0;
            long execs = 0;
            long counter = 0;
            do
            {
                Console.WriteLine("Insira número de pedido inicial.");
                executions = Console.ReadLine();
            } while (!long.TryParse(executions, out initialOrder));
            do
            {
                Console.WriteLine("Insira cotação inicial.");
                executions = Console.ReadLine();
            } while (!long.TryParse(executions, out initialQuote));
            do
            {
                Console.WriteLine("Insira a quantidade de execuções a serem realizadas.");
                executions = Console.ReadLine();
            } while (!long.TryParse(executions, out execs));

            Console.WriteLine("Executando...");
            if (!Directory.Exists("Logs"))
            {
                Directory.CreateDirectory("Logs");
            }
            string fileName = string.Format("Logs\\{0}_log.csv", DateTime.Now.ToString("ddMMyyyy_HHmmss"));
            Stress stress = new Stress(fileName);
            List<Task> tasks = new List<Task>();
            var result = Parallel.For(1, execs + 1, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount == 1 ? 1 : Environment.ProcessorCount - 1 }, i =>
                 {
                     stress.Execute(GetData(initialOrder, initialQuote), i);
                     initialOrder++;
                     initialQuote++;
                 });

            stress.CloseFile();
            Console.WriteLine("Finalizado Pressione alguma tecla para encerrar...", Stress.executingItens);

            Console.ReadLine();
        }

        private static string GetData(long pedido, long cotacao)
        {
            StringBuilder result = new StringBuilder();

            result.AppendLine("{");
            result.AppendLine("\"DataAtual\": \"" + DateTime.Now.ToString("yyyy-MM-dd") + "\",");
            result.AppendLine("\"NumeroCotacao\": \"" + cotacao.ToString().PadLeft(15, '0').Substring(0, 15) + "\",");
            result.AppendLine("\"NumeroPedido\": \"" + pedido.ToString().PadLeft(10, '0').Substring(0, 10) + "\",");
            result.AppendLine("\"DataValidadeCotacaoz\": \"" + DateTime.Now.AddDays(30).ToString("yyyy-MM-dd") + "\",");
            result.AppendLine("\"CodigoProduto\": \"56\",");
            result.AppendLine("\"CodigoSubProduto\": \"1862\",");
            result.AppendLine("\"CodigoSeguradora\": \"0026\",");
            result.AppendLine("\"CodigoCanal\": \"936\",");
            result.AppendLine("\"TipoPacote\": \"0005\",");
            result.AppendLine("\"Seguro\": {");
            result.AppendLine("\"IndicadorSeguroNovo\":\"\",");
            result.AppendLine("\"CepPernoite\": null");
            result.AppendLine("},");
            result.AppendLine("\"Segurado\": {");
            result.AppendLine("\"NomeSegurado\": \"\",");
            result.AppendLine("\"CodigoTipoPessoa\": \"13\",");
            result.AppendLine("\"NumeroDocumento\": \"28436058810\",");
            result.AppendLine("\"EstadoCivilSegurado\": \" \",");
            result.AppendLine("\"SexoSegurado\": \" \",");
            result.AppendLine("\"DataNascimentoSegurado\": \"1979-06-13\",");
            result.AppendLine("\"CodigoGrupoSegurado\": \"2017\",");
            result.AppendLine("\"DDContato\": \"11\",");
            result.AppendLine("\"TelefoneContato\": \"991870728\"");
            result.AppendLine("},");
            result.AppendLine("\"Veiculo\": {");
            result.AppendLine("\"CodigoFipe\": \"0000054046\",");
            result.AppendLine("\"DescricaoFipe\": \"FOX COMFORTLINE I MOTION 1.6 FLEX 8V 5P \",");
            result.AppendLine("\"AnoModelo\": \"2015\",");
            result.AppendLine("\"ZeroKm\": \"N\",");
            result.AppendLine("\"DescricaoMarcaVeiculo\": \"VW - VOLKSWAGEN \",");
            result.AppendLine("\"TipoCombustivel\": \"GG\",");
            result.AppendLine("\"Placa\": \"\",");
            result.AppendLine("\"IndicadorChassiRemarcado\": \" \",");
            result.AppendLine("\"Chassi\": \"\",");
            result.AppendLine("\"TipoUsoVeiculo\": \"0000000001\",");
            result.AppendLine("\"IndicadorDeficienteFisico\": \"N\"");
            result.AppendLine("},");
            result.AppendLine("	\"Comissao\": {");
            result.AppendLine("\"PercentualComissao\": \"02200000000\",");
            result.AppendLine("\"IndicadorFlexibilizacao\": \"N\",");
            result.AppendLine("\"PercentualDescontoFlexibilizacao\": \"00000000000\",");
            result.AppendLine("\"CodigoSeguradoraMercado\": \"\",");
            result.AppendLine("\"ValorCotacaoMercado\": \"000000000000000\",");
            result.AppendLine("\"CodCorretorSusepSantander\": \"1020415729\",");
            result.AppendLine("\"CNPJCorretorSantander\": \"04270778000171\",");
            result.AppendLine("\"TipoCorretor\": \"0000000001\",");
            result.AppendLine("\"FlagCorretorPrincipal\": \"0000000001\",");
            result.AppendLine("\"PercentualComissaoSantander\": \"00000000000\",");
            result.AppendLine("\"CodCorretorSusepDemais\": \"\",");
            result.AppendLine("\"CNPJCorretorDemais\": \"047253307000168 \",");
            result.AppendLine("\"PercentualComissaoDemais\": \"00000000000\"");
            result.AppendLine("},");
            result.AppendLine("	\"Coberturas\": {");
            result.AppendLine("\"CoberturaCasco\": \"199\",");
            result.AppendLine("\"PercentualFipe\": \"100\",");
            result.AppendLine("\"CodigoFranquia\": \"0001\",");
            result.AppendLine("\"CoberturaCarroReserva\": \" \",");
            result.AppendLine("\"DiasCarroReserva\": \"00\",");
            result.AppendLine("\"CoberturaVidros\": \" \",");
            result.AppendLine("\"TipoAssistencia\": \"219\",");
            result.AppendLine("\"IndicadorAcessoriosAdicionais\": \"N\",");
            result.AppendLine("\"ValorAcessoriosAdicionais\": \"000000000000000\",");
            result.AppendLine("\"IndicadorVeiculoBlindado\": \"N\",");
            result.AppendLine("\"DataBlindagem\": \"\",");
            result.AppendLine("\"ValorBlindagem\": \"000000000000000\",");
            result.AppendLine("\"IndicadorKitGas\": \"N\",");
            result.AppendLine("\"DataInstalacaoKitGas\": \"\",");
            result.AppendLine("\"ValorKitGas\": \"000000000000000\",");
            result.AppendLine("\"DanosMateriais\": \"000000010000000\",");
            result.AppendLine("\"DanosCorporais\": \"000000010000000\",");
            result.AppendLine("\"DanosMorais\": \"000000001000000\",");
            result.AppendLine("\"ValorAppMorte\": \"000000000500000\",");
            result.AppendLine("\"ValorAppInvalidez\": \"000000000500000\",");
            result.AppendLine("\"ValorDespesaExtraordinaria\": \" \",");
            result.AppendLine("\"DiasParalisacao\": \"000\",");
            result.AppendLine("\"ValorEquipamento\": \"000000000000000\",");
            result.AppendLine("\"ValorCarroceria\": \"000000000000000\",");
            result.AppendLine("\"TipoCarroceria\": \" \",");
            result.AppendLine("\"IndicadorBloqueadorRastreador\": \"N\",");
            result.AppendLine("\"CodigoModeloBloqueadorRastreador\": \"\",");
            result.AppendLine("\"QuantidadeMeses\": \"012\"");
            result.AppendLine("},");
            result.AppendLine("	\"Condutor\": {");
            result.AppendLine("\"IndicadorPossuiCnh\": \"S\"");
            result.AppendLine("},");
            result.AppendLine("	\"Perfil\": {");
            result.AppendLine("\"IndicadorSeguradoCondutorVeiculo\": null");
            result.AppendLine("}");
            result.AppendLine("}");
            return result.ToString();
        }
    }
}
