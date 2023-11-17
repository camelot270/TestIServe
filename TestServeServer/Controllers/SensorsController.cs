using Microsoft.AspNetCore.Mvc;
using TestIServe.Server.Sensors.Domain;
using TestIServe.Server.Storage.Queries;

namespace TestIServe.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SensorsController : Controller
    {
        private readonly ILogger<SensorsController> _logger;
        private readonly ISensorRepository _sensorRepository;
        private readonly ISensorsQueryService _sensorsQueryService;

        public SensorsController(ILogger<SensorsController> logger, ISensorRepository sensorRepository, ISensorsQueryService sensorsQueryService)
        {
            _logger = logger;
            _sensorRepository = sensorRepository;
            _sensorsQueryService = sensorsQueryService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return Ok(_sensorRepository.Sensors);
        }

        [HttpGet("{sensorId}/GetTemperature")]
        public IActionResult GetTemperature(string sensorId)
        {
            if (!_sensorRepository.Sensors.ContainsKey(sensorId))
            {
                return NotFound("Датчик не найден");
            }
            return Ok(_sensorRepository.Sensors[sensorId].GetTemperature());
        }

        [HttpGet("{sensorId}/GetHumidifier")]
        public IActionResult GetHumidifier(string sensorId)
        {
            if (!_sensorRepository.Sensors.ContainsKey(sensorId))
            {
                return NotFound("Датчик не найден");
            }
            return Ok(_sensorRepository.Sensors[sensorId].GetHumidifier());
        }

        [HttpGet("{sensorId}/GetCo2")]
        public IActionResult GetCo2(string sensorId)
        {
            if (!_sensorRepository.Sensors.ContainsKey(sensorId))
            {
                return NotFound("Датчик не найден");
            }
            return Ok(_sensorRepository.Sensors[sensorId].GetCarbonDioxideContent());
        }

        [HttpGet("{sensorId}/GetAverageTemperature")]
        public IActionResult GetAverageTemperature(string sensorId, DateTimeOffset from, int countMinutes)
        {
            if (!_sensorRepository.Sensors.ContainsKey(sensorId))
            {
                return NotFound("Датчик не найден");
            }

            var res = _sensorsQueryService.GetAverageTemperature(sensorId, from, countMinutes);

            if (res == null)
            {
                return NotFound("Данные по диапазону не найдены");
            }

            return Ok(res);
        }

        [HttpGet("{sensorId}/GetAverageHumidifier")]
        public IActionResult GetAverageHumidifier(string sensorId, DateTimeOffset from, int countMinutes)
        {
            if (!_sensorRepository.Sensors.ContainsKey(sensorId))
            {
                return NotFound("Датчик не найден");
            }

            var res = _sensorsQueryService.GetAverageHumidifier(sensorId, from, countMinutes);

            if (res == null)
            {
                return NotFound("Данные по диапазону не найдены");
            }

            return Ok(res);
        }

        [HttpGet("{sensorId}/GetMinCarbonDioxideContent")]
        public IActionResult GetMinCarbonDioxideContent(string sensorId, DateTimeOffset from, int countMinutes)
        {
            if (!_sensorRepository.Sensors.ContainsKey(sensorId))
            {
                return NotFound("Датчик не найден");
            }

            var res = _sensorsQueryService.GetMinCarbonDioxideContent(sensorId, from, countMinutes);

            if (res == null)
            {
                return NotFound("Данные по диапазону не найдены");
            }

            return Ok(res);
        }

        [HttpGet("{sensorId}/GetMaxCarbonDioxideContent")]
        public IActionResult GetMaxCarbonDioxideContent(string sensorId, DateTimeOffset from, int countMinutes)
        {
            if (!_sensorRepository.Sensors.ContainsKey(sensorId))
            {
                return NotFound("Датчик не найден");
            }

            var res = _sensorsQueryService.GetMaxCarbonDioxideContent(sensorId, from, countMinutes);

            if (res == null)
            {
                return NotFound("Данные по диапазону не найдены");
            }

            return Ok(res);
        }
    }
}