CREATE TABLE [dbo].[Acc_ProductRatingParameters] (
    [Id]            NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ProductId]     NUMERIC (18)  NOT NULL,
    [ParameterName] VARCHAR (100) NOT NULL,
    [IsActive]      BIT           CONSTRAINT [DF_Acc_ProductRatingParameters_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_Acc_ProductRatingParameters] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

