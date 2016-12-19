using App4.Model;
using App4.Repository;
using System.Threading.Tasks;

namespace App4.ViewModel
{
    public class UsuarioViewModel
    {
        //public Usuario Usuario { get; set; } = new Usuario();

        public bool LoginSucesso;

        public UsuarioViewModel()
        {

        }

        public async Task<Usuario> Login(string senha)
        {
            Usuario usuario = new Usuario();

            usuario = await UsuarioRepository.Login(senha);

            //Usuario.AvatarHash = respostaLogin.avatar;
            //Usuario.Email = respostaLogin.email;

            return usuario;
        }

        public async Task<bool> EsqueciSenha(string email)
        {
            RespostaEsqueciSenha asd = await UsuarioRepository.EsqueciSenha(email);

            if (asd.sucesso)
            {
                return true;
            }

            return false;
        }
    }
}
