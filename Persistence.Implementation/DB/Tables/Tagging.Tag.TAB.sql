IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name ='Tagging')
	EXEC dbo.sp_executesql @statement=N'CREATE SCHEMA Tagging';
GO

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Tagging].[Tag]') AND type in (N'U'))
--DROP TABLE [Tagging].[Tag]
--GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Tagging].[Tag]') AND type in (N'U'))
CREATE TABLE [Tagging].[Tag](
	[TagId] [int] IDENTITY(1,1) NOT NULL,
	[TagName] [nvarchar](50) NOT NULL,
	[UserId] [int] NOT NULL,
	[ApplicationId] [int] NOT NULL,
	[CreateDatetime] [datetime] NOT NULL,		
	[UpdatedDateTime] [datetime] NOT NULL,	
	[IsActive] bit NOT NULL,
 CONSTRAINT [PK_Tag] PRIMARY KEY CLUSTERED 
(
	[TagId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


-- select * from [Tagging].[Tag]