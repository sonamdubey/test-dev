CREATE TABLE [dbo].[TC_PrefNewCarColour] (
    [TC_PrefNewCarColourId] INT          IDENTITY (1, 1) NOT NULL,
    [TC_NewCarInquiriesId]  INT          NULL,
    [VersionColorsId]       NUMERIC (18) NULL,
    [CreatedOn]             DATETIME     CONSTRAINT [DF_TC_PrefNewCarColour_CreatedOn] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_TC_PrefNewCarColourId] PRIMARY KEY NONCLUSTERED ([TC_PrefNewCarColourId] ASC)
);

