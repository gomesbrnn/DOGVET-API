using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.DTO.Animal;
using App.DTO.Atendimento;
using App.DTO.Autenticacao;
using App.DTO.Clinica;
using App.DTO.Tutor;
using App.DTO.Veterinario;
using App.Models;
using AutoMapper;

namespace App.Mappings
{
    public class MappingsProfile : Profile
    {
        public MappingsProfile()
        {
            CreateMap<Clinica, CriarClinicaDTO>().ReverseMap();
            CreateMap<Clinica, AtualizarClinicaDTO>().ReverseMap();

            CreateMap<Tutor, CriarTutorDTO>().ReverseMap();
            CreateMap<Tutor, AtualizarTutorDTO>().ReverseMap();

            CreateMap<Animal, CriarAnimalDTO>().ReverseMap();
            CreateMap<Animal, AtualizarAnimalDTO>().ReverseMap();

            CreateMap<Veterinario, CriarVeterinarioDTO>().ReverseMap();
            CreateMap<Veterinario, AtualizarVeterinarioDTO>().ReverseMap();

            CreateMap<Atendimento, CriarAtendimentoDTO>().ReverseMap();
            CreateMap<Atendimento, AtualizarAtendimentoDTO>().ReverseMap();

            CreateMap<Autenticacao, CriarAutenticacaoDTO>().ReverseMap();
            CreateMap<Autenticacao, AtualizarAutenticacaoDTO>().ReverseMap();
        }
    }
}