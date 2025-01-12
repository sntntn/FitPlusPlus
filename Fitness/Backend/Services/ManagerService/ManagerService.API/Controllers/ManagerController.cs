using ManagerService.Common.Entities;
using ManagerService.Common.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ManagerService.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ManagerController : ControllerBase
    {
        private readonly IManagerRepository _managerRepository;

        public ManagerController(IManagerRepository managerRepository)
        {
            _managerRepository = managerRepository ?? throw new ArgumentNullException(nameof(managerRepository));
        }
        /*
        [HttpGet("[action]/{id}", Name = "GetTrainer")]
        [ActionName("Trainer")]
        [ProducesResponseType(typeof(Trainer), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Trainer), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Trainer>> GetTrainer(string id)
        {
            var trainer = await _managerRepository.GetTrainerAsync(id);
            if (trainer == null)
            {
                return NotFound();
            }

            return Ok(trainer);
        }

        [HttpPost("[action]")]
        [ActionName("Trainer")]
        [ProducesResponseType(typeof(Trainer), StatusCodes.Status201Created)]
        public async Task<ActionResult<Trainer>> CreateTrainer([FromBody] Trainer trainer)
        {
            await _managerRepository.CreateTrainerAsync(trainer);
            
            return CreatedAtRoute("GetTrainer", new { id = trainer.Id }, trainer);
        }

        [HttpDelete("[action]/{id}", Name = "DeleteTrainer")]
        [ActionName("Trainer")]
        [ProducesResponseType(typeof(Trainer), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteTrainer(string id)
        {
            return Ok(await _managerRepository.DeleteTrainerAsync(id));

        }
        
        [HttpPut("[action]")]
        [ActionName("Trainer")]
        [ProducesResponseType(typeof(Trainer), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateTrainer([FromBody] Trainer trainer)
        {
            return Ok(await _managerRepository.UpdateTrainerAsync(trainer));
        }
        ------------------------------------------------------------------------------------*/
        [HttpGet("[action]/{id}", Name = "GetManager")]
        [ActionName("Manager")]
        [ProducesResponseType(typeof(Manager), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Manager), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Manager>> GetManager(string id)
        {
            var manager = await _managerRepository.GetManagerAsync(id);
            if (manager == null)
            {
                return NotFound();
            }

            return Ok(manager);
        }

        [HttpPost("[action]")]
        [ActionName("Manager")]
        [ProducesResponseType(typeof(Manager), StatusCodes.Status201Created)]
        public async Task<ActionResult<Manager>> CreateManager([FromBody] Manager manager)
        {
            await _managerRepository.CreateManagerAsync(manager);
            
            return CreatedAtRoute("GetManager", new { id = manager.Id }, manager);
        }

        [HttpDelete("[action]/{id}", Name = "DeleteManager")]
        [ActionName("Manager")]
        [ProducesResponseType(typeof(Manager), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteManager(string id)
        {
            return Ok(await _managerRepository.DeleteManagerAsync(id));

        }
        
        [HttpPut("[action]")]
        [ActionName("Manager")]
        [ProducesResponseType(typeof(Manager), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateManager([FromBody] Manager manager)
        {
            return Ok(await _managerRepository.UpdateManagerAsync(manager));
        }
        
        /*----------------------------------------------------------------------------------------*/
        
        [HttpGet("[action]/{id}", Name = "GetFinance")]
        [ActionName("Finance")]
        [ProducesResponseType(typeof(Finance), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Finance), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Finance>> GetFinance(string id)
        {
            var finance = await _managerRepository.GetFinanceAsync(id);
            if (finance == null)
            {
                return NotFound();
            }

            return Ok(finance);
        }

        [HttpPost("[action]")]
        [ActionName("Finance")]
        [ProducesResponseType(typeof(Finance), StatusCodes.Status201Created)]
        public async Task<ActionResult<Manager>> CreateFinance([FromBody] Finance finance)
        {
            await _managerRepository.CreateFinanceAsync(finance);
            
            return CreatedAtRoute("GetFinance", new { id = finance.Id }, finance);
        }

        [HttpDelete("[action]/{id}", Name = "DeleteFinance")]
        [ActionName("Finance")]    
        [ProducesResponseType(typeof(Finance), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteFinance(string id)
        {
            return Ok(await _managerRepository.DeleteFinanceAsync(id));

        }
        
        [HttpPut("[action]")]
        [ActionName("Finance")]
        [ProducesResponseType(typeof(Finance), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateFinance([FromBody] Finance finance)
        {
            return Ok(await _managerRepository.UpdateFinanceAsync(finance));
        }
        
        
    }
}    