using BrokerEngine.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ProducerMsgBroker.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ProducerController : ControllerBase
	{
		[HttpPost(Name = "Postar Mensagem na fila")]
		public IActionResult Post([FromBody] IEnumerable<string> mensagens, [FromServices] IMessageBrokerProducer producer)
		{
			try
			{
				foreach (var mensagem in mensagens)
				{
					Console.WriteLine($"--> Publishing message --> {mensagem}");
						 
					producer.Publish(mensagem);

					Console.WriteLine($"--> Published message --> {mensagem}");
				}
				
				return NoContent();
			}
			catch(Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}