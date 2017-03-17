using App4.Model;
using App4.Model.Resposta;
using App4.Repository;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace App4.ViewModel
{
    public class UsuarioViewModel : INotifyPropertyChanged
    {
        #region Campos

        private string avatarUrl;
        private string nomeUsuario;
        private bool nomeUsuarioEntryIsEnabled;
        private string email;
        private UsuarioModel usuario;

        #endregion

        #region Propriedades

        public string AvatarUrl { get { return usuario.AvatarUrl; } set { avatarUrl = value; OnPropertyChanged("AvatarUrl"); } }
        public string NomeUsuario { get { return usuario.NomeUsuario; } set { nomeUsuario = value; OnPropertyChanged("NomeUsuario"); } }
        public bool NomeUsuarioEntryIsEnabled { get { return nomeUsuarioEntryIsEnabled; } set { nomeUsuarioEntryIsEnabled = value; OnPropertyChanged("NomeUsuarioEntryIsEnabled"); } }
        public string Email { get { return usuario.Email; } set { email = value; OnPropertyChanged("Email"); } }
        
        public UsuarioModel Usuario { get { return usuario; } set { usuario = value; } }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name)); }

        public RespostaStatus TesteConexao()
        {
            var resposta = UsuarioRepository.TesteConexao().Result;
            var status = RespostaStatus.Sucesso;

            switch (resposta.status)
            {
                case 1:
                    status = RespostaStatus.Sucesso;
                    break;
                case 0:
                    status = RespostaStatus.Inexistente;
                    break;
                default:
                    status = RespostaStatus.ErroGenerico;
                    break;
            }
            return status;
        }

        public async Task<RespostaStatus> AtualizarCadastro()
        {
            var resposta2 = await UsuarioRepository.Atualizar();

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

        public async Task<Tuple<RespostaStatus,UsuarioModel>> CadastrarELogar(string email, string senha)
        {
            UsuarioModel usuario = new UsuarioModel();

            string nomeUsuario = ObterNomeUsuario(email);

            var resposta = await UsuarioRepository.Cadastro(email, senha, nomeUsuario);
            var respostaStatus = RespostaStatus.Sucesso;

            switch (resposta.mensagem.ToUpper())
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
            return new Tuple<RespostaStatus, UsuarioModel>(respostaStatus, usuario);
        }

        private static string ObterNomeUsuario(string email)
        {
            return email.Split('@')[0] +
                            DateTime.Now.ToString().Replace(" ", "")
                            .Replace("-", "").Replace(".", "")
                            .Replace(":", "").Replace("/", "")
                            .Replace("A", "").Replace("M", "").Replace("P", "");
        }

        public async Task<bool> Login(string email, string senha)
        {
            this.Usuario = await UsuarioRepository.Login(email, senha);

            return this.Usuario != null ? true : false;
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
