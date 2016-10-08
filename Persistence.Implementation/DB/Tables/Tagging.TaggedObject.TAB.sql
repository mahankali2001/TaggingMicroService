IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name ='Tagging')
	EXEC dbo.sp_executesql @statement=N'CREATE SCHEMA Tagging';
GO

--IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Tagging].[TaggedObject]') AND type in (N'U'))
--DROP TABLE [Tagging].[TaggedObject]
--GO
IF EXISTS(SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TaggedObject')
BEGIN
	IF NOT EXISTS(SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'TaggedObject' AND COLUMN_NAME = 'ObjectTextId')
	BEGIN		
		drop table [Tagging].[TaggedObject]
	END
END
go


IF  NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Tagging].[TaggedObject]') AND type in (N'U'))
CREATE TABLE [Tagging].[TaggedObject](
	[TaggedObjectId] [int] IDENTITY(1,1) NOT NULL,	
	[TagId] [int] NOT NULL,	
	[ObjectTextId] nvarchar(40) NULL,
	[ObjectId] int NULL,
	[CreateDatetime] [datetime] NOT NULL,		
	[UpdatedDateTime] [datetime] NOT NULL,	
	[IsActive] bit NOT NULL,
 CONSTRAINT [PK_TaggedObject] PRIMARY KEY CLUSTERED 
(
	[TaggedObjectId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

-- select * from Businessmodule
-- select * from [Tagging].[Tag]
-- select * from [Tagging].[TaggedObject]