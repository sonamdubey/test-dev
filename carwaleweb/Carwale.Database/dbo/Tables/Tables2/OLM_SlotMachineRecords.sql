CREATE TABLE [dbo].[OLM_SlotMachineRecords] (
    [Id]         NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [Name]       VARCHAR (50) NOT NULL,
    [Contact]    VARCHAR (15) NOT NULL,
    [Email]      VARCHAR (50) NOT NULL,
    [ClientIp]   VARCHAR (20) NULL,
    [IsWinner]   BIT          NULL,
    [NineCount]  SMALLINT     NULL,
    [WinNumbers] VARCHAR (15) NULL,
    [EntryDate]  DATETIME     CONSTRAINT [DF_OLM_SlotMachineRecords_EntryDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_OLM_SlotMachineRecords] PRIMARY KEY CLUSTERED ([Id] ASC)
);

