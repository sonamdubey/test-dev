CREATE TABLE [dbo].[TC_CarVersionPQ] (
    [TC_CarVersionPQId]  INT IDENTITY (1, 1) NOT NULL,
    [DealerId]           INT NULL,
    [VersionId]          INT NULL,
    [TC_PQFieldMasterId] INT NULL,
    [TC_UserID]          INT NULL,
    [Value]              INT NULL,
    [IsActive]           BIT CONSTRAINT [DF_TC_CarVersionPQ_IsActive] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_TC_CarVersionPQTC_CarVersionPQId] PRIMARY KEY NONCLUSTERED ([TC_CarVersionPQId] ASC)
);

