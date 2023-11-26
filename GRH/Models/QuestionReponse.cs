namespace GRH.Models
{
    public class QuestionReponse
    {

        public List<Question> questions { get; set; }
        public List<List<Reponse>> reponses { get; set; } 

        public QuestionReponse() { }

        public QuestionReponse(List<Question> questions, List<List<Reponse>> reponses)
        {
            this.questions = questions;
            this.reponses = reponses;
        }
    }
}