using Entities;
using ServiceContracts.DTO;
using ServiceContracts;
using RepositoryContracts;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Services
{
 public class PersonsAdderService : IPersonsAdderService
 {
  //private field
  private readonly IPersonsRepository _personsRepository;

  //constructor
  public PersonsAdderService(IPersonsRepository personsRepository, ILogger<PersonsGetterService> logger, IDiagnosticContext diagnosticContext)
  {
   _personsRepository = personsRepository;
  }


  public async Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest)
  {
   //check if PersonAddRequest is not null
   if (personAddRequest == null)
   {
    throw new ArgumentNullException(nameof(personAddRequest));
   }
   
   //convert personAddRequest into Person type
   Person person = personAddRequest.ToPerson();

   //generate PersonID
   person.PersonID = Guid.NewGuid();

   //add person object to persons list
   await _personsRepository.AddPerson(person);

   //convert the Person object into PersonResponse type
   return person.ToPersonResponse();
  }

 }
}
