using Google.Apis.Sheets.v4.Data;

namespace MatriculationExamsServer.DTO
{
    public class Exam
    {
        public Exam(string ExamScore,string ExamSubject, Color ColorCell,string ExamName)
        {
            this.ExamScore = ExamScore;
            this.ExamSubject = ExamSubject;
            this.ColorCell = ColorCell;
            this.ExamName = ExamName;

        }
        public Exam() { }
    
        
       // public string ExamId { get; set; }
        public string ExamScore { get; set; }
        public string ExamSubject{ get; set; }
        public string ExamName { get; set; }

        public Color ColorCell { get; set; }
     //   public string ExamType { get; set; }

    }
}
