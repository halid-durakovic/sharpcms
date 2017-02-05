using System.Linq;
using Dapper;
using sharpcms.database;

namespace sharpcms.content.providers
{
    public class ContentFragmentProvider
    {
        private readonly DbConnectionService _db;

        public ContentFragmentProvider() : this(new DbConnectionService())
        {
        }

        private ContentFragmentProvider(DbConnectionService db)
        {
            _db = db;
        }

        public void CreateIfDoesNotExist(string name)
        {
            if (contentFragmentExists(name)) return;

            using (var c = _db.GetConnection(name))
            {
                c.Connection.Execute($"{getCreateContentFragmentTableSql()};{getCreateContentFragmentCreatedConstraintSql()};{getCreateContentFragmentUpdatedConstraintSql()};");
            }
        }

        public void DeleteIfDoesExist(string name)
        {
            if (!contentFragmentExists(name)) return;

            using (var c = _db.GetConnection(name))
            {
                c.Connection.Execute($"{getDeleteContentFragmentCreatedConstraintSql()};{getDeleteContentFragmentUpdatedConstraintSql()};{getDeleteContentFragmentTableSql()};");
            }
        }

        private bool contentFragmentExists(string name)
        {
            using (var c = _db.GetConnection(name))
            {
                return c.Connection.Query<int>($"{getContentFragmentExistsSql()}").First() == 1;
            }
        }

        private string getCreateContentFragmentTableSql()
        {
            return $@"CREATE TABLE [dbo].[ContentFragment](
	            [Id] [int] IDENTITY(1,1) NOT NULL,
	            [Order] [int] NOT NULL,
	            [Content] [nvarchar](max) NOT NULL,
	            [Section] [nvarchar](1024) NOT NULL,
	            [Target] [nvarchar](1024) NOT NULL,
	            [Author] [nvarchar](1024) NOT NULL,
	            [Tags] [nvarchar](max) NOT NULL,
	            [Created] [datetime] NOT NULL,
	            [Updated] [datetime] NOT NULL,
	            [Deleted] [datetime] NULL,
             CONSTRAINT [PK_ContentFragment] PRIMARY KEY CLUSTERED 
            (
	            [Id] ASC
            )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
            ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]";
        }

        private string getDeleteContentFragmentTableSql()
        {
            return @"DROP TABLE [dbo].[ContentFragment]";
        }

        private string getCreateContentFragmentCreatedConstraintSql()
        {
            return @"ALTER TABLE [dbo].[ContentFragment] ADD  CONSTRAINT [DF_ContentFragment_Created]  DEFAULT (getdate()) FOR [Created]";
        }

        private string getDeleteContentFragmentCreatedConstraintSql()
        {
            return @"ALTER TABLE [dbo].[ContentFragment] DROP CONSTRAINT [DF_ContentFragment_Created]";
        }

        private string getCreateContentFragmentUpdatedConstraintSql()
        {
            return @"ALTER TABLE [dbo].[ContentFragment] ADD  CONSTRAINT [DF_ContentFragment_Updated]  DEFAULT (getdate()) FOR [Updated]";
        }

        private string getDeleteContentFragmentUpdatedConstraintSql()
        {
            return @"ALTER TABLE [dbo].[ContentFragment] DROP CONSTRAINT [DF_ContentFragment_Updated]";
        }

        private string getContentFragmentExistsSql()
        {
            return @"IF (EXISTS(SELECT [table_name] FROM INFORMATION_SCHEMA.TABLES WHERE [table_name] = N'ContentFragment'))
                BEGIN
	                SELECT 1
                END
                ELSE
                BEGIN
	                SELECT 0
                END";
        }
    }
}