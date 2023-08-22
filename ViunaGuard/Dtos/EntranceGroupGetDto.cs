namespace ViunaGuard.Dtos;

public class EntranceGroupGetDto
{
    public int Id { get; set; }
    public List<EntranceGetDto> Entrances { get; set; } = new();
}