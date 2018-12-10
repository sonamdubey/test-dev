CREATE TABLE [dbo].[TC_BWLeadStatus] (
    [TC_BWLeadStatusId] INT          IDENTITY (1, 1) NOT NULL,
    [Description]       VARCHAR (50) NOT NULL,
    [IsActive]          BIT          NOT NULL,
    CONSTRAINT [PK_TC_BWLeadStatus] PRIMARY KEY CLUSTERED ([TC_BWLeadStatusId] ASC)
);

