using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Lingo_WebApp.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class LingoController : Controller
    {
        private static readonly string[] _lingoWords = new[]{
            "Afijn", "Credo", "Deern", "Duaal", "Echec", "Egard", "Koket", "Ethos", "Fluks", "Frêle"
        };

        private LingoWord _currentWord { get; set; }

        private readonly ILogger<LingoController> _logger;

        public LingoController(ILogger<LingoController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<LingoWord>> Get()
        {
            Random rn = new Random(Guid.NewGuid().GetHashCode());
            if (_currentWord == null)
            {
                _currentWord = new LingoWord(_lingoWords.ElementAt(rn.Next(0, _lingoWords.Length - 1)));
            }
            return _currentWord;
        }

        [HttpPost, ActionName("Guess"), Route("Guess")]
        public async Task<ActionResult<Item>> GetGuess(Item word)
        {
            try
            {
                _currentWord.Check(word.name);
                return new Item { isComplete = true };
            }
            catch (Exception ex)
            {
                return new Item { isComplete = false };
            }
        }

        [HttpGet, Route("Reset")]
        public async Task<ActionResult<LingoWord>> Reset()
        {
            Random rn = new Random(Guid.NewGuid().GetHashCode());
            _currentWord = new LingoWord(_lingoWords.ElementAt(rn.Next(0, _lingoWords.Length - 1)));
            return _currentWord;
        }
    }

    public class Item
    {
        public bool isComplete { get; set; }
        public string name { get; set; }
    }
}
