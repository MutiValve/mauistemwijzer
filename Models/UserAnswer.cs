namespace mauistemwijzer.Models;

public enum AnswerType
{
    Eens,
    Oneens,
    GeenVanBeide,
    Overgeslagen
}

public class UserAnswer
{
    public int QuestionId { get; set; }
    public AnswerType Answer { get; set; }
}
