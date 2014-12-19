namespace Blog.Service.Contract.Dto
{
    public class SavePostDto
    {        
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public int AuthodId { get; set; }
    }
}
