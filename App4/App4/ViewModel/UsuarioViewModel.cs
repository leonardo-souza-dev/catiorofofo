using App4.Model;
using App4.Repository;
using System;
using System.Threading.Tasks;

namespace App4.ViewModel
{
    public class UsuarioViewModel
    {
        //public Usuario Usuario { get; set; } = new Usuario();

        public bool LoginSucesso;

        public UsuarioViewModel(ConfiguracaoApp config)
        {
            UsuarioRepository.SetarConfiguracao(config);
        }

        public async Task<Usuario> CadastrarELogar(string email, string senha)
        {
            Usuario usuario = new Usuario();
            usuario = await UsuarioRepository.Cadastro(email, senha);

            return usuario;
        }

        public async Task<Tuple<RespostaStatus,Usuario>> Login(string email, string senha)
        {
            var resposta = await UsuarioRepository.Login(email, senha);

            if (resposta.mensagem.ToUpper() == "INEXISTENTE")
            {
                return new Tuple<RespostaStatus,Usuario>(RespostaStatus.Inexistente, null);
            }

            if (resposta.mensagem.ToUpper() == "SUCESSO")
            {
                Usuario usuario = new Usuario();
                usuario.UsuarioId = resposta.usuario.usuarioId;
                usuario.Email = resposta.usuario.email;

                return new Tuple<RespostaStatus, Usuario>(RespostaStatus.Sucesso, usuario);
            }

            return null;
        }

        public async Task<RespostaStatus> EsqueciSenha(string email)
        {
            var resposta = await UsuarioRepository.EsqueciSenha(email);

            if (resposta.sucesso == false && resposta.mensagem == "nao foi encontrado usuario com esse email")
            {
                return RespostaStatus.Inexistente;
            }

            if (resposta.sucesso)
            {
                return RespostaStatus.Sucesso;
            }

            return RespostaStatus.ErroGenerico;
        }
    }
}
