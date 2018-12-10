CREATE TABLE [dbo].[ESM_Contact] (
    [id]          NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [ClientId]    NUMERIC (18)  NOT NULL,
    [Name]        VARCHAR (50)  NOT NULL,
    [Mobile]      NUMERIC (18)  NOT NULL,
    [email]       VARCHAR (50)  NULL,
    [LandLine]    NUMERIC (18)  NULL,
    [Designation] VARCHAR (50)  NULL,
    [Fax]         NUMERIC (18)  NULL,
    [Address]     VARCHAR (50)  NULL,
    [stateId]     NUMERIC (18)  NOT NULL,
    [cityId]      NUMERIC (18)  NOT NULL,
    [Remarks]     VARCHAR (250) NULL,
    [UpdatedOn]   DATETIME      NOT NULL,
    [UpdatedBy]   NUMERIC (18)  NOT NULL,
    CONSTRAINT [PK_ESM_Contact] PRIMARY KEY CLUSTERED ([id] ASC) WITH (FILLFACTOR = 90)
);

