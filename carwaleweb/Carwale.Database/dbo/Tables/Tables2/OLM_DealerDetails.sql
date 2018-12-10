CREATE TABLE [dbo].[OLM_DealerDetails] (
    [Id]                NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [SkodaDealerId]     NUMERIC (18)  NOT NULL,
    [CityId]            INT           NOT NULL,
    [DealerPrinciple]   VARCHAR (500) NOT NULL,
    [DMobileNoAndEmail] VARCHAR (500) NOT NULL,
    [ContactPerson]     VARCHAR (500) NULL,
    [CMobileNoAndEmail] VARCHAR (500) NULL,
    [UpdatedBy]         NUMERIC (18)  NULL,
    [UpdatedOn]         DATETIME      NULL,
    [IsDeleted]         BIT           NULL,
    CONSTRAINT [PK_OLM_DealerDetails] PRIMARY KEY CLUSTERED ([Id] ASC)
);

