CREATE TABLE [dbo].[AbSure_CarError] (
    [Id]                  NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [AbSure_CarDetailsId] NUMERIC (18)  NOT NULL,
    [ErrorTimeStamp]      DATETIME      NULL,
    [ErrorCode]           VARCHAR (MAX) NULL,
    [Description]         VARCHAR (MAX) NULL,
    [CriticalityIndex]    SMALLINT      NULL,
    [Meaning]             VARCHAR (MAX) NULL,
    [Causes]              VARCHAR (MAX) NULL,
    [Symptoms]            VARCHAR (MAX) NULL,
    [Impact]              VARCHAR (MAX) NULL,
    [Solutions]           VARCHAR (MAX) NULL,
    [EntryDate]           DATETIME      CONSTRAINT [DF_AbSure_CarError_EntryDate] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_AbSure_CarScore_CarError] PRIMARY KEY CLUSTERED ([Id] ASC)
);

