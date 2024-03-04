using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Soneta.Core.KSeF.API;
using System.Net;
using WebApplicationYes.Attributes;
using WebApplicationYes.Models;
using WebApplicationYes.Models.AdditionalClasses; 

namespace WebApplicationYes.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class OrdersController : Controller
    {
        [HttpGet]
        [BasicAuthFilter]
        public IActionResult Orders(string orderNumber = "", string customerIdentifier = "", string date = "")
        {

            try
            {
                BusinessLoader.Load(EnovaConfig.EnovaFolder, false, false, true);

                if (string.IsNullOrEmpty(orderNumber) && string.IsNullOrEmpty(date))
                    return BadRequest("Nie przekazano wymaganych parametrów");

                if (!string.IsNullOrEmpty(orderNumber))
                {
                    var order = (Document)new Documents().GetRow(new string[] { orderNumber, customerIdentifier, date });
                    if (order == null)
                        return BadRequest($"Nie znaleziono dokumentu zamówienia o numerze {orderNumber}");
                    else return Ok(order);
                }
                else
                {
                    var orders = (List<Document>)new Documents().GetRow(new string[] { orderNumber, customerIdentifier, date });
                    if (orders == null || !orders.Any())
                        return BadRequest($"Nie znaleziono dokumentu z dnia {date}");
                    else return Ok(orders);
                }
            }
            catch (Exception e)
            {
                return BadRequest($"Błąd przetwarzania zapytania: {e.Message}");
            }
        }
    }
}
