using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace FreeSchema1._1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpPost("GetData")]
        public Dictionary<string, dynamic> GetData([FromBody] dynamic forecast) {

            Dictionary<string, dynamic> initialData = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(forecast.GetRawText());

            Dictionary<string, dynamic> finalData = new Dictionary<string, dynamic>();

            foreach (KeyValuePair<string, dynamic> kvp in initialData)
            {
                dynamic data;
                Console.WriteLine(kvp.Value.GetType());

                if (kvp.Value.GetType() == typeof(JArray))
                {
                    var i = 0;
                    Dictionary<string, dynamic> temp = new Dictionary<string, dynamic>();
                    foreach (dynamic item in kvp.Value)
                    {
                        temp.Add( i.ToString(), item);
                        i++;
                    }
                 
                    data = JsonConvert.SerializeObject(temp);

                }
                else if (kvp.Value.GetType() == typeof(JObject))
                {
                    Console.WriteLine("Object Found");

                    data = GetData(kvp.Value);
                    Console.WriteLine(data);
                }
                else
                {
                    data = kvp.Value;
                }

                // Console.WriteLine(data);

                finalData.Add(kvp.Key, data);
            }

            return finalData;

            //     Console.WriteLine(JsonConvert.SerializeObject(finalData));

        }

      /*  private void getChild(dynamic data)
        {

            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(data.GetRawText());
            //Console.WriteLine(dictionary);

            Dictionary<string, dynamic> finalData = new Dictionary<string, dynamic>();


            //Console.WriteLine(item);
            foreach (KeyValuePair<string, object> kvp in dictionary)
            {

                dynamic json = kvp.Value;

                if (json.GetType() == typeof(JArray))
                {
                    // Console.WriteLine(json);
                    var i = 0;
                    Dictionary<string, dynamic> temp = new Dictionary<string, dynamic>();
                    foreach (JObject item in json)
                    {

                        temp.Add(i.ToString(), item);

                        i++;
                    }

                    data = JsonConvert.SerializeObject(temp);

                    foreach (KeyValuePair<string, object> kvp_data in data)
                    {
                        if (kvp_data.GetType() == typeof(JArray))
                        {
                            getChild(kvp_data.Value);
                        }
                        
                    }


                        
                }
                

          
            }


        }*/
    }
}