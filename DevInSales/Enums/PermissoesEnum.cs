using System.ComponentModel.DataAnnotations;

namespace DevInSales.Enums
{
    public enum PermissoesEnum
    {
        [Display(Name ="Usuário")]
        Usuario = 1,
        [Display(Name = "Gerente")]
        Gerente,
        [Display(Name = "Administrador")]
        Administrador
    }
}
