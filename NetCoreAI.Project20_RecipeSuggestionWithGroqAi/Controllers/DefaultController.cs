using Microsoft.AspNetCore.Mvc;
using NetCoreAI.Project20_RecipeSuggestionWithGroqAi.Models;

namespace NetCoreAI.Project20_RecipeSuggestionWithGroqAi.Controllers
{
    public class DefaultController : Controller
    {
        private readonly GroqAiService _groqAiService;

        public DefaultController(GroqAiService groqAiService)
        {
            _groqAiService = groqAiService;
        }

        [HttpGet]
        public IActionResult CreateRecipe()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRecipe(string ingredients)
        {
            var result = await _groqAiService.GetRecipeAsync(ingredients);
            ViewBag.recipe = result;
            return View();
        }
    }
}
