using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Personal.Entities
{
  public class Post
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Slug { get; set; }
    public string Content { get; set; }
    public string DisplayPictureLink { get; set; }
    public PostStatus PostStatus { get; set; }
    public virtual ICollection<PostTag> PostTags { get; set; }
    public virtual ICollection<PostCategory> PostCategories { get; set; }
    public Post()
    {
      PostTags = new Collection<PostTag>();
      PostCategories = new Collection<PostCategory>();
    }

  }
}