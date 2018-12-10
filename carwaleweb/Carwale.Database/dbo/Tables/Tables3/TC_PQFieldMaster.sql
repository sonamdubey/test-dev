CREATE TABLE [dbo].[TC_PQFieldMaster] (
    [TC_PQFieldMasterId] INT          IDENTITY (1, 1) NOT NULL,
    [Field]              VARCHAR (50) NULL,
    [IsCompulsory]       TINYINT      NOT NULL,
    [IsActive]           BIT          CONSTRAINT [DF_TC_PQFieldMaster_IsActive] DEFAULT ((1)) NULL,
    PRIMARY KEY CLUSTERED ([TC_PQFieldMasterId] ASC)
);

