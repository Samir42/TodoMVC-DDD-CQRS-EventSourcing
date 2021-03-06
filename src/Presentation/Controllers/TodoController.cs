﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NServiceBus;
using Infrastructure.Responses;
using Infrastructure.Extensions;

namespace Example.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TodoController : ControllerBase
    {

        private readonly ILogger<TodoController> _logger;
        private readonly IMessageSession _session;

        public TodoController(ILogger<TodoController> logger, IMessageSession session)
        {
            _logger = logger;
            _session = session;
        }

        [HttpGet]
        public Task<Paged<Todo.Models.TodoResponse>> All()
        {
            return _session.Request<Todo.Queries.AllTodos, Todo.Models.TodoResponse>(new Todo.Queries.AllTodos
            {
            });
        }
        [HttpGet("/active")]
        public Task<Paged<Todo.Models.TodoResponse>> Active()
        {
            return _session.Request<Todo.Queries.ActiveTodos, Todo.Models.TodoResponse>(new Todo.Queries.ActiveTodos
            {
            });
        }
        [HttpGet("/complete")]
        public Task<Paged<Todo.Models.TodoResponse>> Complete()
        {
            return _session.Request<Todo.Queries.CompleteTodos, Todo.Models.TodoResponse>(new Todo.Queries.CompleteTodos
            {
            });
        }

        [HttpPost]
        public Task Add(DTOs.Todo model)
        {
            return _session.CommandToDomain(new Todo.Commands.Add
            {
                TodoId = model.TodoId,
                Message = model.Message
            });
        }
        [HttpPost("/edit")]
        public Task Edit(DTOs.Todo model)
        {
            return _session.CommandToDomain(new Todo.Commands.Edit
            {
                TodoId = model.TodoId,
                Message = model.Message
            });
        }
        [HttpDelete]
        public Task Remove(DTOs.Todo model)
        {
            return _session.CommandToDomain(new Todo.Commands.Remove
            {
                TodoId = model.TodoId,
            });
        }
        [HttpPost("/mark_complete")]
        public Task Complete(DTOs.Todo model)
        {
            return _session.CommandToDomain(new Todo.Commands.MarkComplete
            {
                TodoId = model.TodoId,
            });
        }
        [HttpPost("/mark_active")]
        public Task Active(DTOs.Todo model)
        {
            return _session.CommandToDomain(new Todo.Commands.MarkActive
            {
                TodoId = model.TodoId,
            });
        }
    }
}
