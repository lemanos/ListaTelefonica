using AutoMapper;
using ListaTelefonica.Application.ViewModels;
using ListaTelefonica.Domain.Entities;
using ListaTelefonica.Infrastructure.Persistense.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListaTelefonica.Application.Services
{
    public class ContatoService : IContatoService
    {

        private readonly IContatoRepository _contatoRepository;
        private readonly ILogger<IContatoService> _logger;
        private readonly IMapper _mapper;

        public ContatoService(ILogger<IContatoService> logger, IMapper mapper, IContatoRepository cadastroTesteRepository)
        {
            _contatoRepository = cadastroTesteRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ContatoViewModel>> GetAll()
        {
            return _mapper.Map<IEnumerable<ContatoViewModel>>(_contatoRepository.AsQueryable().Where(p => p.IsAtivo == true));
        }

        public async Task<ContatoViewModel> RetornaPorId(int id)
        {
            var retorno = _contatoRepository.AsQueryable().Where(p => p.Id == id).FirstOrDefault();
            return _mapper.Map<ContatoViewModel>(retorno);
        }

        public async Task<ContatoViewModel> Incluir(ContatoViewModel obj)
        {
            var objRetorno = new ContatoViewModel();

            obj.Idade = Validation.ContatoValidation.CalcularIdade(obj);
            var validacaoDados = Validation.ContatoValidation.ValidarDados(obj);

            if (!validacaoDados.Valido)
                objRetorno.MsgErro = "Erro ao incluir dados! " + validacaoDados.Erro;
            else
            {
                try
                {
                    var objInc = _mapper.Map<Contato>(obj);
                    var testeInc = await _contatoRepository.Insert(objInc);
                    objRetorno = _mapper.Map<ContatoViewModel>(objInc);
                }
                catch (Exception ex)
                {
                    objRetorno.MsgErro = "Erro ao incluir dados! " + ex.Message;
                }
            }

            return objRetorno;
        }

        public async Task<ContatoViewModel> Excluir(int id)
        {
            var retorno = new ContatoViewModel();

            try
            {
                var obj = _contatoRepository.SelectById(id).Result;

                if (obj == null)
                {
                    retorno.Valido = false;
                    retorno.MsgErro = "Contato não encontrado!";
                }
                else
                {
                    _contatoRepository.Delete(obj);
                    retorno.Valido = true;
                    await _contatoRepository.Commit();
                }
            }
            catch (Exception ex)
            {
                retorno.Valido = false;
                retorno.MsgErro = "Erro ao excluir Contato! " + ex.Message;

            }

            return retorno;
        }

        public async Task<ContatoViewModel> Alterar(ContatoViewModel obj)
        {
            try
            {
                var objAlt = _mapper.Map<Contato>(obj);
                await _contatoRepository.Update(objAlt);
                await _contatoRepository.Commit();
                obj.Valido = true;
            }
            catch (Exception ex)
            {
                obj.Valido = false;
                obj.MsgErro = "Erro ao incluir dados! " + ex.Message;
            }

            return obj;
        }
    }

}
