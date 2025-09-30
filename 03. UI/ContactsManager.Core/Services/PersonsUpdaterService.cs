using Entities;
using ServiceContracts.DTO;
using ServiceContracts;
using RepositoryContracts;
using Exceptions;

namespace Services
{
 public class PersonsUpdaterService : IPersonsUpdaterService
 {
  //private field
  private readonly IPersonsRepository _personsRepository;

  //constructor
  public PersonsUpdaterService(IPersonsRepository personsRepository)
  {
   _personsRepository = personsRepository;
  }


  public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
  {
   if (personUpdateRequest == null)
    throw new ArgumentNullException(nameof(personUpdateRequest));
   
   //get matching person object to update
   Person? matchingPerson = await _personsRepository.GetPersonByPersonID(personUpdateRequest.PersonID);
   if (matchingPerson == null)
   {
    throw new InvalidPersonIDException("Given person id doesn't exist");
   }

   //update all details
   matchingPerson.PersonName = personUpdateRequest.PersonName;
   matchingPerson.Email = personUpdateRequest.Email;
   matchingPerson.DateOfBirth = personUpdateRequest.DateOfBirth;
   matchingPerson.Gender = personUpdateRequest.Gender.ToString();
   matchingPerson.CountryID = personUpdateRequest.CountryID;
   matchingPerson.Address = personUpdateRequest.Address;
   matchingPerson.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;

   await _personsRepository.UpdatePerson(matchingPerson); //UPDATE

   return matchingPerson.ToPersonResponse();
  }
 }
}
