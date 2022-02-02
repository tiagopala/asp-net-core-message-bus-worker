﻿namespace AspNetCoreMessageBusWorker.Domain.Models;

public class Customer
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Cpf { get; set; }
}