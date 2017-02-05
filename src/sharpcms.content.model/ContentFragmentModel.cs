using System;
using Dapper.Contrib.Extensions;

namespace sharpcms.content.model
{
    [Table(nameof(ContentFragmentModel))]
    public class ContentFragmentModel
    {
        [Key]
        public int Id { get; set; }

        public int Order { get; set; }

        public string Content { get; set; }

        public string Section { get; set; }

        public string Target { get; set; }

        public string Author { get; set; }

        public string Tags { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

        public DateTime? Deleted { get; set; }

        protected bool Equals(ContentFragmentModel other)
        {
            return Id == other.Id 
                && Order == other.Order 
                && string.Equals(Content, other.Content) 
                && string.Equals(Section, other.Section) 
                && string.Equals(Target, other.Target) 
                && string.Equals(Author, other.Author) 
                && string.Equals(Tags, other.Tags);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ContentFragmentModel) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id;
                hashCode = (hashCode * 397) ^ Order;
                hashCode = (hashCode * 397) ^ (Content != null ? Content.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Section != null ? Section.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Target != null ? Target.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Author != null ? Author.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Tags != null ? Tags.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Created.GetHashCode();
                hashCode = (hashCode * 397) ^ Updated.GetHashCode();
                hashCode = (hashCode * 397) ^ Deleted.GetHashCode();
                return hashCode;
            }
        }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Order)}: {Order}, {nameof(Content)}: {Content}, {nameof(Section)}: {Section}, {nameof(Target)}: {Target}, {nameof(Author)}: {Author}, {nameof(Tags)}: {Tags}";
        }
    }
}