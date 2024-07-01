using System.ComponentModel.DataAnnotations;

namespace AllRiskSolutions_Desafio.Domain.Abstractions;

public abstract class Model(Guid id)
{
    [Key] public Guid Id { get; set; } = id;
}