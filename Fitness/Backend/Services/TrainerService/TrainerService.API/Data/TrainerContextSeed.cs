using MongoDB.Bson;
using MongoDB.Driver;
using TrainerService.API.Entities;

namespace TrainerService.API.Data
{
    public class TrainerContextSeed
    {
        public static void SeedData(IMongoCollection<Trainer> trainerCollection)
        {
            var existTrainers = trainerCollection.Find(p => true).Any();
            if (!existTrainers)
            {
                trainerCollection.InsertManyAsync(GetPreconfiguredTrainers());
            }
        }

        public static IEnumerable<Trainer> GetPreconfiguredTrainers()
        {
            return new List<Trainer>()
            {
                new Trainer()
                {
                    Id=ObjectId.GenerateNewId().ToString(),
                    FullName="John Doe",
                    ContactEmail="johndoe@gmail.com",
                    ContactPhone="555-1234",
                    Bio="Experienced personal trainer specializing in strength training and bodybuilding.",
                    TrainingTypes=new List<TrainingType>()
                    {
                        new TrainingType()
                        {
                            Id=ObjectId.GenerateNewId().ToString(),
                            Name="Strength Training",
                            Duration=new TimeSpan(1,0,0),
                            Difficulty="Intermediate"
                        },
                        new TrainingType()
                        {
                            Id=ObjectId.GenerateNewId().ToString(),
                            Name="Bodybuilding",
                            Duration=new TimeSpan(1,30,0),
                            Difficulty="Advanced"
                        }
                    }
                },
                new Trainer()
                {
                    Id=ObjectId.GenerateNewId().ToString(),
                    FullName="Jane Smith",
                    ContactEmail="janesmith@gmail.com",
                    ContactPhone="555-5678",
                    Bio="Certified yoga instructor with over 10 years of experience.",
                    TrainingTypes=new List<TrainingType>()
                    {
                        new TrainingType()
                        {
                            Id=ObjectId.GenerateNewId().ToString(),
                            Name="Hatha Yoga",
                            Duration=new TimeSpan(1,0,0),
                            Difficulty="Beginner"
                        },
                        new TrainingType()
                        {
                            Id=ObjectId.GenerateNewId().ToString(),
                            Name="Vinyasa Yoga",
                            Duration=new TimeSpan(1,15,0),
                            Difficulty="Intermediate"
                        }
                    }
                },
                new Trainer()
                {
                    Id=ObjectId.GenerateNewId().ToString(),
                    FullName="Mike Johnson",
                    ContactEmail="mikejohnson@gmail.com",
                    ContactPhone="555-9101",
                    Bio="Proffesional trainer focusing on HIIT and endurance training.",
                    TrainingTypes=new List<TrainingType>()
                    {
                        new TrainingType()
                        {
                            Id=ObjectId.GenerateNewId().ToString(),
                            Name="HIIT",
                            Duration=new TimeSpan(0,45,0),
                            Difficulty="Advanced"
                        },
                        new TrainingType()
                        {
                            Id=ObjectId.GenerateNewId().ToString(),
                            Name="Endurance Training",
                            Duration=new TimeSpan(1,30,0),
                            Difficulty="Intermediate"
                        }
                    }
                }
            };
        }
    }
}
