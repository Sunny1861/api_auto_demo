using FluentAssertions;
using System.Net;
using WebAPIDemo.Tests.Common.Base;

namespace WebAPIDemo.Tests.Controller
{

    [TestClass]
    public class TodoItemsApiTests : TodoItemBaseTests
    {
        [TestMethod]
        public async Task GetAll_ReturnsSuccessAndList()
        {
            await GetAllTodoAsync();
        }

        [TestMethod]
        [DataRow("Test todo 1", false)]
        [DataRow("Test todo 2", true)]
        [DataRow("Test \"todo \\2", true)]
        [DataRow("", false)]
        [DataRow("中文", true)]
        public async Task CreateItem_ReturnsCreated(string name, bool isComplete)
        {
            var todo = new { Name = name, IsComplete = isComplete };

            var response = await SafeSendAsync(() => Client.PostAsync($"{Path}", AsStringContent(todo)), "CreateItem");
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            TodoItemDto? res = await DeserializeJsonAsync<TodoItemDto>(response.Content);

            // Assert the response matches the input
            res.Should().BeEquivalentTo(todo); // Ignores res.Id since it's not in the anonymous object

        }

        [TestMethod]
        [Priority(2)]
        [TestCategory("Negative")]
        public async Task GetItem_WithNegativeId_NotFound_Returns404()
        {
            var response = await GetTodoAsync(-1);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [TestMethod]
        public async Task GetItem_WithInvalidId_NotFound_Returns404()
        {
            long invalidId = await GetInvalidId();
            var response = await GetTodoAsync(invalidId);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [TestMethod]
        public async Task PutItem_UpdatesSuccessfully()
        {
            var created = await CreateTodoAsync("Update Me", false);
            var updated = created with { Name = "Updated", IsComplete = true };
            var response = await UpdateTodoAsync(updated);
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var check = await GetTodoAsync(created.Id);
            var res = await DeserializeJsonAsync<TodoItemDto>(check.Content);
            res.Should().BeEquivalentTo(updated);
            
        }

        [TestMethod]
        public async Task PutItem_WithNegativeId_NotFound_Returns404()
        {
            var updated = new TodoItemDto(Id: -1, Name:"Update Me", IsComplete: true);
            var response = await UpdateTodoAsync(updated);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [TestMethod]
        public async Task PutItem_WithInvalidId_NotFound_Returns404()
        {
            var updated = new TodoItemDto(Id: await GetInvalidId(), Name:"Update Me", IsComplete: true);
            var response = await UpdateTodoAsync(updated);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [TestMethod]
        public async Task DeleteItem_RemovesItem()
        {
            var created = await CreateTodoAsync("Delete Me", false);
            var response = await DeleteTodoAsync(created.Id);
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var check = await GetTodoAsync(created.Id);
            check.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [TestMethod]
        public async Task DeleteItem_WithNegativeId_NotFound_Returns404()
        {
            var response = await DeleteTodoAsync(-1);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [TestMethod]
        public async Task DeleteItem_WithInvalidId_NotFound_Returns404()
        {
            long invalidId = await GetInvalidId();

            var response = await DeleteTodoAsync(invalidId);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }

    #region old code

    // [TestClass]
    // public class ToDoItemControllerTests : TodoItemBaseTests
    // {
    //     #region  Create Todo
    //     [DataTestMethod]
    //     [DataRow("Create do home work 1", false)]
    //     [DataRow("", true)]
    //     [DataRow(null, true)]
    //     [DataRow("Create my lenth is 256 bytes Abcdefgabcdfggabcd Abcdefgabcdfggabcd Abcdefgabcdfggabcd Abcdefgabcdfggabcd Abcdefgabcdfggabcd Abcdefgabcdfggabcd Abcdefgabcdfggabcd Abcdefgabcdfggabcd", true)]
    //     public async Task CreateTodoItem(string? name, bool IsComplete)
    //     {
    //         var createdItem = await this.HelpCreateTodoItem(name!, IsComplete);

    //         createdItem.Name.Should().BeEquivalentTo(name);
    //         if (!IsComplete)
    //         {
    //             createdItem.IsComplete.Should().BeFalse();
    //         }
    //         else
    //         {
    //             createdItem.IsComplete.Should().BeTrue();
    //         }

    //     }

    //     [TestMethod]
    //     public async Task CreateTodoItemWithInvalidParam()
    //     {
    //         string json = "{\"name\": 55,\"isComplete\": true}";
    //         var content = new StringContent(json, Encoding.UTF8, "application/json");

    //         // Act
    //         var response = await _client.PostAsync($"/{Path}", content);
    //         Console.WriteLine("-------------" + response);
    //         // Validate status code is 400, above post payload is incorrect
    //         response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    //     }
    //     #endregion

    //     #region Update Todo
    //     [TestMethod]
    //     public async Task ModifyTodoItem()
    //     {
    //         var createdItem = await this.HelpCreateTodoItem("do task 8", false);

    //         var updateItem = new TodoItem
    //         {
    //             Id = createdItem.Id,
    //             Name = "update item, original is: " + createdItem.Name,
    //             IsComplete = true
    //         };

    //         var updateContent = new StringContent(
    //             JsonSerializer.Serialize(updateItem),
    //             Encoding.UTF8,
    //             "application/json");

    //         // Act
    //         var putResponse = await _client.PutAsync($"/{Path}/{createdItem.Id}", updateContent);

    //         // Assert
    //         putResponse.StatusCode.Should().Be(HttpStatusCode.NoContent); // 204 No Content expected

    //     }

    //     [TestMethod]
    //     public async Task ModifyTodoItemWithNegativeId()
    //     {
    //         long negativeId = -1;
    //         var updateItem = new TodoItem
    //         {
    //             Id = negativeId,
    //             Name = "Try to update not exiting item",
    //             IsComplete = true
    //         };

    //         var updateContent = new StringContent(
    //             JsonSerializer.Serialize(updateItem),
    //             Encoding.UTF8,
    //             "application/json");

    //         // Act
    //         var putResponse = await _client.PutAsync($"/{Path}/{negativeId}", updateContent);

    //         // Assert
    //         putResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    //     }

    //     [TestMethod]
    //     public async Task ModifyTodoItemWithInvalidId()
    //     {
    //         long invalidId = 8;

    //         // Act
    //         var response = await _client.GetAsync($"/{Path}");
    //         var json = await response.Content.ReadAsStringAsync();

    //         // Deserialize the response to a list
    //         List<TodoItem>? items = JsonSerializer.Deserialize<List<TodoItem>>(json, new JsonSerializerOptions
    //         {
    //             PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    //         });

    //         List<TodoItem>? sorted = items?.OrderByDescending(x => x.Id).ToList();
    //         var lastItem = sorted?.FirstOrDefault();
    //         if (lastItem != null)
    //         {
    //             // Test may run in parallel, in order to avoid the concurrent issue, plus 1000 base on current biggest Id
    //             invalidId = lastItem.Id + 1000;
    //         }

    //         var updateItem = new TodoItem
    //         {
    //             Id = invalidId,
    //             Name = "Try to update not exiting item",
    //             IsComplete = true
    //         };

    //         var updateContent = new StringContent(
    //             JsonSerializer.Serialize(updateItem),
    //             Encoding.UTF8,
    //             "application/json");

    //         // Act
    //         var putResponse = await _client.PutAsync($"{Path}/{invalidId}", updateContent);

    //         // Assert
    //         putResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    //     }

    //     #endregion

    //     #region  Delete Todo
    //     [TestMethod]
    //     public async Task DeleteToDoItem()
    //     {
    //         var newCreated_item = await this.HelpCreateTodoItem("new created do to item", false);

    //         var response = await _client.DeleteAsync($"/{Path}/{newCreated_item.Id}");

    //         // Assert
    //         response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    //     }

    //     [TestMethod]
    //     public async Task DeleteToDoItemWithInvalidId()
    //     {
    //         // Act
    //         var response = await _client.DeleteAsync($"/{Path}/-1");

    //         // Assert
    //         response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    //     }


    //     #endregion

    //     #region  Get todo
    //     [TestMethod]
    //     public async Task GetOneTodoItem()
    //     {
    //         var newCreated_item = await this.HelpCreateTodoItem("new created do to item", false);

    //         var response = await _client.GetAsync($"/{Path}/{newCreated_item.Id}");

    //         // Assert
    //         response.StatusCode.Should().Be(HttpStatusCode.OK);

    //         var responseBody = await response.Content.ReadAsStringAsync();

    //         var options = new JsonSerializerOptions
    //         {
    //             WriteIndented = true,
    //             PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    //         };
    //         var getItem = JsonSerializer.Deserialize<TodoItem>(
    //             responseBody,
    //             options
    //         );

    //         newCreated_item.Should().BeEquivalentTo(getItem);

    //     }

    //     [TestMethod]
    //     public async Task GetTodoItemWithInvalidId()
    //     {
    //         // Act
    //         var response = await _client.GetAsync($"/{Path}/-1");

    //         // Assert
    //         response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    //     }

    //     [TestMethod]
    //     public async Task GetAllToDoItem()
    //     {
    //         var response = await _client.GetAsync($"/{Path}");

    //         // Assert
    //         response.StatusCode.Should().Be(HttpStatusCode.OK);
    //     }

    //     #endregion

    //     #region Helper 
    //     /*
    //     This is help method, help to create a todo item, it will help on creating, modifying or deleting
    //     */
    //     private async Task<TodoItem> HelpCreateTodoItem(string name, bool IsComplete)
    //     {
    //         var newItem = new TodoItem
    //         {
    //             Name = name,
    //             IsComplete = IsComplete
    //         };

    //         var options = new JsonSerializerOptions
    //         {
    //             WriteIndented = true,
    //             PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    //         };

    //         string json = JsonSerializer.Serialize(newItem, options);
    //         var content = new StringContent(json, Encoding.UTF8, "application/json");

    //         // Act
    //         var response = await _client.PostAsync($"/{Path}", content);

    //         // Assert
    //         response.StatusCode.Should().Be(HttpStatusCode.Created);

    //         var responseBody = await response.Content.ReadAsStringAsync();

    //         var createdItem = JsonSerializer.Deserialize<TodoItem>(
    //             responseBody,
    //             options
    //         ) ?? throw new InvalidOperationException("Failed to deserialize TodoItem.");
    //         return createdItem;
    //     }

    //     #endregion
    // }
    #endregion
}