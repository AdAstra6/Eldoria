using System.Collections.Generic;

public static class QuestionsCategoriesExtensions
{
    private static readonly Dictionary<QuestionsCategories, string> CategoryKeys = new()
    {
        { QuestionsCategories.MATH, "math" },
        { QuestionsCategories.GEOGRAPHY, "geography" },
        { QuestionsCategories.BIOLOGY, "biology" },
        { QuestionsCategories.GENERAL_KNOWLEDGE, "general_knowledge" },
        { QuestionsCategories.ETHICS, "ethics" },
        { QuestionsCategories.ZOOLOGY, "zoology" },
        { QuestionsCategories.ASTRONOMY, "astronomy" }
    };

    public static string GetKey(this QuestionsCategories category)
    {
        return CategoryKeys.TryGetValue(category, out var key) ? key : category.ToString();
    }
}