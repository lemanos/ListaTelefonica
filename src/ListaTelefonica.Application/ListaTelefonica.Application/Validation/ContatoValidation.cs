using ListaTelefonica.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ListaTelefonica.Application.Validation
{
    public class ContatoValidation
    {
        public static ErrorMessage ValidarDados(ContatoViewModel obj)
        {
            var validacao = new ErrorMessage() { Valido = true };

            if (obj.DataNascimento > DateTime.Now)
                validacao = new ErrorMessage() { Valido = false, Erro = "Data de nascimento é maior que a data atual" };

            if (obj.Idade < 18)
                validacao = new ErrorMessage() { Valido = false, Erro = "O contato tem que ser maior de idade" };

            return validacao;
        }

        public static int CalcularIdade(ContatoViewModel obj)
        {
            var dataNascimento = obj.DataNascimento;
            int idade = DateTime.Now.Year - dataNascimento.Year;
            if (DateTime.Now.DayOfYear < dataNascimento.DayOfYear)
            {
                idade = idade - 1;
            }
            return idade;
        }
    }
}
