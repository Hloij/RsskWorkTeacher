using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ConsoleApp2
{

    public class APIMetods1
    {

        // Создаем статичный экземпляр HttpClient
        //private static readonly HttpClient _httpClient = new HttpClient();
        private static readonly HttpClient _httpClient =
            new HttpClient(new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback =
                (message, cert, chain, errors) => true
            });
        // Задаем базовый адрес для доступа к API
        private static readonly string _apiBaseUrl = "http://192.168.24.6/rasp/odata/standard.odata/";
        static string _username = "odata";

        /// <summary>
        /// Получение списка 
        /// </summary>
        /// <typeparam name="T"> Тип коллекции, например List<Abonents> </typeparam>
        /// <param name="endPoint"> Имя точки подключения </param>
        /// <returns> Коллекция указанного типа </returns>
        public async static Task<T> Get<T>(string endPoint)
        {
            // Преобразуем имя пользователя и пароль в строку для Basic Authentication
            // var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_username}:{Class1.Decrypt("97SW8poLBglE3mK13PhrpA==")}"));
            // Добавляем заголовок авторизации
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_username}:{Class1.Decrypt("97SW8poLBglE3mK13PhrpA==")}")));
            //Получаем сообщение с WEB API
            var response =  _httpClient.GetAsync(_apiBaseUrl + endPoint).Result;
            //Если ошибка возвращаем значение по умолчанию
            if (!response.IsSuccessStatusCode)
            {
                return default(T);
            }
            //Из полученного сообщения извлекаем данные в формате JSON
            var content = response.Content.ReadAsStringAsync().Result;
            //Преобразуем JSON в набор данных
            var data = JsonConvert.DeserializeObject<T>(content);
            return data;
        }
        public static T GetXML<T>(string endPoint)
        {

            //Получаем сообщение с WEB API
            var response = _httpClient.GetAsync(_apiBaseUrl + endPoint).Result;
            //Если ошибка возвращаем значение по умолчанию
            if (!response.IsSuccessStatusCode)
            {
                return default(T);
            }
            //Из полученного сообщения извлекаем данные в формате JSON
            var content = response.Content.ReadAsStringAsync().Result;
            //Преобразуем JSON в набор данных



            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

            using (StringReader reader = new StringReader(content))
            {
                var feed = (T)xmlSerializer.Deserialize(reader);

                return feed;
            }


        }


        /// <summary>
        /// Добавление записи
        /// </summary>
        /// <typeparam name="T"> Класс сущности </typeparam>
        /// <param name="body"> Добавляемая сущность </param>
        /// <param name="endPoint"> Имя точки подключения </param>
        /// <returns> Строка запроса к серверу приложений </returns>
        /// <exception cref="HttpRequestException"></exception>
        public static string Post<T>(T body, string endPoint)
        {
            //Конвертируем данные в JSON
            var json = JsonConvert.SerializeObject(body);
            //Формируем строку запроса к WEB API
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            //Формируем запроса к WEB API
            var result = _httpClient.PostAsync(_apiBaseUrl + endPoint, content).Result;
            //Если ошибка генерируем исключение
            if (!result.IsSuccessStatusCode)
            {
                throw new HttpRequestException(result.ReasonPhrase);
            }

            //Возвращаем строку запроса
            return result.ToString();
        }


        /// <summary>
        /// Редактирование записи по Id
        /// </summary>
        /// <typeparam name="T"> Класс сущности </typeparam>
        /// <param name="body"> Изменяемая сущность </param>
        /// <param name="id"> id изменяемой сущности </param>
        /// <param name="endPoint"> Имя точки подключения </param>
        /// <returns> Строка запроса к серверу приложений </returns>
        /// <exception cref="HttpRequestException"></exception>
        public static string Put<T>(T body, int id, string endPoint)
        {
            var json = JsonConvert.SerializeObject(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var result = _httpClient.PutAsync(_apiBaseUrl + endPoint + "/" + id.ToString(),
                content).Result;
            if (!result.IsSuccessStatusCode)
            {
                throw new HttpRequestException(result.ReasonPhrase);
            }
            return result.ToString();
        }


        /// <summary>
        /// Удаление записи по Id
        /// </summary>
        /// <param name="id"> id удаляемой записи  </param>
        /// <param name="endPoint"> Имя точки подключения </param>
        /// <returns> Строка запроса к серверу приложений </returns>
        public static string Delete(int id, string endPoint)
        {
            var result =
                _httpClient.DeleteAsync(_apiBaseUrl + endPoint + "/" + id.ToString()).Result;
            return result.ToString();
        }


        //public static T Get1<T>(string endPoint)
        //{
        //    //  client.BaseAddress = new Uri(Url);
        //    //client.Encoding = Encoding.UTF8;
        //    var response = _httpClient.GetFromJsonAsync<T>(_apiBaseUrl + endPoint).Result;
        //    //HttpStatusCode.Unauthorized.ToString();
        //    return response;
        //}
        //public static string Post1<T>(T body, string endPoint)
        //{
        //    //   client.BaseAddress = new Uri(Url);
        //    _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    var result = _httpClient.PostAsJsonAsync<T>(_apiBaseUrl + endPoint, body).Result;
        //    if (!result.IsSuccessStatusCode)
        //    {
        //        throw new HttpRequestException(result.ReasonPhrase);
        //    }

        //    return result.ToString();
        //}
        //public static string Put1<T>(T body, int id, string endPoint)
        //{

        //    var result = _httpClient.PutAsJsonAsync<T>(_apiBaseUrl + endPoint + "/" + id.ToString(), body).Result;
        //    if (!result.IsSuccessStatusCode)
        //    {
        //        throw new HttpRequestException(result.ReasonPhrase);
        //    }
        //    return result.ToString();
        //}
    }
    public class Class1
    {
        private static readonly byte[] Key = Encoding.UTF8.GetBytes("1234567812345678"); // 16 байт для AES-128
        private static readonly byte[] IV = Encoding.UTF8.GetBytes("1234567812345678"); // 16 байт для AES

        public static string Encrypt(string plainText)
        {

            using (var aes = Aes.Create())
            {
                aes.Key = Key;
                aes.IV = IV;

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    using (var ms = new MemoryStream())
                    {
                        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        {
                            using (var sw = new StreamWriter(cs))
                            {
                                sw.Write(plainText);
                            }
                            return Convert.ToBase64String(ms.ToArray());
                        }
                    }
                }
            }
        }

        public static string Decrypt(string cipherText)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = Key;
                aes.IV = IV;

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    using (var ms = new MemoryStream(Convert.FromBase64String(cipherText)))
                    {
                        using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                        {
                            using (var sr = new StreamReader(cs))
                            {
                                return sr.ReadToEnd();
                            }
                        }
                    }
                }
            }
        }
    }
}

