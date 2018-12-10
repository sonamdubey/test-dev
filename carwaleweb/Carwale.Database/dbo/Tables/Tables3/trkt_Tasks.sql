CREATE TABLE [dbo].[trkt_Tasks] (
    [TaskNo]       INT           IDENTITY (1, 1) NOT NULL,
    [MasterTaskNo] INT           NULL,
    [Descr]        VARCHAR (300) NOT NULL,
    [CategoryId]   SMALLINT      NOT NULL,
    [StartDate]    DATETIME      NOT NULL,
    [EndDate]      DATETIME      NULL,
    [IsActive]     BIT           NULL
);

