CREATE TABLE [dbo].[NewDealers] (
    [ID]            NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]          VARCHAR (50)  NOT NULL,
    [PhoneNo]       VARCHAR (25)  NULL,
    [ContactPerson] VARCHAR (50)  NULL,
    [EmailId]       VARCHAR (50)  NULL,
    [Organisation]  VARCHAR (50)  NULL,
    [Address]       VARCHAR (250) NULL,
    [Area]          VARCHAR (50)  NULL,
    [City]          NUMERIC (18)  NOT NULL,
    [Pincode]       VARCHAR (6)   NULL,
    [IsActive]      BIT           CONSTRAINT [DF_NewDealers_IsActive] DEFAULT (1) NOT NULL,
    [EntryDate]     DATETIME      NOT NULL,
    [StatusID]      NUMERIC (18)  NULL,
    CONSTRAINT [PK_NewDealers] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_NewDealers_Cities] FOREIGN KEY ([City]) REFERENCES [dbo].[Cities] ([ID])
);

