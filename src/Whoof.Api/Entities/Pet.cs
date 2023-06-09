﻿using Whoof.Api.Enums;

namespace Whoof.Api.Entities;

public class Pet : BaseEntity
{
    public string? Name { get; set; }
    public PetType PetType { get; set; }
    
    public List<PetVaccination>? Vaccinations { get; set; }
}