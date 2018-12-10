CREATE TABLE [dbo].[OLM_PreferenceFields] (
    [Id]         NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [ModelId]    NUMERIC (18) NOT NULL,
    [FieldValue] VARCHAR (5)  NOT NULL,
    [FieldOrder] SMALLINT     NOT NULL,
    [UpdateOn]   DATETIME     CONSTRAINT [DF_OLM_PerformanceFields_UpdateOn] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_OLM_PerformanceFields] PRIMARY KEY CLUSTERED ([Id] ASC)
);

