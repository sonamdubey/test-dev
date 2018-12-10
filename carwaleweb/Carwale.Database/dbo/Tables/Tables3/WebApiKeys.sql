CREATE TABLE [dbo].[WebApiKeys] (
    [ID]       INT          IDENTITY (1, 1) NOT NULL,
    [SourceId] NUMERIC (18) NULL,
    [CWK]      VARCHAR (30) NULL,
    [IsActive] BIT          NULL,
    CONSTRAINT [PK__WebApiKe__3214EC2741EA03DC] PRIMARY KEY CLUSTERED ([ID] ASC)
);

