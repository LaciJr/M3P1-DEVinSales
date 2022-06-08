using DevInSales.Models;
using DevInSales.Enums;

namespace DevInSales.Seeds
{
    public class ProfileSeed
    {
        public static List<Profile> Seed { get; set; } = new List<Profile>() { new Profile(1, "Usuário",PermissoesEnum.Usuario), new Profile(2, "Gerente",PermissoesEnum.Gerente), new Profile(3, "Administrador", PermissoesEnum.Administrador) };
    }
}
