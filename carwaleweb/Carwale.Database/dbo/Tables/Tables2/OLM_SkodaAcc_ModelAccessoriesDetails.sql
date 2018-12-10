CREATE TABLE [dbo].[OLM_SkodaAcc_ModelAccessoriesDetails] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [ModelId]     INT           NOT NULL,
    [CategoryId]  INT           NOT NULL,
    [Image]       VARCHAR (20)  NULL,
    [Description] VARCHAR (100) NOT NULL,
    [Price]       NUMERIC (18)  NOT NULL,
    [HostUrl]     VARCHAR (150) NULL,
    [IsActive]    BIT           CONSTRAINT [DF_OLM_SkodaAcc_ModelAccessoriesDetails_IsActive] DEFAULT ((1)) NOT NULL
);

