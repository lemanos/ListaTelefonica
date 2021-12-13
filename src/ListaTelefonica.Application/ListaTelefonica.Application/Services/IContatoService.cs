using ListaTelefonica.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ListaTelefonica.Application.Services
{
    public  interface IContatoService
    {
        Task<IEnumerable<ContatoViewModel>> GetAll();
        Task<ContatoViewModel> RetornaPorId(int id);
        Task<ContatoViewModel> Incluir(ContatoViewModel obj);
        Task<ContatoViewModel> Alterar(ContatoViewModel obj);
        Task<ContatoViewModel> Excluir(int id);
    }
}
