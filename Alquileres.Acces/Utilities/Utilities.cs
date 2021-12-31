using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text;

namespace Alquileres.Acces.Utilities
{
    public class Utilities
    {

        /// <summary>
        /// Metodo para obtener la respuesta de un htttp
        /// </summary>
        /// <param name="strEndPoint">End point del cual se dese obtener la respuesta</param>
        /// <returns>Retorna un string indicando la respuesta</returns>
        public async Task<string> GetResponseHttp(string strEndPoint)
        {
            try
            {
                var httpClient = new HttpClient();
                var result = await httpClient.GetStringAsync(strEndPoint);

                return result;
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// Metodo para enviar request http
        /// </summary>
        /// <param name="strEndPoint">End point del cual se dese obtener la respuesta</param>
        /// <returns>Retorna un string indicando la respuesta</returns>
        public async Task<string> SendRequestHttp(string strEndPoint, string strJsonBody)
        {
            try
            {
                HttpClient client = new HttpClient();
                //below is incorrect

                var content = new StringContent(strJsonBody.ToString(), Encoding.UTF8, "application/json");
                var result = await client.PostAsync(strEndPoint, content);

                return result.Content.ReadAsStringAsync().Result;
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Metodo para deserializar la respuesta de un json
        /// </summary>
        /// <param name="jsonBody"></param>
        /// <returns>Retorna un objeto el cual despues puede ser convertido en cualquier tipo que se desee manejar</returns>
        public object? DeserializeObjectJson(string jsonBody, Type entitie)
        {
            try
            {
                var response = JsonConvert.DeserializeObject(jsonBody, entitie);
                return response;
            }
            catch
            {
                throw;
            }
        }

        public void SaveFile(HttpContext content, string filePath)
        {
            try
            {
                using (var inputStream = new FileStream(filePath, FileMode.Create))
                {
                    // read file to stream
                    content.Request.Form.Files[0].CopyTo(inputStream);
                    // stream to byte array
                    byte[] array = new byte[inputStream.Length];
                    inputStream.Seek(0, SeekOrigin.Begin);
                    inputStream.Read(array, 0, array.Length);
                    inputStream.Close();
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
