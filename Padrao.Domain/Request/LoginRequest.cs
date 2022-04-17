using System.ComponentModel.DataAnnotations;
namespace Padrao.Domain.Request
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "O campo {0} é Obrigatório")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "O campo {0} é Obrigatório")]
        public string PassWord { get; set; }
     
    }
}
