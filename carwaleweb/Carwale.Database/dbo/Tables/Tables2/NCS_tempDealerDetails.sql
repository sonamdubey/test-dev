CREATE TABLE [dbo].[NCS_tempDealerDetails] (
    [Id]            NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [DealerId]      NUMERIC (18)  NOT NULL,
    [Name]          VARCHAR (200) NULL,
    [LandLine]      VARCHAR (50)  NULL,
    [Mobile]        VARCHAR (50)  NULL,
    [Address]       VARCHAR (500) NULL,
    [ContactPerson] VARCHAR (50)  NULL,
    [Email]         VARCHAR (200) NULL,
    [IsApproved]    BIT           NULL,
    [UpdatedBy]     NUMERIC (18)  NULL,
    [UpdatedOn]     DATETIME      NOT NULL,
    [Fax]           VARCHAR (50)  NULL,
    CONSTRAINT [PK_NCS_tempDealerDetails] PRIMARY KEY CLUSTERED ([Id] ASC)
);

