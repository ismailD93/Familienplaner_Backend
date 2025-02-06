﻿using KalenderAppBackend.Models;
using Microsoft.AspNetCore.Identity;

namespace KalenderAppBackend.Interfaces;

public interface IUserEventRepo
{
    Task<UserEvent> CreateAsync(AppUser user, Event @event);
}
