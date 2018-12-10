CREATE TABLE [dbo].[CW_CarProcessingFees] (
    [Id]             INT           IDENTITY (1, 1) NOT NULL,
    [MinAmount]      DECIMAL (18)  NULL,
    [MaxAmount]      DECIMAL (18)  NULL,
    [Text]           VARCHAR (100) NULL,
    [ProcessingFees] DECIMAL (18)  NULL,
    [IsActive]       BIT           CONSTRAINT [DF_CW_CarProcessingFees_IsActive] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_CW_CarProcessingFees] PRIMARY KEY CLUSTERED ([Id] ASC)
);

