CREATE TABLE [dbo].[OLM_OutletModelMapping] (
    [Id]         NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [ShowroomId] INT          NOT NULL,
    [ModelId]    INT          NOT NULL,
    [IsActive]   BIT          NOT NULL,
    [UpdatedBy]  BIGINT       NULL,
    [UpadtedOn]  DATETIME     CONSTRAINT [DF_OLM_OutletModelMapping_UpadtedOn] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_OLM_OutletModelMapping] PRIMARY KEY CLUSTERED ([Id] ASC)
);

