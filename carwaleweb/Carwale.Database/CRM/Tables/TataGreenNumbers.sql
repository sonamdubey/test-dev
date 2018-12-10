CREATE TABLE [CRM].[TataGreenNumbers] (
    [Id]          NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [CBDId]       NUMERIC (18)  NOT NULL,
    [Number]      VARCHAR (200) NULL,
    [UpdatedBy]   NUMERIC (18)  NOT NULL,
    [UpdatedDate] DATETIME      NOT NULL,
    CONSTRAINT [PK_TataGreenNumbers] PRIMARY KEY CLUSTERED ([Id] ASC)
);

