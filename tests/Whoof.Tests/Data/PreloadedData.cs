using Whoof.Domain.Entities;
using Whoof.Domain.Enums;
using Whoof.Infrastructure.Persistence;

namespace Whoof.Tests.Data;

public static class PreloadedData
{
    public static void Load(AppDbContext dbContext)
    {
        var vaccines = new List<Vaccine>
        {
            new()
            {
                Name = "Rabies",
                Description = "Protects against rabies virus",
                PetType = PetType.Dog,
                Duration = 365
            },
            new()
            {
                Name = "Distemper",
                Description = "Protects against distemper virus",
                PetType = PetType.Dog,
                Duration = 365
            },
            new()
            {
                Name = "Parvovirus",
                Description = "Protects against parvovirus",
                PetType = PetType.Dog,
                Duration = 365
            },
            new()
            {
                Name = "Bordetella",
                Description = "Protects against kennel cough",
                PetType = PetType.Dog,
                Duration = 180
            },
            new()
            {
                Name = "Leptospirosis",
                Description = "Protects against leptospirosis bacteria",
                PetType = PetType.Dog,
                Duration = 365
            },
            new()
            {
                Name = "Feline Leukemia",
                Description = "Protects against feline leukemia virus",
                PetType = PetType.Cat,
                Duration = 365
            },
            new()
            {
                Name = "Feline Calicivirus",
                Description = "Protects against feline calicivirus",
                PetType = PetType.Cat,
                Duration = 365
            },
            new()
            {
                Name = "Feline Herpesvirus",
                Description = "Protects against feline herpesvirus",
                PetType = PetType.Cat,
                Duration = 365
            },
            new()
            {
                Name = "Canine Influenza",
                Description = "Protects against canine influenza virus",
                PetType = PetType.Dog,
                Duration = 365
            },
            new()
            {
                Name = "Canine Parainfluenza",
                Description = "Protects against canine parainfluenza virus",
                PetType = PetType.Dog,
                Duration = 365
            },
            new()
            {
                Name = "Capybarism",
                Description = "Random vaccine for capybaras",
                PetType = PetType.Capybara,
                Duration = 720
            },
        };

        dbContext.Vaccines.AddRange(vaccines);

        var petNames = new List<string>
        {
            "Bella", "Max", "Charlie", "Lucy", "Cooper", "Luna", "Rocky",
            "Stella", "Buddy", "Daisy", "Bailey", "Sadie", "Jack", "Molly",
            "Milo", "Rosie", "Tucker", "Lola", "Oliver", "Ruby", "Sudo",
        };
        var petTypes = Enum.GetValues(typeof(PetType)).Cast<PetType>().ToList();
        var random = new Random();

        var pets = new List<Pet>();
        var petVaccinations = new List<PetVaccination>();

        for (var i = 0; i < 50; i++)
        {
            var petName = petNames[random.Next(petNames.Count)];
            
            var petType = PetType.Unspecified;
            while (petType == PetType.Unspecified)
                petType = petTypes[random.Next(petTypes.Count)];

            var pet = new Pet
            {
                Name = petName,
                PetType = petType,
                Vaccinations = new List<PetVaccination>(),
                OwnerId = "test@whoof.api"
            };

            var numVaccinations = random.Next(1, 5);
            for (var j = 0; j < numVaccinations; j++)
            {
                var vaccine = vaccines[random.Next(vaccines.Count)];
                var appliedAt = DateTimeOffset.UtcNow.AddDays(-random.Next(30, 365));

                var petVaccination = new PetVaccination
                {
                    PetId = pet.Id,
                    VaccineId = vaccine.Id,
                    AppliedAt = appliedAt,
                    Pet = pet,
                    Vaccine = vaccine
                };

                pet.Vaccinations.Add(petVaccination);
                petVaccinations.Add(petVaccination);
            }

            pets.Add(pet);
        }

        dbContext.Pets.AddRange(pets);
        dbContext.PetVaccinations.AddRange(petVaccinations);

        dbContext.SaveChanges();
    }
}