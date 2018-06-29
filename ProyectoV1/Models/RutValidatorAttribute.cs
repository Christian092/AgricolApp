using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoV1.Models
{
    public class RutValidatorAttribute: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string rutString = Convert.ToString(value);
            string[] rutSeparado = rutString.Split('-');
            if (rutSeparado.Length != 2)
            {
                return new ValidationResult("Rut no válido");
            }
            else
            {
                try
                {
                    int rut = Convert.ToInt32(rutSeparado[0].Replace(".", string.Empty));
                    string digito = digitoVerificador(rut);
                    if (digito.Equals(rutSeparado[1].ToUpper()))
                    {
                        return null;
                    }
                    else
                    {
                        return new ValidationResult("Rut no válido");
                    }
                }
                catch (Exception ex)
                {
                    return new ValidationResult("Rut no válido");
                }


            }
        }
        private string digitoVerificador(int rut)
        {
            int Digito;
            int Contador;
            int Multiplo;
            int Acumulador;
            string RutDigito;

            Contador = 2;
            Acumulador = 0;

            while (rut != 0)
            {
                Multiplo = (rut % 10) * Contador;
                Acumulador = Acumulador + Multiplo;
                rut = rut / 10;
                Contador = Contador + 1;
                if (Contador == 8)
                {
                    Contador = 2;
                }

            }

            Digito = 11 - (Acumulador % 11);
            RutDigito = Digito.ToString().Trim();
            if (Digito == 10)
            {
                RutDigito = "K";
            }
            if (Digito == 11)
            {
                RutDigito = "0";
            }
            return (RutDigito);
        }
    }
}