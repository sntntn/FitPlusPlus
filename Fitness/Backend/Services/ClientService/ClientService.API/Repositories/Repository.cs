using Amazon.Runtime.Internal;
using ClientService.API.Data;
using ClientService.API.Entities;
using MongoDB.Driver;

namespace ClientService.API.Repositories
{
    public class Repository : IRepository
    {
        private readonly IContext _context;
        public Repository(IContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Client>> GetClients()
        {
            return await _context.Clients.Find(p => true).ToListAsync();
        }
        public async Task<Client> GetClientById(string id)
        {
            return await _context.Clients.Find(p => p.Id == id).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<Client>> GetClientsByName(string name)
        {
            return await _context.Clients.Find(p => p.Name == name).ToListAsync();
        }
        public async Task<Client> GetClientByEmail(string email)
        {
            return await _context.Clients.Find(p => p.Email == email).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<Client>> GetClientsBySurname(string surname)
        {
            return await _context.Clients.Find(p => p.Surname == surname).ToListAsync();
        }
        public async Task CreateClient(Client client)
        {
            await _context.Clients.InsertOneAsync(client);
            var clientSchedule = new ClientSchedule(client.Id);
            await _context.ClientSchedules.InsertOneAsync(clientSchedule);
        }
        public async Task<bool> UpdateClient(Client client)
        {
            var result = await _context.Clients.ReplaceOneAsync(p => p.Id == client.Id, client);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }
        public async Task<bool> DeleteClient(string id)
        {
            var resultClient = await _context.Clients.DeleteOneAsync(p => p.Id == id);
            var resultSchedule = await _context.ClientSchedules.DeleteOneAsync(p => p.ClientId == id);

            return resultClient.IsAcknowledged && resultClient.DeletedCount > 0
                && resultSchedule.IsAcknowledged && resultSchedule.DeletedCount>0;
        }
        public async Task DeleteAllClients()
        {
            await _context.Clients.DeleteManyAsync(p => true);
        }

        public async Task<ClientSchedule> GetClientScheduleByClientId(string id)
        {
            return await _context.ClientSchedules.Find(s => s.ClientId == id).FirstOrDefaultAsync();
        }
        public async Task<WeeklySchedule> GetClientWeekSchedule(string clientId, int weekId)
        {
            var clientSchedule = await GetClientScheduleByClientId(clientId);
            return clientSchedule?.WeeklySchedules.FirstOrDefault(ws => ws.WeekId == weekId);
        }

        public async Task<IEnumerable<string>> GetTrainerIdsFromClientSchedule(string clientId)
        {
            var clientSchedule = await GetClientScheduleByClientId(clientId);

            if (clientSchedule == null)
            {
                return Enumerable.Empty<string>();
            }

            // Assuming `clientSchedule` contains a collection of training slots
            var trainerIds = clientSchedule.WeeklySchedules
                .SelectMany(ws => ws.DailySchedules.Values)
                .SelectMany(dayList => dayList)
                .Select(si => si.TrainerId)
                .Where(id => !string.IsNullOrEmpty(id))
                .Distinct();

            return trainerIds;
        }

        public async Task<bool> UpdateClientSchedule(ClientSchedule clientSchedule)
        {
            var result = await _context.ClientSchedules.ReplaceOneAsync(cs => cs.ClientId == clientSchedule.ClientId, clientSchedule, new ReplaceOptions { IsUpsert = true });
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> BookTraining(BookTrainingInformation bti)
        {
            var clientSchedule = await GetClientScheduleByClientId(bti.ClientId);
            if (clientSchedule == null)
            {
                return false;
            }
            
            var weeklySchedule = clientSchedule.WeeklySchedules.FirstOrDefault(ws => ws.WeekId == bti.WeekId);
            if (weeklySchedule == null)
            {
                return false;
            }

            if (!weeklySchedule.DailySchedules.TryGetValue(bti.DayName, out var dailySchedule))
                return false;
            
            int numberOfCells = (int)bti.Duration.TotalMinutes / 15;
            

            var startSlotIndex = dailySchedule.FindIndex(slot => slot.StartHour == bti.StartHour && slot.StartMinute == bti.StartMinute);

            if (startSlotIndex == -1 || startSlotIndex + numberOfCells > dailySchedule.Count)
                return false;

            for (int i = startSlotIndex; i < startSlotIndex + numberOfCells; i++)
            {
                if (bti.IsBooking)
                {
                    if (!dailySchedule[i].IsAvailable)
                        return false;

                    dailySchedule[i].IsAvailable = false;
                    dailySchedule[i].TrainerId = bti.TrainerId;
                    dailySchedule[i].TrainingType = bti.TrainingType;
                    dailySchedule[i].TrainerName = bti.TrainerName;
                    dailySchedule[i].TrainingStartHour = bti.StartHour;
                    dailySchedule[i].TrainingDuration = bti.Duration;
                    dailySchedule[i].TrainingStartMinute = bti.StartMinute;
                }
                else
                {
                    dailySchedule[i].IsAvailable = true;
                    dailySchedule[i].TrainerId = "";
                    dailySchedule[i].TrainingType = "";
                    dailySchedule[i].TrainingStartHour = -1;
                }
            }

            await UpdateClientSchedule(clientSchedule);
            
            return true;
        }
        public async Task CancelledTrainingByTrainer(CancelTrainingInformation cti)
        {
            //obavlja se validacija na frontu
            var clientSchedule = await GetClientScheduleByClientId(cti.ClientId);
            var weeklySchedule = clientSchedule.WeeklySchedules.FirstOrDefault(ws => ws.WeekId == cti.WeekId);
            weeklySchedule.DailySchedules.TryGetValue(cti.DayName, out var dailySchedule);
            
            int numberOfCells = (int)cti.Duration.TotalMinutes / 15;
            var startSlotIndex = dailySchedule.FindIndex(slot => slot.StartHour == cti.StartHour && slot.StartMinute == cti.StartMinute);
           
            for (int i = startSlotIndex; i < startSlotIndex + numberOfCells; i++)
            {
                dailySchedule[i].IsAvailable = true;
                dailySchedule[i].TrainerId = "";
                dailySchedule[i].TrainingType = "";
                dailySchedule[i].TrainingStartHour = -1;
            }

            await UpdateClientSchedule(clientSchedule);
        }

    }
}
