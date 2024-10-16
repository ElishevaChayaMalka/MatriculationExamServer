namespace MatriculationExamsServer.DTO
{
    public class ExamScores
    {
        public ExamScores(string SubjectName, List<Exam> Exams) {
            this.SubjectName = SubjectName;
            this.Exams = Exams;
         
        } 
        public string SubjectName {  get; set; }
        public List<Exam> Exams { get; set; }


    }
}
