using Microsoft.AspNetCore.Mvc;
using Waste.API.Models;
using WasteManagement.API.Data;

namespace WasteManagement.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LookupController : ControllerBase
    {
        private readonly ILogger<LookupController> _logger;

        public LookupController(ILogger<LookupController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetLookupData")]
        public IEnumerable<LookupData> Get()
        {
            List<LookupData> data = new List<LookupData>();
            
            foreach (var item in ReadWasteTypes())
            {
                LookupData lookupData = new LookupData();
                lookupData.Id = item.Id;
                lookupData.Name = item.Name;
                data.Add(lookupData);
            }

            return data.ToArray();
        }

        private List<WasteType> ReadWasteTypes()
        {
            List<WasteType> types = new List<WasteType>();
            using (var db = new WasteContext())
            {
                var query = from b in db.WasteType
                            orderby b.Name
                            select b;
                types = db.WasteType.ToList();
            }
            return types;
        }
    }
}