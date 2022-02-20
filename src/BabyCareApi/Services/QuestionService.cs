using BabyCareApi.Models.Common;
using BabyCareApi.Models.Requests;
using MongoDB.Driver;
using OrbitFoodApi.Models.Common;

namespace BabyCareApi.Services;

public class QuestionService
{
  private static readonly FilterDefinitionBuilder<Question> Filter = Builders<Question>.Filter;
  private static readonly UpdateDefinitionBuilder<Question> Update = Builders<Question>.Update;
  private static readonly FindOneAndUpdateOptions<Question> _ReturnAfter = new()
  {
    ReturnDocument = ReturnDocument.After
  };
  public IMongoCollection<Question> Collection { get; set; }

  public QuestionService(IMongoCollection<Question> questionCollection)
  {
    Collection = questionCollection;

  }

  public Task CreateAsync(Question model) => Collection.InsertOneAsync(model);

  public async Task<Question?> GetByIdAsync(string Id) => await Collection.Find(Filter.Eq(x => x.Id, Id)).FirstOrDefaultAsync();

  public Task<List<Question>> ListAsync(ListQuestions model)
  {
    return Collection
    .Find(Filter.Eq(x => x.SurveyId, model.SurveyId))
    .Limit(model.Limit)
    .ToListAsync();
  }

  public async Task<Question?> UpdateAsync(string Id, UpdateQuestionRequest model)
  {
    var updates = new List<UpdateDefinition<Question>>();

    if (model.Title != null)
      updates.Add(Update.Set(x => x.Title, model.Title));

    if (model.QuestionType != null)
      updates.Add(Update.Set(x => x.QuestionType, model.QuestionType));


    if (updates.Any())
      return await Collection.FindOneAndUpdateAsync(Filter.Eq(x => x.Id, Id), Update.Combine(updates), _ReturnAfter);

    return await GetByIdAsync(Id);
  }
}