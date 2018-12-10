CREATE TABLE [dbo].[OLM_ShowroomDetails] (
    [Id]            NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [SkodaDealerId] NUMERIC (18)  NOT NULL,
    [CityId]        INT           NOT NULL,
    [SAddress]      VARCHAR (500) NOT NULL,
    [SContactNo]    VARCHAR (500) NULL,
    [SEmail]        VARCHAR (500) NULL,
    [SFax]          VARCHAR (500) NULL,
    [IsRapidOutlet] BIT           NULL,
    [SOutlet]       VARCHAR (50)  NULL,
    [OutletCode]    VARCHAR (50)  NULL,
    [Lattitude]     VARCHAR (50)  NULL,
    [Longitude]     VARCHAR (50)  NULL,
    [UpdatedBy]     NUMERIC (18)  NULL,
    [UpdatedOn]     DATETIME      NULL,
    [IsDeleted]     BIT           NULL,
    CONSTRAINT [PK_OLM_ShowroomDetails] PRIMARY KEY CLUSTERED ([Id] ASC)
);

