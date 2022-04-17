using System.ComponentModel.DataAnnotations;

namespace Padrao.Domain.Request
{
    public class NewUserRequest
    {
        [Required(ErrorMessage  = "O campo {0} é Obrigatório")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "O campo {0} é Obrigatório")]
        public string Email { get; set; }
        [Required(ErrorMessage = "O campo {0} é Obrigatório")]
        public string PassWord { get; set; }
        [Required(ErrorMessage = "O campo {0} é Obrigatório")]
        public string Name { get; set; }
    }
}
