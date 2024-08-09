using ClientService.API.Entities;
using ClientService.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ClientService.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ClientController:ControllerBase
    {
        private readonly IRepository _repository;

        public ClientController(IRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Client>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Client>>> GetClients()
        {
            var clients = await _repository.GetClients();
            return Ok(clients);
        }

        [HttpGet("{id}", Name = "GetClient")]
        [ProducesResponseType(typeof(IEnumerable<Client>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Client>> GetClientById(string id)
        {
            var result = await _repository.GetClientById(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [Route("[action]/{name}")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Client>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Client>>> GetClientsByName(string name)
        {
            var results = await _repository.GetClientsByName(name);
            return Ok(results);
        }

        [Route("[action]/{surname}")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Client>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Client>>> GetClientsBySurname(string surname)
        {
            var results = await _repository.GetClientsBySurname(surname);
            return Ok(results);
        }

        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<Client>), StatusCodes.Status201Created)]
        public async Task<ActionResult<Client>> CreateClient([FromBody] Client client)
        {
            await _repository.CreateClient(client);
            return CreatedAtRoute("GetClient", new { id = client.Id }, client);

        }

        [HttpPut]
        [ProducesResponseType(typeof(Client), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateClient([FromBody] Client client)
        {
            return Ok(await _repository.UpdateClient(client));
        }

        [HttpDelete("{id}", Name = "DeleteClient")]
        [ProducesResponseType(typeof(Client), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteClient(string id)
        {
            return Ok(await _repository.DeleteClient(id));
        }

        [Route("[action]")]
        [HttpDelete]
        [ProducesResponseType(typeof(Client), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteAllClients()
        {
            await _repository.DeleteAllClients();
            return Ok();
        }
    }
}
