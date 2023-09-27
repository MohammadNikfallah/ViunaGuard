namespace ViunaGuard.Dtos
{
    public class EntrancePermissionGetDto
    {
        public int Id { get; set; }
        public Person Person { get; set; } = null!;
        public DateTime StartValidityTime { get; set; }
        public DateTime EndValidityTime { get; set; }
        public Car? Car { get; set; }
        public bool PermissionGranted { get; set; }
        public OrganizationPlace? OrganizationPlace { get; set; }
        public List<SignedPermissionGetDto> Signatures { get; set; } = new();
    }
}
