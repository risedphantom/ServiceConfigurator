CREATE TABLE [dbo].[IISSite] (
    [ID]       INT           IDENTITY (1, 1) NOT NULL,
    [SiteName] VARCHAR (250) NOT NULL,
    [Hostname] VARCHAR (250) NOT NULL,
    [Group]    VARCHAR (250) NULL,
    [Deleted]  BIT           DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_IISSite] PRIMARY KEY CLUSTERED ([ID] ASC)
);

