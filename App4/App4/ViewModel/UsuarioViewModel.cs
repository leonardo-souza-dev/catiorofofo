using App4.Model;
using App4.Model.Resposta;
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

        public async Task<RespostaStatus> AtualizarCadastro(Usuario usuario)
        {
            var resposta2 = await UsuarioRepository.Atualizar(usuario);
            var status = RespostaStatus.Sucesso;

            switch (resposta2.mensagem.ToUpper())
            {
                case "SUCESSO":
                    status = RespostaStatus.Sucesso;
                    break;
                case "INEXISTENTE":
                    status = RespostaStatus.Inexistente;
                    break;
                case "JAEXISTE":
                    status = RespostaStatus.JaExiste;
                    break;
                case "ERROGENERICO":
                default:
                    status = RespostaStatus.ErroGenerico;
                    break;
            }

            return status;
        }

        public async Task<Tuple<RespostaStatus,Usuario>> CadastrarELogar(string email, string senha)
        {
            Usuario usuario = new Usuario();

            var nomeUsuario = email.Split('@')[0] +
                DateTime.Now.ToString().Replace(" ", "")
                .Replace("-", "").Replace(".", "")
                .Replace(":", "").Replace("/", "")
                .Replace("A", "").Replace("M", "").Replace("P", "");
            //TODO:jogar essa regra de criacao automatica de nome de usuario para a web api

            var resposta = await UsuarioRepository.Cadastro(email, senha, nomeUsuario);
            var respostaStatus = RespostaStatus.Sucesso;

            switch(resposta.mensagem.ToUpper())
            {
                case "SUCESSO":
                    usuario.NomeArquivoAvatar = resposta.usuario.nomeArquivoAvatar;
                    usuario.Email = resposta.usuario.email;
                    usuario.NomeUsuario = resposta.usuario.nomeUsuario;
                    usuario.UsuarioId = resposta.usuario.usuarioId;
                    break;
                case "JAEXISTE":
                    respostaStatus = RespostaStatus.JaExiste;
                    usuario = null;
                    break;
            }
            return new Tuple<RespostaStatus, Usuario>(respostaStatus, usuario);
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
                usuario.NomeArquivoAvatar = resposta.usuario.nomeArquivoAvatar;
                usuario.NomeUsuario = resposta.usuario.nomeUsuario;

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
