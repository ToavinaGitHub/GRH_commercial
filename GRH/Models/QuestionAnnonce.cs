namespace GRH.Models
{
    public class QuestionAnnonce
    {
        public int idQuestionAnnonce { get; set; }

        public Question question { get; set; }

        public Annonce annonce { get; set; }

        public QuestionAnnonce() { }

        public QuestionAnnonce(int idQuestionAnnonce, Question question, Annonce annonce)
        {
            this.idQuestionAnnonce = idQuestionAnnonce;
            this.question = question;
            this.annonce = annonce;
        }
    }
}
