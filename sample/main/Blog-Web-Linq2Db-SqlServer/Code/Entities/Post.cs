using System;
using LinqToDB.Mapping;

namespace Blog.Web.Linq2Db.SqlServer.Code.Entities
{
    [Table(Name = "POSTS")]
    public class Post
    {
        [PrimaryKey, Identity, Column(Name = "POST_ID")]
        public virtual int Id { get; set; }

        [Column(Name = "TITLE")]                
        public virtual string Title { get; set; }

        [Column(Name = "TEXT")]                
        public virtual string Text { get; set; }

        [Column(Name = "DATE")]                
        public virtual DateTime Created { get; set; }
        
        [Column(Name = "AUTHOR_ID")]
        public virtual int AuthorId { get; set; }
    }
}
