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

        public async Task<Usuario> Cadastro(string email, string senha)
        {
            Usuario usuario = new Usuario();
            usuario = await UsuarioRepository.Cadastro(email, senha);

            return usuario;
        }

        public async Task<Usuario> Login(string senha)
        {
            Usuario usuario = new Usuario();
            usuario = await UsuarioRepository.Login(senha);

            return usuario;
        }

        public async Task<RespostaStatus> EsqueciSenha(string email)
        {
            RespostaEsqueciSenha asd = await UsuarioRepository.EsqueciSenha(email);

            if (asd.sucesso==false && asd.mensagem == "nao foi encontrado usuario com esse email")
            {
                return RespostaStatus.EmailInexistente;
            }

            if (asd.sucesso)
            {
                return RespostaStatus.Sucesso;
            }

            return RespostaStatus.ErroGenerico;
        }
    }
}
