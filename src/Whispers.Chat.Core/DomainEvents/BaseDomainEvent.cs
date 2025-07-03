using System;
using MediatR;

namespace Whispers.Chat.Core.DomainEvents;

/// <summary>
/// Base class for domain events
/// </summary>
public abstract class BaseDomainEvent : INotification
{
  public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;
} 