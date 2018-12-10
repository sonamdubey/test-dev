CREATE TABLE [dbo].[AP_Processes] (
    [Id]       INT          NOT NULL,
    [APName]   VARCHAR (50) NOT NULL,
    [IsActive] BIT          CONSTRAINT [DF_AP_Processes_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_AP_Processes] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

