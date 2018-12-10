CREATE TABLE [dbo].[CRM_FLCMessage] (
    [ID]             NUMERIC (18)   IDENTITY (1, 1) NOT NULL,
    [ModelId]        NUMERIC (18)   NOT NULL,
    [CityId]         NUMERIC (18)   NOT NULL,
    [Message]        VARCHAR (1500) NOT NULL,
    [AddedOn]        DATETIME       NULL,
    [AddedBy]        NUMERIC (18)   NULL,
    [StateId]        NUMERIC (18)   NULL,
    [LatestUpdateOn] DATETIME       NULL,
    [LatestUpdateBy] NUMERIC (18)   NULL,
    CONSTRAINT [PK_CRM_FLCMessage] PRIMARY KEY CLUSTERED ([ID] ASC)
);

