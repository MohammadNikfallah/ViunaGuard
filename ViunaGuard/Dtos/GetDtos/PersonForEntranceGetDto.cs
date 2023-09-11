namespace ViunaGuard.Dtos;

public class PersonForEntranceGetDto
{
    public string Id { get; set; } = null!;
    public PersonAdditionalInfo? PersonAdditionalInfo { get; set; }
    public string NationalId { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? CellPhoneNumber { get; set; }
    public string? FathersName { get; set; }
    public Gender? Gender { get; set; }
    public City? BirthPlaceCity { get; set; }
    public DateOnly? BirthDay { get; set; }
    public City? CityOfResidence { get; set; }
    public EducationalDegree? EducationalDegree { get; set; }
    public MilitaryServiceStatus? MilitaryServiceStatus { get; set; }
    public Nationality? Nationality { get; set; }
    public Religion? Religion { get; set; }
    public MaritalStatus? MaritalStatus { get; set; }
}