using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
 public class PersonsGetterServiceWithFewExcelFields : IPersonsGetterService
 {
  private readonly PersonsGetterService _personGetterService;

  public PersonsGetterServiceWithFewExcelFields(PersonsGetterService personsGetterService)
  {
   _personGetterService = personsGetterService;
  }

  public async Task<List<PersonResponse>> GetAllPersons()
  {
   return await _personGetterService.GetAllPersons();
  }

  public async Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString)
  {
   return await _personGetterService.GetFilteredPersons(searchBy, searchString);
  }

  public async Task<PersonResponse?> GetPersonByPersonID(Guid? personID)
  {
   return await _personGetterService.GetPersonByPersonID(personID);
  }
 }
}
