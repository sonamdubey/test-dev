CREATE TABLE [dbo].[Classified_ReportListing] (
    [Id]            NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [InquiryId]     NUMERIC (18)  NOT NULL,
    [InquiryType]   SMALLINT      NOT NULL,
    [ReasonId]      INT           NOT NULL,
    [Description]   VARCHAR (MAX) NOT NULL,
    [CustomerId]    NUMERIC (18)  NULL,
    [EntryDate]     DATETIME      CONSTRAINT [DF_Classified_ReportListing_EntryDate] DEFAULT (getdate()) NOT NULL,
    [IsActionTaken] BIT           CONSTRAINT [DF_Classified_ReportListing_IsActionTaken] DEFAULT ((0)) NULL,
    [EmailId]       VARCHAR (50)  NULL,
    CONSTRAINT [PK_Classified_ReportListing] PRIMARY KEY CLUSTERED ([Id] ASC)
);

