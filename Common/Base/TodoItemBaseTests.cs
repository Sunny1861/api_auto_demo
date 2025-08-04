using FluentAssertions;
using System.Net;

namespace WebAPIDemo.Tests.Common.Base
{
    public abstract class TodoItemBaseTests : BaseTest
    {
        protected const string Path = "/api/TodoItems"; // Static so it's shared across all feature test methods

        protected record TodoItemDto(long Id, string Name, bool IsComplete);

        protected async Task<TodoItemDto> CreateTodoAsync(string name = "Sample", bool isComplete = false)
        {
            var todo = new { Name = name, IsComplete = isComplete };
            var response = await SafeSendAsync(() => Client.PostAsync($"{Path}", AsStringContent(todo)), "CreateTodo");

            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            return (await DeserializeJsonAsync<TodoItemDto>(response.Content))!;
        }

        protected async Task<HttpResponseMessage> DeleteTodoAsync(long id)
        {
            return await SafeSendAsync(() => Client.DeleteAsync($"{Path}/{id}"), "DeleteTodo");
        }

        protected async Task<HttpResponseMessage> GetTodoAsync(long id)
        {
            return await SafeSendAsync(() => Client.GetAsync($"{Path}/{id}"), "GetTodo");
        }

        protected async Task<HttpResponseMessage> UpdateTodoAsync(TodoItemDto updated)
        {
            return await SafeSendAsync(() =>
                Client.PutAsync($"{Path}/{updated.Id}", AsStringContent(updated)), "UpdateTodo");
        }

        protected async Task<List<TodoItemDto>> GetAllTodoAsync()
        {
            var response = await SafeSendAsync(() => Client.GetAsync($"{Path}"), "GetAll");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            return await DeserializeJsonAsync<List<TodoItemDto>>(response.Content) ?? [];
        }

        protected async Task<long> GetInvalidId()
        {
            var items = await this.GetAllTodoAsync();
            // Find max ID
            long maxId = items!.Count > 0 ? items.Max(x => x.Id) : 0;
            // The test may run in parallel, so calculate the valid Id use max Id + 10000
            long invalidId = maxId + 10000;

            return invalidId;
        }
        //TODO: More feature common setup will add
    }
}