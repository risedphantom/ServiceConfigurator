CREATE TABLE [dbo].[IISRestriction] (
    [ID]           INT            IDENTITY (1, 1) NOT NULL,
    [TypeName]     VARCHAR (250)  NOT NULL,
    [StartMoment]  DATETIME       NOT NULL,
    [Enabled]      BIT            NOT NULL,
    [Rule]         XML            NOT NULL,
    [IISSiteID]    INT            NOT NULL,
    [StopMoment]   DATETIME       NOT NULL,
    [CreateMoment] DATETIME       DEFAULT (getdate()) NOT NULL,
    [Error]        VARCHAR (8000) NULL,
    CONSTRAINT [PK_IISRestrictions] PRIMARY KEY CLUSTERED ([ID] ASC)
);

