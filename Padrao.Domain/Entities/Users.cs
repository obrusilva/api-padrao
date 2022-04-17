namespace Padrao.Domain.Entities
{
    public class Users
    {
        public virtual int Id { get; set; }
        public virtual string UserName { get; set; }
        public virtual string Email { get; set; }
        public virtual  string PassWord { get; set; }
        public virtual string Name { get; set; }
    }
}
