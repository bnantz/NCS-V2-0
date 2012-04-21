CREATE TABLE [dbo].[SiteMap] (
    [ID]          [int] NOT NULL,
    [Title]       [varchar] (32),
    [Description] [varchar] (512),
    [Url]         [varchar] (512),
    [Roles]       [varchar] (512),
    [Parent]      [int]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[SiteMap] ADD 
    CONSTRAINT [PK_SiteMap] PRIMARY KEY CLUSTERED 
    (
        [ID]
    )  ON [PRIMARY] 
GO;

CREATE PROCEDURE proc_GetSiteMap AS SELECT [ID], [Title],
[Description], [Url], [Roles], [Parent] FROM [SiteMap] ORDER BY [ID];
GO;