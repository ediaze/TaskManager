﻿using System.Net;
using TaskManager.Application.Dtos;
using TaskManager.Application.DTOs;
using TaskManager.Application.Exceptions;
using TaskManager.Application.Interfaces;
using TaskManager.Application.Mapping;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.Interfaces;

namespace TaskManager.Application.Services
{
    public class TaskService(ITaskItemRepository repository): ITaskService
    {
        private readonly ITaskItemRepository _repository = repository;

        public async Task<TaskItemDto?> GetByIdAsync(Guid id)
        {
            var taskItem = await _repository.GetByIdAsync(id) ?? throw new ApiException(HttpStatusCode.NotFound);
            return TaskMapper.ConvertToDto(taskItem);
        }

        public async Task<IList<TaskItemDto>?> GetAllAsync()
        {
            var taskItems = await _repository.GetAllAsync() ?? throw new ApiException(HttpStatusCode.NotFound);
            var items = taskItems.Select(TaskMapper.ConvertToDto).ToList();
            return (IList<TaskItemDto>)items;
        }

        public async Task<TaskItemDto?> AddAsync(TaskItemCreationDto taskDto)
        {
            if(taskDto == null)
            {
                throw new ApiException(HttpStatusCode.BadRequest);
            }

            var taskItem = new TaskItem
            {
                Name = taskDto.Name,
                Description = taskDto.Description,
                DueDate = taskDto.DueDate,
                IsCompleted = taskDto.IsCompleted
            };

            var task = await _repository.AddAsync(taskItem) ?? throw new ApiException(HttpStatusCode.NotFound);
            return TaskMapper.ConvertToDto(task);
        }

        public async Task<int?> UpdateAsync(Guid id, TaskItemDto taskDto)
        {
            if (id != taskDto.Id)
            {
                throw new ApiException(HttpStatusCode.BadRequest);
            }

            var taskItem = await _repository.GetByIdAsync(id) ?? throw new ApiException(HttpStatusCode.NotFound);

            taskItem.Name = taskDto.Name;
            taskItem.Description = taskDto.Description;
            taskItem.DueDate = taskDto.DueDate;
            taskItem.IsCompleted = taskDto.IsCompleted;

            var rowsAffected = await _repository.UpdateAsync(taskItem) ?? throw new ApiException(HttpStatusCode.NotFound);
            return rowsAffected;
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            var taskItem = await _repository.GetByIdAsync(id) ?? throw new ApiException(HttpStatusCode.NotFound);
            var rowsAffected = await _repository.DeleteAsync(taskItem);
            return rowsAffected;
        }
    }
}
