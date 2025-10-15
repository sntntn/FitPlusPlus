using AutoMapper;
using ClientService.Common.Entities;
using ClientService.Common.Repositories;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClientService.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ClientController:ControllerBase
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public ClientController(IRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [Authorize(Roles = "Admin, Trainer")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Client>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Client>>> GetClients()
        {
            var clients = await _repository.GetClients();
            return Ok(clients);
        }

        [Authorize(Roles = "Admin, Trainer, Client")]
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
        
        // TODO("Dodati GetClientsByIds - mozda gRPC!!!")

        [Authorize(Roles = "Admin, Trainer, Client")]
        [Route("[action]/{name}")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Client>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Client>>> GetClientsByName(string name)
        {
            var results = await _repository.GetClientsByName(name);
            return Ok(results);
        }

        [Authorize(Roles = "Admin, Trainer, Client")]
        [Route("[action]/{surname}")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Client>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Client>>> GetClientsBySurname(string surname)
        {
            var results = await _repository.GetClientsBySurname(surname);
            return Ok(results);
        }

        [Authorize(Roles = "Admin, Trainer, Client")]
        [Route("[action]/{email}")]
        [HttpGet]
        [ProducesResponseType(typeof(Client), StatusCodes.Status200OK)]
        public async Task<ActionResult<Client>> GetClientByEmail(string email)
        {
            var result = await _repository.GetClientByEmail(email);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<Client>), StatusCodes.Status201Created)]
        public async Task<ActionResult<Client>> CreateClient([FromBody] Client client)
        {
            await _repository.CreateClient(client);
            return CreatedAtRoute("GetClient", new { id = client.Id }, client);

        }
        [Authorize(Roles = "Admin, Client")]
        [HttpPut]
        [ProducesResponseType(typeof(Client), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateClient([FromBody] Client client)
        {
            return Ok(await _repository.UpdateClient(client));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}", Name = "DeleteClient")]
        [ProducesResponseType(typeof(Client), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteClient(string id)
        {
            return Ok(await _repository.DeleteClient(id));
        }

        [Authorize(Roles = "Admin")]
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
