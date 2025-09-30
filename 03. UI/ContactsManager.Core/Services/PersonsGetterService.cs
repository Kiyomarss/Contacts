using Entities;
using ServiceContracts.DTO;
using ServiceContracts;
using RepositoryContracts;
using Microsoft.Extensions.Logging;
using Serilog;
using SerilogTimings;

namespace Services
{
 public class PersonsGetterService : IPersonsGetterService
 {
  //private field
  private readonly IPersonsRepository _personsRepository;
  private readonly ILogger<PersonsGetterService> _logger;
  private readonly IDiagnosticContext _diagnosticContext;

  //constructor
  public PersonsGetterService(IPersonsRepository personsRepository, ILogger<PersonsGetterService> logger, IDiagnosticContext diagnosticContext)
  {
   _personsRepository = personsRepository;
   _logger = logger;
   _diagnosticContext = diagnosticContext;
  }

  public virtual async Task<List<PersonResponse>> GetAllPersons()
  {
   _logger.LogInformation("GetAllPersons of PersonsService");

   var persons = await _personsRepository.GetAllPersons();

   return persons
     .Select(temp => temp.ToPersonResponse()).ToList();
  }


  public virtual async Task<PersonResponse?> GetPersonByPersonID(Guid? personID)
  {
   if (personID == null)
    return null;

   Person? person = await _personsRepository.GetPersonByPersonID(personID.Value);

   if (person == null)
    return null;

   return person.ToPersonResponse();
  }


  public virtual async Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString)
  {
   _logger.LogInformation("GetFilteredPersons of PersonsService");

   List<Person> persons;

   using (Operation.Time("Time for Filtered Persons from Database"))
   {
    persons = searchBy switch
    {
     nameof(PersonResponse.PersonName) =>
      await _personsRepository.GetFilteredPersons(temp =>
      temp.PersonName.Contains(searchString)),

     nameof(PersonResponse.Email) =>
      await _personsRepository.GetFilteredPersons(temp =>
      temp.Email.Contains(searchString)),

     nameof(PersonResponse.DateOfBirth) =>
      await _personsRepository.GetFilteredPersons(temp =>
      temp.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(searchString)),


     nameof(PersonResponse.Gender) =>
      await _personsRepository.GetFilteredPersons(temp =>
      temp.Gender.Contains(searchString)),

     nameof(PersonResponse.CountryID) =>
      await _personsRepository.GetFilteredPersons(temp =>
      temp.Country.CountryName.Contains(searchString)),

     nameof(PersonResponse.Address) =>
     await _personsRepository.GetFilteredPersons(temp =>
     temp.Address.Contains(searchString)),

     _ => await _personsRepository.GetAllPersons()
    };
   } //end of "using block" of serilog timings

   _diagnosticContext.Set("Persons", persons);

   return persons.Select(temp => temp.ToPersonResponse()).ToList();
  }
 }
}
