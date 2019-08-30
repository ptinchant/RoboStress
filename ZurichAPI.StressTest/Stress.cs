using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ZurichAPI.StressTest
{
    public class Stress
    {
        public static long executingItens;
        public static long executed;
        private static string apiToken = "c2FudGFuZGVyOmY2Q3RLd1RZYXk=";
        private static string apiUrl = "https://cotaz.zurich.com.br/ZAS.Tradutor/api/Cotacao";
        private static HttpClient client = new HttpClient();
        private static StreamWriter logFile;
        public Stress(string fileName)
        {
            if (logFile == null)
            {
                logFile = File.CreateText(fileName);
                logFile.AutoFlush = true;
                logFile.WriteLine("{0};{1};{2};{3};{4};{5};{6};",
                   "#",
                  "Inicio Execucao",
                  "Dados Enviados",
                  "Retorno",
                  "Erro",
                  "Fim Execucao",
                  "Status");
            }
        }

        public bool IsExecuting()
        {
            return executingItens > 0;
        }

        public void Execute(string data, long executionId)
        {            
            Execution execution = new Execution();
           
            execution.StartDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            execution.SentData = data;
            execution.ExecutionId = executionId;
            execution.ErrorData = string.Empty;
            try
            {
                var result = CallAPI(data);
                execution.ResultData = result.Value;
                if (result.Key)
                    execution.State = "SUCCESS";
                else
                    execution.State = "ERROR";
                execution.ExecutionFinished = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            }
            catch (Exception ex)
            {
                execution.State = "ERROR";
                execution.ExecutionFinished = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                execution.ErrorData = ex.Message;
            }
            finally
            {
                try
                {
                    logFile.WriteLine("{0};{1};{2};{3};{4};{5};{6};",
                        execution.ExecutionId,
                        execution.StartDate,
                        execution.SentData.Replace(Environment.NewLine, " "),
                        execution.ResultData.Replace(Environment.NewLine, " "),
                        execution.ErrorData.Replace(Environment.NewLine, " "),
                        execution.ExecutionFinished,
                        execution.State);
                }
                catch { }
            }
        }

        public void CloseFile()
        {
            logFile.Close();
        }
        private KeyValuePair<bool, string> CallAPI(string data)
        {
            KeyValuePair<bool, string> result = new KeyValuePair<bool, string>(true, string.Empty);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);
            request.Headers.Add("auth-token", apiToken);
            request.Method = "POST";

            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            Byte[] byteArray = encoding.GetBytes(data);

            request.ContentLength = byteArray.Length;
            request.ContentType = @"application/json";

            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                        new KeyValuePair<bool, string>(true, reader.ReadToEnd());
                    }
                }
            }
            catch (WebException ex)
            {
                if (((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.BadRequest)
                {
                    using (Stream stream = ex.Response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                        new KeyValuePair<bool, string>(false, reader.ReadToEnd());

                    }

                }
                else
                    throw ex;
            }
            return result;
        }
    }
}
