CREATE TABLE [dbo].[StatesRTOCode] (
    [StateRTOCodeId] INT          IDENTITY (1, 1) NOT NULL,
    [StateId]        INT          NOT NULL,
    [StateRTOCode]   VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_StatesRTOCode_1] PRIMARY KEY CLUSTERED ([StateRTOCodeId] ASC)
);

