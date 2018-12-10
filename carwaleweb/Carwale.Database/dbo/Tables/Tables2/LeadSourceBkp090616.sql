CREATE TABLE [dbo].[LeadSourceBkp090616] (
    [Id]                INT          IDENTITY (1, 1) NOT NULL,
    [PlatformId]        INT          NOT NULL,
    [AdType]            INT          NOT NULL,
    [SourceType]        VARCHAR (50) NULL,
    [LeadClickSourceId] INT          NOT NULL,
    [InquirySourceId]   INT          NOT NULL
);

