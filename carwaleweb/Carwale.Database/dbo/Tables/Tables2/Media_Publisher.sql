CREATE TABLE [dbo].[Media_Publisher] (
    [ID]       NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]     VARCHAR (100) NULL,
    [IsActive] BIT           CONSTRAINT [DF_Media_Publisher_IsActive] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_Media_Publisher] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

